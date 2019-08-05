using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TSqlQueryBuilder {
    public class WhereFollowingBuilder<TSource> {
        protected readonly List<Clause> _clauses;

        public WhereFollowingBuilder(List<Clause> clauses) {
            _clauses = clauses;
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
