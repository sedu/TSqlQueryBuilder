using System.Text.RegularExpressions;

namespace TSqlQueryBuilder.Tests {
    public abstract class BaseTest {
        protected string NormalizeSqlQuery(string query) {
            return string.IsNullOrEmpty(query) ? string.Empty : Regex.Replace(query, @"\s+", string.Empty).Trim().ToLowerInvariant();
        }
    }
}
