using System.ComponentModel;

namespace TSqlQueryBuilder {
    public enum AssignmentOperator {
        /// <summary>
        /// Operator = (EQUALS). Basic assignment. 
        /// </summary>
        [Description("=")]
        Basic = 1,

        /// <summary>
        /// Operator += (Add EQUALS). Adds some amount to the original value and sets the original value to the result.
        /// </summary>
        [Description("+=")]
        Addition = 2,

        /// <summary>
        /// Operator -= (Subtract EQUALS). Subtracts some amount from the original value and sets the original value to the result.
        /// </summary>
        [Description("-=")]
        Subtraction = 3,

        /// <summary>
        /// Operator *= (Multiply EQUALS). Multiplies by an amount and sets the original value to the result.
        /// </summary>
        [Description("*=")]
        Multiplication = 4,

        /// <summary>
        /// Operator /= (Divide EQUALS). Divides by an amount and sets the original value to the result.
        /// </summary>
        [Description("/=")]
        Division = 5,

        /// <summary>
        /// Operator %= (Modulo EQUALS). Divides by an amount and sets the original value to the modulo.
        /// </summary>
        [Description("%=")]
        Modulo = 6,

        /// <summary>
        /// Operator &amp;= (Bitwise AND EQUALS). Performs a bitwise AND and sets the original value to the result.
        /// </summary>
        [Description("&=")]
        BitwiseAnd = 7,

        /// <summary>
        /// Operator |= (Bitwise OR EQUALS). Performs a bitwise OR and sets the original value to the result.
        /// </summary>
        [Description("|=")]
        BitwiseOr = 8,

        /// <summary>
        /// Operator ^= (Bitwise Exclusive OR EQUALS). Performs a bitwise exclusive OR and sets the original value to the result.
        /// </summary>
        [Description("^=")]
        BitwiseExclusiveOr = 9
    }
}
