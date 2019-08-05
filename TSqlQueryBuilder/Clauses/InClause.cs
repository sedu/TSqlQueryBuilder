using TSqlQueryBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TSqlQueryBuilder {
    public class InClause<TTable,TMember> : Clause {
        public Field Field { get; private set; }
        public IEnumerable<TMember> Value { get; private set; }

        public InClause(string tableName, string fieldName, IEnumerable<TMember> value) {
            Initialize(tableName, fieldName, value);
        }

        public InClause(Expression<Func<TTable, TMember>> memberExpression, IEnumerable<TMember> value) {
            string fieldName = SqlBuilderHelper.GetMemberNameFromExpression(memberExpression.Body);
            Initialize(typeof(TTable).Name, fieldName, value);
        }

        private void Initialize(string tableName, string fieldName, IEnumerable<TMember> value) {
            if (value == null || !value.Any()) {
                throw new ArgumentException($"Can't be null or empty.", nameof(value));
            }
            Field = new Field(tableName, fieldName);
            Value = value;
        }

        public override TSqlQuery Compile(ClauseCompilationContext context) {
            string parameterName = SqlBuilderHelper.ComposeParameterName(Field.TableName, Field.FieldName, context);
            context.ParameterNames.Add(parameterName);
            Dictionary<string, object> parameters = new Dictionary<string, object> {
                { parameterName, Value }
            };
            string query = $"{Field.GetFullName()} {TSqlSyntax.In} {SqlBuilderHelper.PrepareParameterName(parameterName)}";
            return new TSqlQuery(query, parameters);
        }
    }

    public class InClause : InClause<object, object> {
        public InClause(string tableName, string fieldName, IEnumerable<object> value) : base(tableName, fieldName, value) { }
    }
}