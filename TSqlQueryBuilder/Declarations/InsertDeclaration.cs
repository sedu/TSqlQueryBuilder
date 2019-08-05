using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TSqlQueryBuilder.Helpers;

namespace TSqlQueryBuilder {
    public class InsertDeclaration<T> {
        private readonly Dictionary<string, object> _valueByField;

        public InsertDeclaration() {
            _valueByField = new Dictionary<string, object>();
        }

        public Dictionary<string, object> GetFields() {
            return new Dictionary<string, object>(_valueByField);
        }

        public InsertDeclaration<T> Set(Expression<Func<T, object>> fieldExpression, object value) {
            if (fieldExpression == null) {
                throw new ArgumentNullException(nameof(fieldExpression));
            }

            string fieldName = SqlBuilderHelper.GetMemberNameFromExpression(fieldExpression);

            if (_valueByField.ContainsKey(fieldName)) {
                throw new ArgumentException($"The column name '{fieldName}' is specified more than once in the column list of an INSERT. A column cannot be assigned more than one value in the same clause.");
            }

            _valueByField.Add(fieldName, value);

            return this;
        }
    }
}