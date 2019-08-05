using System;

namespace TSqlQueryBuilder {
    public class FieldSelectItem : ISelectItem {
        public Field Field { get; }
        public FieldSelectItem(Field field) {
            Field = field ?? throw new ArgumentNullException(nameof(field));
        }
        public string Compile() {
            string result = Field.GetFullName();
            if (Field.Alias != null) {
                return $"{result} {TSqlSyntax.As} [{Field.Alias}]";
            }
            return result;
        }
    }
}
