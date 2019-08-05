using TSqlQueryBuilder.Extensions;

namespace TSqlQueryBuilder {
    public class SelectScopeIdentityClause : Clause {
        public override TSqlQuery Compile(ClauseCompilationContext context) {
            return new TSqlQuery($"{TSqlSyntax.Select} {TSqlStatement.ScopeIdentityCall.GetDescription()}",null);
        }
    }
}