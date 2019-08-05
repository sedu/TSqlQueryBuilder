using System.ComponentModel;

namespace TSqlQueryBuilder {
    public enum JoinType {
        [Description(TSqlSyntax.Inner)]
        Inner = 1,

        [Description(TSqlSyntax.Left)]
        Left = 2,

        [Description(TSqlSyntax.LeftOuter)]
        LeftOuter = 3,

        [Description(TSqlSyntax.Right)]
        Right = 4,

        [Description(TSqlSyntax.RightOuter)]
        RightOuter = 5,

        [Description(TSqlSyntax.Full)]
        Full = 6,

        [Description(TSqlSyntax.FullOuter)]
        FullOuter = 7
    }
}
