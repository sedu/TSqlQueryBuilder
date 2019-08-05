using System.Collections.Generic;

namespace TSqlQueryBuilder {
    public class ClauseCompilationContext {
        public HashSet<string> ParameterNames { get; }

        public ClauseCompilationContext() {
            ParameterNames = new HashSet<string>();
        }
    }
}