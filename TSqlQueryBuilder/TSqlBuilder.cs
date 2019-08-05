using TSqlQueryBuilder.Clauses;
using TSqlQueryBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TSqlQueryBuilder {
    public class TSqlBuilder {
        private readonly List<Clause> _clauses;
        private readonly ClauseCompilationContext _clauseCompilationContext;

        public TSqlBuilder() {
            _clauses = new List<Clause>();
            _clauseCompilationContext = new ClauseCompilationContext();
        }

        public TSqlBuilder SetTransactionIsolationLevel(TransactionIsolationLevel transactionIsolationLevel) {
            var clause = new TransactionIsolationLevelClause(transactionIsolationLevel);
            _clauses.Add(clause);
            return this;
        }

        public SelectFollowingBuilder<T> Select<T>() {
            return Select<T>(Enumerable.Empty<Field>());
        }
        public SelectFollowingBuilder<T> Select<T>(TableHint hints) {
            return Select<T>(hints, Enumerable.Empty<Field>());
        }
        public SelectFollowingBuilder<T> Select<T>(IEnumerable<Expression<Func<T, object>>> fieldSelectors) {
            IEnumerable<Field> fields = fieldSelectors.Select(
                f => new Field(typeof(T).Name, SqlBuilderHelper.GetMemberNameFromExpression(f))
            );
            return Select<T>(fields);
        }
        public SelectFollowingBuilder<T> Select<T>(TableHint hints, IEnumerable<Expression<Func<T, object>>> fieldSelectors) {
            IEnumerable<Field> fields = fieldSelectors.Select(
                f => new Field(typeof(T).Name, SqlBuilderHelper.GetMemberNameFromExpression(f))
            );
            return Select<T>(hints, fields);
        }
        public SelectFollowingBuilder<T> Select<T>(params Expression<Func<T, object>>[] fieldSelectors) {
            return Select<T>((IEnumerable<Expression<Func<T, object>>>)fieldSelectors);
        }
        public SelectFollowingBuilder<T> Select<T>(TableHint hints, params Expression<Func<T, object>>[] fieldSelectors) {
            return Select<T>(hints, (IEnumerable<Expression<Func<T, object>>>)fieldSelectors);
        }
        public SelectFollowingBuilder<T> Select<T>(IEnumerable<Field> fields) {
            return Select<T>(null, fields);
        }
        public SelectFollowingBuilder<T> Select<T>(TableHint? hints, IEnumerable<Field> fields) {
            IEnumerable<Field> actualFieldList = fields;

            if (actualFieldList == null || !actualFieldList.Any()) {
                actualFieldList = ReflectionHelper.GetPropertieNames<T>().Select(f => new Field(typeof(T).Name, f));
            }

            SelectClause<T> clause = new SelectClause<T>(actualFieldList.Select(f => new FieldSelectItem(f)), hints);
            _clauses.Add(clause);

            return new SelectFollowingBuilder<T>(_clauses);
        }
        public SelectFollowingBuilder<T> Select<T>(params Field[] fields) {
            return Select<T>((IEnumerable<Field>)fields);
        }
        public SelectFollowingBuilder<T> Select<T>(TableHint hints, params Field[] fields) {
            return Select<T>(hints, (IEnumerable<Field>)fields);
        }
        public SelectFollowingBuilder<T> Select<T>(SelectDeclaration<T> declaration) {
            return Select<T>(null, declaration);
        }
        public SelectFollowingBuilder<T> Select<T>(TableHint? hints, SelectDeclaration<T> declaration) {
            SelectClause<T> clause = new SelectClause<T>(declaration.GetSelectItems(), hints);
            clause.TopCount = declaration.TopCount;
            clause.Distinct = declaration.DistinctSelect;
            _clauses.Add(clause);

            return new SelectFollowingBuilder<T>(_clauses);
        }
        public SelectFollowingBuilder<T> Select<T>(Expression<Func<SelectDeclaration<T>, SelectDeclaration<T>>> expression) {
            return Select<T>(null, expression);
        }
        public SelectFollowingBuilder<T> Select<T>(TableHint? hints, Expression<Func<SelectDeclaration<T>, SelectDeclaration<T>>> expression) {
            SelectDeclaration<T> declaration = new SelectDeclaration<T>();
            declaration = expression.Compile().Invoke(declaration);
            return Select<T>(hints, declaration);
        }

        public void Insert<T>(InsertDeclaration<T> declaration) {
            InsertClause insertClause = new InsertClause(typeof(T).Name, declaration.GetFields());
            _clauses.Add(insertClause);
        }
        public void Insert<T>(Expression<Func<InsertDeclaration<T>, InsertDeclaration<T>>> expression) {
            InsertDeclaration<T> declaration = new InsertDeclaration<T>();
            declaration = expression.Compile().Invoke(declaration);
            Insert<T>(declaration);
        }

        public UpdateFollowingBuilder<T> Update<T>(UpdateDeclaration<T> declaration) {
            UpdateClause updateClause = new UpdateClause(typeof(T).Name, declaration.GetUpdateItems());
            _clauses.Add(updateClause);

            return new UpdateFollowingBuilder<T>(_clauses);
        }
        public UpdateFollowingBuilder<T> Update<T>(Expression<Func<UpdateDeclaration<T>, UpdateDeclaration<T>>> expression) {
            UpdateDeclaration<T> declaration = new UpdateDeclaration<T>();
            declaration = expression.Compile().Invoke(declaration);

            return Update(declaration);
        }

        public void SelectScopeIdentity() {
            _clauses.Add(new SelectScopeIdentityClause());
        }

        public void SelectRowCount() {
            _clauses.Add(new SelectRowCountClause());
        }

        public TSqlQuery CompileQuery() {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            StringBuilder queryString = new StringBuilder();
            foreach (Clause clause in _clauses) {
                TSqlQuery clauseQuery = clause.Compile(_clauseCompilationContext);
                queryString.AppendLine(clauseQuery.Query);
                foreach (KeyValuePair<string, object> item in clauseQuery.Parameters) {
                    parameters.Add(item.Key, item.Value);
                }
            }
            return new TSqlQuery(queryString.ToString(), parameters);
        }

        private Dictionary<string, object> GetFieldsToUpsertFromExpression<T>(IEnumerable<Expression<Func<T, object>>> fieldValues) {
            if (fieldValues == null || !fieldValues.Any()) {
                throw new ArgumentNullException(nameof(fieldValues));
            }

            Dictionary<string, object> propsToInsert = new Dictionary<string, object>();
            foreach (Expression<Func<T, object>> item in fieldValues) {
                BinaryExpression binaryExp = SqlBuilderHelper.ConvertToBinaryExpression(item);
                string fieldName = SqlBuilderHelper.GetMemberNameFromExpression(binaryExp.Left);
                object value = SqlBuilderHelper.GetRightValueFromBinaryExpression(binaryExp);
                propsToInsert.Add(fieldName, value);
            }

            return propsToInsert;
        }
    }
}