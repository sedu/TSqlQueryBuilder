using System;
using System.Collections.Generic;

namespace TSqlQueryBuilder {
    public class TSqlQuery {
        public string Query { get; }
        public Dictionary<string, object> Parameters { get; }

        public TSqlQuery(string query): this(query, null) {}
        public TSqlQuery(string query, Dictionary<string, object> parameters) {
            if (string.IsNullOrWhiteSpace(query)) {
                throw new ArgumentNullException(nameof(query));
            }
            Query = query;
            if (parameters != null) {
                Parameters = new Dictionary<string, object>(parameters);
            } else {
                Parameters = new Dictionary<string, object>();
            }
        }
    }
}
