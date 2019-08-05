using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TSqlQueryBuilder.Helpers {
    internal static class ReflectionHelper {
        public static IEnumerable<string> GetPropertieNames<T>() {
            Type type = typeof(T);
            return type.GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(p => p.Name);
        }
        public static object GetValue<T>(T obj, string propertyName) {
            Type type = typeof(T);
            return type.GetTypeInfo().GetProperty(propertyName).GetValue(obj);
        }
    }
}
