using TSqlQueryBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TSqlQueryBuilder {
    public abstract class FollowingWhere<T> {
        protected readonly List<Clause> _clauses;
        
        public FollowingWhere(List<Clause> clauses) {
            _clauses = clauses;
        }

        public WhereFollowingBuilder<T> Where(Clause clause) {
            _clauses.Add(new WhereClause(clause));
            return new WhereFollowingBuilder<T>(_clauses);
        }

        public WhereFollowingBuilder<T> Where(Expression<Func<T, object>> binaryExpression) {
            UnaryExpression unaryExp = (binaryExpression.Body as UnaryExpression);

            if (unaryExp == null) {
                throw new UnsupportedExpressionException(nameof(UnaryExpression));
            }

            BinaryExpression binaryExp = (unaryExp.Operand as BinaryExpression);

            if (binaryExp == null) {
                throw new UnsupportedExpressionException(nameof(BinaryExpression), "Invalid operand type.");
            }

            return Where(CreateClauseFromExpression(binaryExp));
        }

        public WhereFollowingBuilder<T> Where(Expression<Func<WhereDeclaration<T>, WhereDeclaration<T>>> expression) {
            WhereDeclaration<T> whereClauseDeclaration = new WhereDeclaration<T>();
            whereClauseDeclaration = expression.Compile().Invoke(whereClauseDeclaration);
            return Where(whereClauseDeclaration);
        }

        public WhereFollowingBuilder<T> Where(WhereDeclaration<T> declaration) {
            return Where(declaration.GetClauses().Single());
        }

        private Clause CreateClauseFromExpression(BinaryExpression binaryExp) {
            if (binaryExp.NodeType == ExpressionType.AndAlso || binaryExp.NodeType == ExpressionType.OrElse) {
                BinaryExpression leftExpression = binaryExp.Left as BinaryExpression;
                if (leftExpression == null) {
                    throw new UnsupportedExpressionException("Expression in left side is invalid.", nameof(BinaryExpression));
                }
                BinaryExpression rightsExpression = binaryExp.Right as BinaryExpression;
                if (rightsExpression == null) {
                    throw new UnsupportedExpressionException("Expression in right side is invalid.", nameof(BinaryExpression));
                }

                return new LogicalClause(
                    SqlBuilderHelper.MapExpressionTypeToBooleanOperation(binaryExp.NodeType),
                    CreateClauseFromExpression(leftExpression),
                    CreateClauseFromExpression(rightsExpression)
                );
            }

            return new ComparisonClause(
                SqlBuilderHelper.MapExpressionTypeToBinaryOperation(binaryExp.NodeType),
                SqlBuilderHelper.GetClassNameFromExpression(binaryExp.Left),
                SqlBuilderHelper.GetMemberNameFromExpression(binaryExp.Left),
                SqlBuilderHelper.GetRightValueFromBinaryExpression(binaryExp)
            );
        }
    }
}