using System;
using TSqlQueryBuilder.Helpers;

namespace TSqlQueryBuilder {
    public class DeleteClause : Clause {
        public string TableName { get; }

        public DeleteClause(string tableName) {
            if (string.IsNullOrWhiteSpace(tableName)) {
                throw new ArgumentException($"Parameter {nameof(tableName)} can't be null or empty.", nameof(tableName));
            }
            TableName = tableName;
        }

        public override TSqlQuery Compile(ClauseCompilationContext context) {
            string query = $"{TSqlSyntax.Delete} {TSqlSyntax.From} {SqlBuilderHelper.PrepareTableName(TableName)}";
            return new TSqlQuery(query);
        }
    }
}
