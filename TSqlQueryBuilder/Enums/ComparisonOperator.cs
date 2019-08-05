using System.ComponentModel;

namespace TSqlQueryBuilder {
    public enum ComparisonOperator {
        [Description("=")]
        Equal = 0,

        [Description(">")]
        Greater = 1,

        [Description("<")]
        Less = 2,

        [Description(">=")]
        GreaterOrEqual = 3,

        [Description("<=")]
        LessOrEqual = 4,

        [Description("!=")]
        NotEqual = 5
    }
}
