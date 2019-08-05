using System.ComponentModel;

namespace TSqlQueryBuilder {
    public enum TransactionIsolationLevel {
        [Description("READ UNCOMMITTED")]
        ReadUncommitted = 1,

        [Description("READ COMMITTED")]
        ReadCommitted,

        [Description("REPEATABLE READ")]
        RepeatableRead,

        [Description("SNAPSHOT")]
        Snapshot,

        [Description("SERIALIZABLE")]
        Serializable
    }
}