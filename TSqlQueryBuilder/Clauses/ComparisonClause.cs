using TSqlQueryBuilder.Extensions;
using TSqlQueryBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TSqlQueryBuilder {
    public class ComparisonClause<T> : Clause {
        static readonly HashSet<ComparisonOperator> NullValueComparisonOperators;

        static ComparisonClause() {
            NullValueComparisonOperators = new HashSet<ComparisonOperator> {
                ComparisonOperator.Equal,
                ComparisonOperator.NotEqual
            };
        }

        public Field Field { get; private set; }
        public object Value { get; private set; }
        public ComparisonOperator Operation { get; private set; }

        public ComparisonClause(ComparisonOperator operation, string tableName, string fieldName, object value) {
            Initialize(operation, tableName, fieldName, value);
        }
        public ComparisonClause(ComparisonOperator operation, Expression<Func<T, object>> fieldSelector, object value) {
            Initialize(
                operation,
                typeof(T).Name,
                SqlBuilderHelper.GetMemberNameFromExpression(fieldSelector),
                value
            );
        }
        public ComparisonClause(Expression<Func<T, object>> expression) {
            BinaryExpression binaryExp = SqlBuilderHelper.ConvertToBinaryExpression(expression);

            Initialize(
                SqlBuilderHelper.MapExpressionTypeToBinaryOperation(binaryExp.NodeType),
                typeof(T).Name,
                SqlBuilderHelper.GetMemberNameFromExpression(binaryExp.Left),
                SqlBuilderHelper.GetRightValueFromBinaryExpression(binaryExp)
            );
        }

        public override TSqlQuery Compile(ClauseCompilationContext context) {
            if (Value == null) {
                if (!NullValueComparisonOperators.Contains(Operation)) {
                    throw new InvalidOperationException($"Invalid operation for null value. Should be one of {string.Join(", ", NullValueComparisonOperators)}");
                }
                return GetNullValueQuery();
            }

            Dictionary<string, object> parameters = null;
            string valueString;

            if (Value is TSqlStatement tsqlStatement) {
                valueString = tsqlStatement.GetDescription();
            } else {
                string parameterName = SqlBuilderHelper.GetUniqueParameterName(SqlBuilderHelper.ComposeParameterName(Field.TableName, Field.FieldName), context);
                context.ParameterNames.Add(parameterName);

                parameters = new Dictionary<string, object> {
                    { parameterName, Value }
                };
                valueString = SqlBuilderHelper.PrepareParameterName(parameterName);
            }

            return new TSqlQuery(
                $"{Field.GetFullName()} {SqlBuilderHelper.ConvertBinaryOperationToString(Operation)} {valueString}",
                parameters
            );
        }

        private void Initialize(ComparisonOperator operation, string tableName, string fieldName, object value) {
            Field = new Field(tableName, fieldName);
            Value = value;
            Operation = operation;
        }

        private TSqlQuery GetNullValueQuery() {
            StringBuilder querySb = new StringBuilder();

            querySb.Append($"{Field.GetFullName()} {TSqlSyntax.Is}");
            if (Operation == ComparisonOperator.NotEqual) {
                querySb.Append(" ");
                querySb.Append($"{TSqlSyntax.Not}");
            }
            querySb.Append(" ");
            querySb.Append($"{TSqlSyntax.Null}");

            return new TSqlQuery(querySb.ToString());
        }
    }

    public class ComparisonClause : ComparisonClause<object> {
        public ComparisonClause(ComparisonOperator operation, string tableName, string fieldName, object value)
            : base(operation, tableName, fieldName, value) { }
    }
}