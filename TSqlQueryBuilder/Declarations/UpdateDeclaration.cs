using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TSqlQueryBuilder.Helpers;
using System.Linq;

namespace TSqlQueryBuilder {
    public class UpdateDeclaration<T> {
        private readonly List<UpdateClauseItem> _updateItems;

        public UpdateDeclaration() {
            _updateItems = new List<UpdateClauseItem>();
        }

        public List<UpdateClauseItem> GetUpdateItems() {
            return new List<UpdateClauseItem>(_updateItems);
        }

        public UpdateDeclaration<T> Set(Expression<Func<T, object>> fieldExpression, object value) {
            return Set(fieldExpression, value, AssignmentOperator.Basic);
        }
        public UpdateDeclaration<T> Set(Expression<Func<T, object>> fieldExpression, object value, AssignmentOperator assignmentOperator) {
            if (fieldExpression == null) {
                throw new ArgumentNullException(nameof(fieldExpression));
            }

            string fieldName = SqlBuilderHelper.GetMemberNameFromExpression(fieldExpression);

            if (_updateItems.Any(u => string.Compare(u.FieldName, fieldName, true) == 0)) {
                throw new ArgumentException($"The column '{fieldName}' is specified more than once in the UPDATE clause. A column cannot be assigned more than one value in the same clause.");
            }
            _updateItems.Add(new UpdateClauseItem(fieldName, value, assignmentOperator));

            return this;
        }
    }
}