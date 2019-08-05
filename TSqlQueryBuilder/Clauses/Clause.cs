namespace TSqlQueryBuilder {
    public abstract class Clause {
        public abstract TSqlQuery Compile(ClauseCompilationContext context);
    }
}
