using TSqlQueryBuilder.Extensions;
using TSqlQueryBuilder.Helpers;
using System.Collections.Generic;
using System.Text;

namespace TSqlQueryBuilder {
    public class UpdateClause : Clause {
        public string TableName { get; }
        public IEnumerable<UpdateClauseItem> UpdateItems { get; }

        public UpdateClause(string tableName, IEnumerable<UpdateClauseItem> updateItems) {
            TableName = tableName;
            UpdateItems = updateItems;
        }
        public override TSqlQuery Compile(ClauseCompilationContext context) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            StringBuilder sb = new StringBuilder();

            foreach (UpdateClauseItem item in UpdateItems) {
                if (sb.Length > 0) {
                    sb.Append(TSqlSyntax.FieldsDelimeter);
                }

                string fieldName = SqlBuilderHelper.PrepareFieldName(TableName, item.FieldName);
                string assignmentOperatorString = SqlBuilderHelper.ConvertAssignmentOperatorToString(item.AssignmentOperator);
                string valueString;

                if (item.Value is TSqlStatement tsqlStatement) {
                    valueString = tsqlStatement.GetDescription();
                } else {
                    string parameterName = SqlBuilderHelper.ComposeParameterName(TableName, item.FieldName, context);
                    context.ParameterNames.Add(parameterName);
                    parameters.Add(parameterName, item.Value);
                    valueString = SqlBuilderHelper.PrepareParameterName(parameterName);
                }

                sb.AppendLine($"{fieldName} {assignmentOperatorString} {valueString}");
            }

            return new TSqlQuery(
                $"{TSqlSyntax.Update} {SqlBuilderHelper.PrepareTableName(TableName)} {TSqlSyntax.Set} {sb}",
                parameters
            );
        }
    }
}