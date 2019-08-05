using TSqlQueryBuilder.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSqlQueryBuilder {
    public class LogicalClause : Clause {
        public Clause[] InnerClauses { get; set; }
        public LogicalOperator BooleanOperation { get; set; }

        public LogicalClause(LogicalOperator operation, IEnumerable<Clause> innerClauses) {
            InnerClauses = innerClauses.ToArray();
            BooleanOperation = operation;
        }
        public LogicalClause(LogicalOperator operation, params Clause[] innerClauses) {
            InnerClauses = innerClauses.ToArray();
            BooleanOperation = operation;
        }

        public override TSqlQuery Compile(ClauseCompilationContext context) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            StringBuilder queryString = new StringBuilder();

            foreach (Clause clause in InnerClauses) {
                if (queryString.Length > 0) {
                    queryString.Append($" {SqlBuilderHelper.ConvertBooleanOperationToString(BooleanOperation)} ");
                }
                TSqlQuery clauseQuery = clause.Compile(context);
                queryString.Append(clauseQuery.Query);
                foreach (KeyValuePair<string, object> item in clauseQuery.Parameters) {
                    parameters.Add(item.Key, item.Value);
                }
            }
            return new TSqlQuery($"({queryString})", parameters);
        }
    }
}
