using TSqlQueryBuilder.Helpers;
using System;
using System.Linq.Expressions;

namespace TSqlQueryBuilder {
    public class Field {
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string Alias { get; set; }

        public Field(string tableName, string fieldName, string alias) {
            if (string.IsNullOrWhiteSpace(tableName)) {
                throw new ArgumentNullException(nameof(tableName));
            }
            if (string.IsNullOrWhiteSpace(fieldName)) {
                throw new ArgumentNullException(nameof(fieldName));
            }
            if (alias != null && string.IsNullOrWhiteSpace(alias)) {
                throw new ArgumentException($"Can't be empty or whitespace.", nameof(alias));
            }
            TableName = tableName;
            FieldName = fieldName;
            Alias = alias;
        }
        public Field(string tableName, string fieldName) : this(tableName, fieldName, null) { }

        public string GetFullName() {
            return SqlBuilderHelper.PrepareFieldName(TableName, FieldName);
        }
    }

    public class Field<T> : Field {
        public Field(Expression<Func<T, object>> expression, string alias)
            : base(typeof(T).Name, SqlBuilderHelper.GetMemberNameFromExpression(expression), alias) { }
        public Field(Expression<Func<T, object>> expression) : this(expression, null) { }
    }
}