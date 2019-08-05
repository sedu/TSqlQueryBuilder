namespace TSqlQueryBuilder {
    public class UpdateClauseItem {
        public string FieldName { get; }
        public object Value { get; }
        public AssignmentOperator AssignmentOperator { get; }

        public UpdateClauseItem(string fieldName, object value, AssignmentOperator assignmentOperator) {
            FieldName = fieldName;
            Value = value;
            AssignmentOperator = assignmentOperator;
        }
    }
}
