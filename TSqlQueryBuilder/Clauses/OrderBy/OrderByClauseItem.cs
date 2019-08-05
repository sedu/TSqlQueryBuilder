using TSqlQueryBuilder.Extensions;
using System;

namespace TSqlQueryBuilder {
    public class OrderByClauseItem {
        public Field Field { get; }
        public OrderDirection OrderDirection { get; }

        public OrderByClauseItem(Field field, OrderDirection orderDirection) {
            Field = field ?? throw new ArgumentNullException(nameof(field));
            OrderDirection = orderDirection;
        }

        public string Compile() {
            string fieldName = Field.Alias ?? Field.GetFullName();
            return $"{fieldName} {OrderDirection.GetDescription()}";
        }
    }
}
