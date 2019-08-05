using System.Text;
using TSqlQueryBuilder.Extensions;

namespace TSqlQueryBuilder.Clauses {
    public class TransactionIsolationLevelClause : Clause {
        private readonly TransactionIsolationLevel _transactionIsolationLevel;
        public TransactionIsolationLevelClause(TransactionIsolationLevel transactionIsolationLevel) {
            _transactionIsolationLevel = transactionIsolationLevel;
        }
        public override TSqlQuery Compile(ClauseCompilationContext context) {
            string query = $"{TSqlSyntax.SetTransactionIsolationLevel} {_transactionIsolationLevel.GetDescription()};";

            return new TSqlQuery(query);
        }
    }
}