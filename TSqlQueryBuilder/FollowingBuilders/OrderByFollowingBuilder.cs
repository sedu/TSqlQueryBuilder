using System.Collections.Generic;

namespace TSqlQueryBuilder {
    public class OrderByFollowingBuilder {
        protected readonly List<Clause> _clauses;

        public OrderByFollowingBuilder(List<Clause> clauses) {
            _clauses = clauses;
        }

        public void OffsetFetch(int offset, int fetch) {
            _clauses.Add(new OffsetFetchClause(offset, fetch));
        }
    }
}
