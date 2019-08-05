using System.ComponentModel;

namespace TSqlQueryBuilder {
    public enum TSqlStatement {
        [Description("SCOPE_IDENTITY()")]
        ScopeIdentityCall = 1,

        [Description("@@ROWCOUNT")]
        RowCountVariable,

        [Description("SYSUTCDATETIME()")]
        SysUtcDateTimeCall,
    }
}