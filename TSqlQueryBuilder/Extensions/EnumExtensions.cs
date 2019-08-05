using System;
using System.Reflection;
using System.ComponentModel;

namespace TSqlQueryBuilder.Extensions {
    internal static class EnumExtensions {
        public static string GetDescription<T>(this T enumeration) where T : struct {
            Type type = enumeration.GetType();

            if (!type.GetTypeInfo().IsEnum) {
                throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumeration));
            }

            MemberInfo[] memberInfo = type.GetTypeInfo().GetMember(enumeration.ToString());
            if (memberInfo != null && memberInfo.Length > 0) {
                DescriptionAttribute descriptionAttribute = memberInfo[0].GetCustomAttribute<DescriptionAttribute>();

                if (descriptionAttribute != null) {
                    return descriptionAttribute.Description;
                } 
            }

            return null;
        }
    }
}