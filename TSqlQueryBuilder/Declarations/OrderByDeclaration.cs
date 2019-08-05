using TSqlQueryBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TSqlQueryBuilder {
    public class OrderByDeclaration<TSource> {
        private readonly List<OrderByClauseItem> _orderItems;

        public OrderByDeclaration() {
            _orderItems = new List<OrderByClauseItem>();
        }

        public List<OrderByClauseItem> GetOrderItems() {
            return new List<OrderByClauseItem>(_orderItems);
        }

        public OrderByDeclaration<TSource> Desc(Expression<Func<TSource, object>> fieldSelector) {
            return Desc<TSource>(fieldSelector);
        }
        public OrderByDeclaration<TSource> Desc<TCustom>(Expression<Func<TCustom, object>> fieldSelector) {
            return OrderBy<TCustom>(fieldSelector, OrderDirection.Desc);
        }
        public OrderByDeclaration<TSource> Asc(Expression<Func<TSource, object>> fieldSelector) {
            return Asc<TSource>(fieldSelector);
        }
        public OrderByDeclaration<TSource> Asc<TCustom>(Expression<Func<TCustom, object>> fieldSelector) {
            return OrderBy<TCustom>(fieldSelector, OrderDirection.Asc);
        }
        public OrderByDeclaration<TSource> OrderBy(Expression<Func<TSource, object>> fieldSelector, OrderDirection orderDirection) {
            return OrderBy<TSource>(fieldSelector, orderDirection);
        }
        public OrderByDeclaration<TSource> OrderBy<TCustom>(Expression<Func<TCustom, object>> fieldSelector, OrderDirection orderDirection) {
            Field field = new Field(typeof(TCustom).Name, SqlBuilderHelper.GetMemberNameFromExpression(fieldSelector));
            _orderItems.Add(new OrderByClauseItem(field, orderDirection));
            return this;
        }
    }
}
