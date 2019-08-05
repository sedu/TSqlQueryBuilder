using System;

namespace TSqlQueryBuilder {
    public class UnsupportedExpressionException: Exception {
        public UnsupportedExpressionException(string expectedExpressionType) : this(null, expectedExpressionType) { }
        public UnsupportedExpressionException(string message, string expectedExpressionType) : base($"Expected expression type is {expectedExpressionType}. {message}") {
        }
    }
}