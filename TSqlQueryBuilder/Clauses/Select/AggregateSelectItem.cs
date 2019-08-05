using TSqlQueryBuilder.Extensions;
using System;

namespace TSqlQueryBuilder {
    public abstract class AggregateSelectItem : ISelectItem {
        public Field Field { get; }
        public string NoFieldReplacement { get; }
        public string Alias { get; }
        public AggregateFunction AggregateFunction { get; }

        public AggregateSelectItem(AggregateFunction aggregateFunction, Field field, string noFieldReplacement, string alias) {
            NoFieldReplacement = noFieldReplacement;
            AggregateFunction = aggregateFunction;
            Field = field;
            Alias = alias;
        }

        public string Compile() {
            string fieldString = Field == null ? NoFieldReplacement : Field.GetFullName();
            string result = $"{AggregateFunction.GetDescription()}({fieldString})";

            if (!string.IsNullOrWhiteSpace(Alias)) {
                result = $"{result} {TSqlSyntax.As} [{Alias}]";
            } 

            return result;
        }
    }

    public class FieldAggregateSelectItem : AggregateSelectItem {
        public FieldAggregateSelectItem(AggregateFunction aggregateFunction, Field field, string alias) : base(aggregateFunction, field, null, alias) {
            if (field == null) {
                throw new ArgumentNullException(nameof(field));
            }
        }
    }

    public class CountAggregateSelectItem : AggregateSelectItem {
        public CountAggregateSelectItem(Field field, string alias) : base(AggregateFunction.Count, field, TSqlSyntax.AllFieldsSymbol, alias) { }
    }
}
