using System;

namespace TSqlQueryBuilder {
    public class OffsetFetchClause : Clause {
        public int Offset { get; }
        public int Fetch { get; }

        public OffsetFetchClause(int offset, int fetch) {
            if (offset < 0) {
                throw new ArgumentException("The offset specified in a OFFSET may not be negative.", nameof(offset));
            }
            if (fetch < 1) {
                throw new ArgumentException("The number of rows provided for a FETCH must be greater then zero.", nameof(fetch));
            }
            Offset = offset;
            Fetch = fetch;
        }

        public override TSqlQuery Compile(ClauseCompilationContext context) {
            string query = $"{TSqlSyntax.Offset} {Offset} {TSqlSyntax.Rows}" +
                           $" {TSqlSyntax.Fetch} {TSqlSyntax.Next} {Fetch} {TSqlSyntax.Rows} {TSqlSyntax.Only}";

            return new TSqlQuery(query);
        }
    }
}
