using TSqlQueryBuilder.Extensions;

namespace TSqlQueryBuilder {
    public class SelectRowCountClause : Clause {
        public override TSqlQuery Compile(ClauseCompilationContext context) {
            return new TSqlQuery($"{TSqlSyntax.Select} {TSqlStatement.RowCountVariable.GetDescription()}", null);
        }
    }
}