using TSqlQueryBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TSqlQueryBuilder {
    public class WhereDeclaration<TSource> {
        private readonly List<Clause> _clauses;

        public WhereDeclaration() {
            _clauses = new List<Clause>();
        }

        public List<Clause> GetClauses() {
            return new List<Clause>(_clauses);
        }

        public WhereDeclaration<TSource> In<P>(Expression<Func<TSource, P>> memberExpression, IEnumerable<P> value) {
            return In<TSource, P>(memberExpression, value);
        }
        public WhereDeclaration<TSource> In<TCustom, TValue>(Expression<Func<TCustom, TValue>> memberExpression, IEnumerable<TValue> value) {
            InClause<TCustom, TValue> clause = new InClause<TCustom, TValue>(memberExpression, value);
            _clauses.Add(clause);
            return this;
        }

        public WhereDeclaration<TSource> Comparison(Expression<Func<TSource, object>> binaryExpression) {
            return Comparison<TSource>(binaryExpression);
        }
        public WhereDeclaration<TSource> Comparison<TCustom>(Expression<Func<TCustom, object>> binaryExpression) {
            ComparisonClause clause = SqlBuilderHelper.ConvertExpressionToBinaryOperationClause<TCustom>(binaryExpression);
            _clauses.Add(clause);
            return this;
        }
        public WhereDeclaration<TSource> Comparison(Expression<Func<TSource, object>> fieldSelector, ComparisonOperator comparisonOperator, object value) {
            return Comparison<TSource>(fieldSelector, comparisonOperator, value);
        }
        public WhereDeclaration<TSource> Comparison<TCustom>(Expression<Func<TCustom, object>> fieldSelector, ComparisonOperator comparisonOperator, object value) {
            ComparisonClause clause = new ComparisonClause(
                    comparisonOperator,
                    typeof(TCustom).Name,
                    SqlBuilderHelper.GetMemberNameFromExpression(fieldSelector),
                    value
                );
            _clauses.Add(clause);
            return this;
        }

        public WhereDeclaration<TSource> And(params Expression<Func<WhereDeclaration<TSource>, WhereDeclaration<TSource>>>[] expressions) {
            return And<TSource>(expressions);
        }
        public WhereDeclaration<TSource> And<TCustom>(params Expression<Func<WhereDeclaration<TCustom>, WhereDeclaration<TCustom>>>[] expressions) {
            return BooleanClause(LogicalOperator.And, expressions);
        }

        public WhereDeclaration<TSource> Or(params Expression<Func<WhereDeclaration<TSource>, WhereDeclaration<TSource>>>[] expressions) {
            return Or<TSource>(expressions);
        }
        public WhereDeclaration<TSource> Or<TCustom>(params Expression<Func<WhereDeclaration<TCustom>, WhereDeclaration<TCustom>>>[] expressions) {
            return BooleanClause(LogicalOperator.Or, expressions);
        }

        private WhereDeclaration<TSource> BooleanClause<TCustom>(LogicalOperator booleanOperation, IEnumerable<Expression<Func<WhereDeclaration<TCustom>, WhereDeclaration<TCustom>>>> expressions) {
            WhereDeclaration<TCustom> nestedDeclaration = new WhereDeclaration<TCustom>();
            foreach (var expres in expressions) {
                nestedDeclaration = expres.Compile().Invoke(nestedDeclaration);
            }
            _clauses.Add(new LogicalClause(booleanOperation, nestedDeclaration.GetClauses()));
            return this;
        }
    }
}