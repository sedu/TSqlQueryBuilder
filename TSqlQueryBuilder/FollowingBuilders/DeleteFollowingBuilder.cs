using System.Collections.Generic;

namespace TSqlQueryBuilder.FollowingBuilders {
    public class DeleteFollowingBuilder<T> : FollowingWhere<T> {
        public DeleteFollowingBuilder(List<Clause> clauses) : base(clauses) { }
    }
}
