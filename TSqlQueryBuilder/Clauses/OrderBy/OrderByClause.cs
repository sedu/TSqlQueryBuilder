using System;
using System.Collections.Generic;
using System.Linq;

namespace TSqlQueryBuilder {
    public class OrderByClause : Clause {
        private readonly IEnumerable<OrderByClauseItem> _orderItems;

        public OrderByClause(IEnumerable<OrderByClauseItem> orderItems) {
            if (orderItems == null) {
                throw new ArgumentNullException(nameof(orderItems));
            }
            if (!orderItems.Any()) {
                throw new ArgumentException("Can't be empty.", nameof(orderItems));
            }
            _orderItems = orderItems;
        }

        public override TSqlQuery Compile(ClauseCompilationContext context) {
            IEnumerable<string> items = _orderItems.Select(i => i.Compile());
            return new TSqlQuery(
                $"{TSqlSyntax.OrderBy} {string.Join(TSqlSyntax.FieldsDelimeter, items)}"
            );
        }
    }
}
