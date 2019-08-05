using System.Collections.Generic;

namespace TSqlQueryBuilder {
    public class WhereClause : Clause {
        public Clause Clause { get; set; }

        public WhereClause(Clause clause) {
            //TODO check clause attrubute
            Clause = clause;
        }

        public override TSqlQuery Compile(ClauseCompilationContext context) {
            TSqlQuery innerQuery = Clause.Compile(context);
            return new TSqlQuery(
                $"{TSqlSyntax.Where} {innerQuery.Query}",
                new Dictionary<string, object>(innerQuery.Parameters)
            );
        }
    }
}