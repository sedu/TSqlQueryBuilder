using TSqlQueryBuilder.Extensions;
using TSqlQueryBuilder.Helpers;
using System.Text;

namespace TSqlQueryBuilder {
    public class JoinClause : Clause {
        public string JoinedTable { get; set; }
        public Field LeftField { get; set; }
        public Field RightField { get; set; }
        public ComparisonOperator Operation { get; set; }
        public TableHint? TableHints { get; set; }
        public JoinType JoinType { get; set; }

        public JoinClause(string joinedTable, Field leftField, Field rightField, ComparisonOperator operation, TableHint? tableHints)
            : this(joinedTable, leftField, rightField, operation, tableHints, JoinType.Inner) { }
        public JoinClause(string joinedTable, Field leftField, Field rightField, ComparisonOperator operation, TableHint? tableHints, JoinType joinType) {
            JoinedTable = joinedTable;
            LeftField = leftField;
            RightField = rightField;
            Operation = operation;
            TableHints = tableHints;
            JoinType = joinType;
        }

        public override TSqlQuery Compile(ClauseCompilationContext context) {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{JoinType.GetDescription()} {TSqlSyntax.Join} {SqlBuilderHelper.PrepareTableName(JoinedTable)}");
            if (TableHints.HasValue) {
                string hints = SqlBuilderHelper.GetTableHintString(TableHints.Value);
                sb.Append(" ");
                sb.Append($"{TSqlSyntax.With}({hints})");
            }
            sb.Append(" ");
            sb.Append($"{TSqlSyntax.On} {LeftField.GetFullName()} {SqlBuilderHelper.ConvertBinaryOperationToString(Operation)} {RightField.GetFullName()}");

            return new TSqlQuery(sb.ToString());
        }
    }
}