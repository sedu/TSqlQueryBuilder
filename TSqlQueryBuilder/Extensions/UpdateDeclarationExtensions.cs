using System;
using System.Linq.Expressions;

namespace TSqlQueryBuilder {
    public static class UpdateDeclarationExtensions {
        public static UpdateDeclaration<T> Set<T>(this UpdateDeclaration<T> declaration, Expression<Func<T, object>> fieldExpression, object value, Func<object, bool> setCondition) {
            if (setCondition(value)) {
                return declaration.Set(fieldExpression, value);
            }
            return declaration;
        }
    }
}