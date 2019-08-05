using System.Collections.Generic;

namespace TSqlQueryBuilder {
    public class UpdateFollowingBuilder<T> : FollowingWhere<T> {
        public UpdateFollowingBuilder(List<Clause> clauses) : base(clauses) { }
    }
}