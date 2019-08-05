using System.ComponentModel;

namespace TSqlQueryBuilder {
    public enum AggregateFunction {
        [Description("COUNT")]
        Count = 1,

        [Description("AVG")]
        Avg,

        [Description("MIN")]
        Min,

        [Description("MAX")]
        Max,

        [Description("SUM")]
        Sum
    }
}
