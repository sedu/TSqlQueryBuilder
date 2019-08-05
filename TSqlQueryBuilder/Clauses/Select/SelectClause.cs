using TSqlQueryBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSqlQueryBuilder {
    public class SelectClause : Clause {
        private int? topCount;

        public int? TopCount {
            get { return topCount; }
            set {
                if (value <= 0) {
                    throw new ArgumentException($"The offset specified in a TOP must be greather then zero.");
                }
                topCount = value;
            }
        }
        public bool Distinct { get; set; }
        public string TableName { get; }
        public List<ISelectItem> SelectItems { get; }
        public TableHint? TableHints { get; }

        public SelectClause(string tableName, IEnumerable<ISelectItem> selectItems, TableHint? tableHints) {
            TableName = tableName;
            SelectItems = new List<ISelectItem>(selectItems);
            TableHints = tableHints;
        }
        public SelectClause(string tableName, IEnumerable<ISelectItem> selectItems): this(tableName, selectItems, null) {
            TableName = tableName;
            SelectItems = new List<ISelectItem>(selectItems);
        }

        public override TSqlQuery Compile(ClauseCompilationContext context) {
            string concatenatedFields = TSqlSyntax.AllFieldsSymbol;
            if (SelectItems != null & SelectItems.Any()) {
                concatenatedFields = string.Join(
                    TSqlSyntax.FieldsDelimeter,
                    SelectItems.Select(f => f.Compile())
                );
            }

            StringBuilder sb = new StringBuilder();
            sb.Append($"{TSqlSyntax.Select}");

            if (Distinct) {
                sb.Append(" ");
                sb.Append($"{TSqlSyntax.Distinct}");
            }
            if (topCount.HasValue) {
                sb.Append(" ");
                sb.Append($"{TSqlSyntax.Top} {topCount}");
            }
            sb.Append(" ");
            sb.Append($"{concatenatedFields} {TSqlSyntax.From} {SqlBuilderHelper.PrepareTableName(TableName)}");

            if (TableHints.HasValue) {
                string hints = SqlBuilderHelper.GetTableHintString(TableHints.Value);
                sb.Append(" ");
                sb.Append($"{TSqlSyntax.With}({hints})");
            }

            return new TSqlQuery(sb.ToString());
        }
    }

    public class SelectClause<T> : SelectClause {
        public SelectClause(IEnumerable<ISelectItem> fields, TableHint? hints) : base(typeof(T).Name, fields, hints) { }
        public SelectClause(IEnumerable<ISelectItem> fields) : this(fields, null) { }
    }
}