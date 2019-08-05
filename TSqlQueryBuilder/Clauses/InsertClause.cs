using TSqlQueryBuilder.Extensions;
using TSqlQueryBuilder.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSqlQueryBuilder {
    public class InsertClause : Clause {
        public string TableName { get; }
        public Dictionary<string, object> FieldWithValues { get; }

        public InsertClause(string tableName, Dictionary<string, object> fieldWithValues) {
            TableName = tableName;
            FieldWithValues = fieldWithValues;
        }

        public override TSqlQuery Compile(ClauseCompilationContext context) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            string properties = string.Join(TSqlSyntax.FieldsDelimeter, FieldWithValues.Keys);
            string values = string.Join(TSqlSyntax.FieldsDelimeter, FieldWithValues.Keys.Select(fieldName => {
                object fieldValue = FieldWithValues[fieldName];

                if (fieldValue is TSqlStatement tsqlStatement) {
                    return tsqlStatement.GetDescription();
                }

                string parameterName = SqlBuilderHelper.ComposeParameterName(TableName, fieldName, context);
                context.ParameterNames.Add(parameterName);
                parameters.Add(parameterName, FieldWithValues[fieldName]);

                return SqlBuilderHelper.PrepareParameterName(parameterName);
            }));

            StringBuilder sb = new StringBuilder();
            sb
                .AppendLine($"{TSqlSyntax.Insert} {SqlBuilderHelper.PrepareTableName(TableName)} ({properties})")
                .AppendLine($"{TSqlSyntax.Values} ({values})");

            return new TSqlQuery(sb.ToString(), parameters);
        }
    }
}