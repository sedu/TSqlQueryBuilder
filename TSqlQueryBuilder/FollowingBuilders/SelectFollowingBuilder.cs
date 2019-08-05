using TSqlQueryBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TSqlQueryBuilder {
    public class SelectFollowingBuilder<TSource> : FollowingWhere<TSource> {
        public SelectFollowingBuilder(List<Clause> clauses): base(clauses) {}

        public SelectFollowingBuilder<TSource> Join<TJoined>(Expression<Func<TJoined, TSource, object>> expression) {
            return Join<TJoined, TSource>(expression);
        }
        public SelectFollowingBuilder<TSource> Join<TJoined, TMain>(Expression<Func<TJoined, TMain, object>> expression) {
            return Join<TJoined, TMain>(expression, JoinType.Inner);
        }

        public SelectFollowingBuilder<TSource> Join<TJoined>(Expression<Func<TJoined, TSource, object>> expression, JoinType joinType) {
            return Join<TJoined, TSource>(null, expression, joinType);
        }
        public SelectFollowingBuilder<TSource> Join<TJoined, TMain>(Expression<Func<TJoined, TMain, object>> expression, JoinType joinType) {
            return Join<TJoined, TMain>(null, expression, joinType);
        }

        public SelectFollowingBuilder<TSource> Join<TJoined>(TableHint tableHints, Expression<Func<TJoined, TSource, object>> expression) {
            return Join<TJoined, TSource>(tableHints, expression);
        }
        public SelectFollowingBuilder<TSource> Join<TJoined, TMain>(TableHint tableHints, Expression<Func<TJoined, TMain, object>> expression) {
            return Join<TJoined, TMain>(tableHints, expression, JoinType.Inner);
        }

        public SelectFollowingBuilder<TSource> Join<TJoined>(TableHint tableHints, Expression<Func<TJoined, TSource, object>> expression, JoinType joinType) {
            return Join<TJoined, TSource>(tableHints, expression, joinType);
        }
        public SelectFollowingBuilder<TSource> Join<TJoined, TMain>(TableHint tableHints, Expression<Func<TJoined, TMain, object>> expression, JoinType joinType) {
            return Join<TJoined, TMain>((TableHint?)tableHints, expression, joinType);
        }

        private SelectFollowingBuilder<TSource> Join<TJoined, TMain>(TableHint? tableHints, Expression<Func<TJoined, TMain, object>> expression, JoinType joinType) {
            var binExp = ((expression.Body as UnaryExpression).Operand as BinaryExpression);

            MemberExpression memberLeft = SqlBuilderHelper.ConvertToMemberExpression(binExp.Left);
            MemberExpression memberRight = SqlBuilderHelper.ConvertToMemberExpression(binExp.Right);

            JoinClause joinClause = new JoinClause(
                typeof(TJoined).Name,
                new Field(memberLeft.Expression.Type.Name, memberLeft.Member.Name),
                new Field(memberRight.Expression.Type.Name, memberRight.Member.Name),
                SqlBuilderHelper.MapExpressionTypeToBinaryOperation(binExp.NodeType),
                tableHints,
                joinType
            );

            _clauses.Add(joinClause);

            return this;
        }

        public OrderByFollowingBuilder OrderBy(Expression<Func<OrderByDeclaration<TSource>, OrderByDeclaration<TSource>>> expression) {
            return OrderBy<TSource>(expression);
        }
        public OrderByFollowingBuilder OrderBy<TCustom>(Expression<Func<OrderByDeclaration<TCustom>, OrderByDeclaration<TCustom>>> expression) {
            OrderByDeclaration<TCustom> declaration = new OrderByDeclaration<TCustom>();
            expression.Compile().Invoke(declaration);
            OrderByClause clause = new OrderByClause(declaration.GetOrderItems());
            _clauses.Add(clause);
            return new OrderByFollowingBuilder(_clauses);
        }
    }
}
