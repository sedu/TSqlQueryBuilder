using TSqlQueryBuilder.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TSqlQueryBuilder.Helpers {
    internal static class SqlBuilderHelper {
        public static string GetUniqueParameterName(string parameterName, ClauseCompilationContext context) {
            string key = parameterName;
            int keyReplicaNumber = 1;
            while (context.ParameterNames.Contains(key)) {
                key = $"{parameterName}_{keyReplicaNumber}";
                keyReplicaNumber++;
            }
            return key;
        }

        public static string ComposeParameterName(string tableName, string fieldName) {
            return $"{tableName}_{fieldName}";
        }

        public static string ComposeParameterName(string tableName, string fieldName, ClauseCompilationContext context) {
            string parameterName = ComposeParameterName(tableName, fieldName);
            return GetUniqueParameterName(parameterName, context);
        }

        public static string PrepareParameterName(string parameterName) {
            return $"@{parameterName}";
        }

        public static string PrepareTableName(string tableName) {
            return $"[{tableName}]";
        }

        public static string PrepareFieldName(string tableName, string fieldName) {
            if (string.IsNullOrWhiteSpace(tableName)) {
                return fieldName;
            }
            return $"{PrepareTableName(tableName)}.[{fieldName}]";
        }

        public static string GetMemberNameFromExpression<T>(Expression<Func<T, object>> fieldSelector) {
            MemberExpression memberExpression = ConvertToMemberExpression(fieldSelector.Body);
            return GetMemberNameFromExpression(memberExpression);
        }

        public static MemberExpression ConvertToMemberExpression(Expression expression) {
            if (expression is MemberExpression asMemberExpression) {
                return asMemberExpression;
            }

            if (expression is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression unaryAsMemeberExpression) {
                return unaryAsMemeberExpression;
            }

            throw new ArgumentException("Unsupported expression.");
        }

        public static string GetMemberNameFromExpression(Expression expression) {
            MemberExpression memberExpression = ExtractMemberExpression(expression);
            if (memberExpression != null) {
                return memberExpression.Member.Name;
            }
            throw new UnsupportedExpressionException(nameof(MemberExpression));
        }

        public static string GetClassNameFromExpression(Expression expression) {
            MemberExpression memberExpression = ExtractMemberExpression(expression);
            if (memberExpression != null) {                
                return memberExpression.Expression.Type.Name;
            }
            throw new UnsupportedExpressionException(nameof(MemberExpression));
        }

        public static ComparisonClause ConvertExpressionToBinaryOperationClause<T>(Expression<Func<T, object>> expression) {
            BinaryExpression binaryExp = ConvertToBinaryExpression(expression);

            return new ComparisonClause(
                    MapExpressionTypeToBinaryOperation(binaryExp.NodeType),
                    typeof(T).Name,
                    GetMemberNameFromExpression(binaryExp.Left),
                    GetRightValueFromBinaryExpression(binaryExp)
                );
        }

        public static BinaryExpression ConvertToBinaryExpression<T>(Expression<Func<T, object>> expression) {
            UnaryExpression unaryExp = (expression.Body as UnaryExpression);

            if (unaryExp == null) {
                throw new UnsupportedExpressionException(nameof(UnaryExpression));
            }

            BinaryExpression binaryExp = (unaryExp.Operand as BinaryExpression);

            if (binaryExp == null) {
                throw new UnsupportedExpressionException(nameof(BinaryExpression), "Invalid operand type.");
            }

            return binaryExp;
        }

        public static string ConvertBinaryOperationToString(ComparisonOperator binaryOperation) {
            if (!Enum.IsDefined(typeof(ComparisonOperator), binaryOperation)) {
                throw new ArgumentException("Unsupported binary operation.", nameof(binaryOperation));
            }
            return binaryOperation.GetDescription();
        }
        
        public static string ConvertBooleanOperationToString(LogicalOperator booleanOperation) {
            if (!Enum.IsDefined(typeof(LogicalOperator), booleanOperation)) {
                throw new ArgumentException("Unsupported boolead operation.", nameof(booleanOperation));
            }
            return booleanOperation.GetDescription();
        }

        public static string ConvertAssignmentOperatorToString(AssignmentOperator assignmentOperator) {
            if (!Enum.IsDefined(typeof(AssignmentOperator), assignmentOperator)) {
                throw new ArgumentException("Unsupported AssignmentOperator.", nameof(assignmentOperator));
            }
            return assignmentOperator.GetDescription();
        }

        public static object GetRightValueFromBinaryExpression(BinaryExpression binaryExp) {
            MemberExpression memberLeft = ExtractMemberExpression(binaryExp.Left);

            if (memberLeft != null && memberLeft.Expression is ParameterExpression) {
                Delegate valueGetter = Expression.Lambda(binaryExp.Right).Compile();
                return valueGetter.DynamicInvoke();
            } 

            throw new ArgumentException("Unsupported binary expression.");
        }

        public static ComparisonOperator MapExpressionTypeToBinaryOperation(ExpressionType expressionType) {
            switch (expressionType) {
                case ExpressionType.Equal: return ComparisonOperator.Equal;
                case ExpressionType.GreaterThan: return ComparisonOperator.Greater;
                case ExpressionType.GreaterThanOrEqual: return ComparisonOperator.GreaterOrEqual;
                case ExpressionType.LessThan: return ComparisonOperator.Less;
                case ExpressionType.LessThanOrEqual: return ComparisonOperator.LessOrEqual;
                case ExpressionType.NotEqual: return ComparisonOperator.NotEqual;
                default:
                    throw new ArgumentException($"Map for {expressionType} is not exist.");
            }
        }

        public static LogicalOperator MapExpressionTypeToBooleanOperation(ExpressionType expressionType) {
            switch (expressionType) {
                case ExpressionType.AndAlso: return LogicalOperator.And;
                case ExpressionType.OrElse: return LogicalOperator.Or;
                default:
                    throw new ArgumentException($"Map for {expressionType} is not exist.");
            }
        }

        public static string PrepareTableHintName(TableHint tableHint) {
            return tableHint.ToString().ToUpper();
        }

        public static IEnumerable<TableHint> GetTableHintList(TableHint tableHints) {
            List<TableHint> hintsToAdd = new List<TableHint>();

            foreach (TableHint hint in Enum.GetValues(typeof(TableHint))) {
                if (tableHints.HasFlag(hint)) {
                    hintsToAdd.Add(hint);
                }
            }

            return hintsToAdd;
        }

        public static string GetTableHintString(TableHint tableHints) {
            IEnumerable<TableHint> hintList = SqlBuilderHelper.GetTableHintList(tableHints);
            return string.Join(
                TSqlSyntax.FieldsDelimeter,
                hintList.Select(s => PrepareTableHintName(s))
            );
        }

        private static MemberExpression ExtractMemberExpression(Expression expression) {
            MemberExpression memberExpression = (expression as MemberExpression);

            if (memberExpression == null) {
                UnaryExpression unaryExpression = (expression as UnaryExpression);
                if (unaryExpression != null) {
                    memberExpression = (unaryExpression.Operand as MemberExpression);
                }
            }

            return memberExpression;
        }
    }
}