using System.ComponentModel;

namespace TSqlQueryBuilder {
    public enum LogicalOperator {
        [Description(TSqlSyntax.And)]
        And = 0,

        [Description(TSqlSyntax.Or)]
        Or = 1
    }
}