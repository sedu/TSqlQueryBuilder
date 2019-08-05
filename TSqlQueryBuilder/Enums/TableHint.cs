using System;

namespace TSqlQueryBuilder {
    [Flags]
    public enum TableHint {
        NoLock = 0x01
    }
}