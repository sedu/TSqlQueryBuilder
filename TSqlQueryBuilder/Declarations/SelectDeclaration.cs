using TSqlQueryBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TSqlQueryBuilder {
    public class SelectDeclaration<TSource> {
        private readonly List<ISelectItem> _selectItems;

        public SelectDeclaration() {
            _selectItems = new List<ISelectItem>();
        }

        public int? TopCount { get; protected set; }
        public bool DistinctSelect { get; protected set; }
        public List<ISelectItem> GetSelectItems() {
            return new List<ISelectItem>(_selectItems);
        }

        public SelectDeclaration<TSource> Top(int top) {
            TopCount = top;
            return this;
        }
        public SelectDeclaration<TSource> Distinct() {
            DistinctSelect = true;
            return this;
        }

        public SelectDeclaration<TSource> AllFields() {
            AllFields<TSource>();
            return this;
        }
        public SelectDeclaration<TSource> AllFields<T>() {
            IEnumerable<Field> fieldList = ReflectionHelper
                .GetPropertieNames<T>()
                .Select(f => new Field(typeof(T).Name, f));

            IEnumerable<FieldSelectItem> fieldSelectItems = fieldList.Select(f => new FieldSelectItem(f));

            _selectItems.AddRange(fieldSelectItems);

            return this;
        }

        public SelectDeclaration<TSource> Field(Expression<Func<TSource, object>> fieldSelector) {
            Field(fieldSelector, null);
            return this;
        }
        public SelectDeclaration<TSource> Field(Expression<Func<TSource, object>> fieldSelector, string alias) {
            Field<TSource>(fieldSelector, alias);
            return this;
        }
        public SelectDeclaration<TSource> Field<T>(Expression<Func<T, object>> fieldSelector) {
            return Field<T>(fieldSelector, null);
        }
        public SelectDeclaration<TSource> Field<T>(Expression<Func<T, object>> fieldSelector, string alias) {
            Field field = new Field(typeof(T).Name, SqlBuilderHelper.GetMemberNameFromExpression(fieldSelector), alias);
            _selectItems.Add(new FieldSelectItem(field));
            return this;
        }

        public SelectDeclaration<TSource> Count() {
            return Count(string.Empty);
        }
        public SelectDeclaration<TSource> Count(string alias) {
            return Count<TSource>(null, alias);
        }
        public SelectDeclaration<TSource> Count(Expression<Func<TSource, object>> fieldSelector) {
            return Count<TSource>(fieldSelector);
        }
        public SelectDeclaration<TSource> Count(Expression<Func<TSource, object>> fieldSelector, string alias) {
            return Count<TSource>(fieldSelector, alias);
        }
        public SelectDeclaration<TSource> Count<TCustom>(Expression<Func<TCustom, object>> fieldSelector) {
            return Count<TCustom>(fieldSelector, null);
        }
        public SelectDeclaration<TSource> Count<TCustom>(Expression<Func<TCustom, object>> fieldSelector, string alias) {
            Field field = null;
            if (fieldSelector != null) {
                field = new Field(typeof(TCustom).Name, SqlBuilderHelper.GetMemberNameFromExpression(fieldSelector));
            }
            _selectItems.Add(new CountAggregateSelectItem(field, alias));
            return this;
        }

        public SelectDeclaration<TSource> Avg(Expression<Func<TSource, object>> fieldSelector) {
            return Avg(fieldSelector, null);
        }
        public SelectDeclaration<TSource> Avg(Expression<Func<TSource, object>> fieldSelector, string alias) {
            return Avg<TSource>(fieldSelector, alias);
        }
        public SelectDeclaration<TSource> Avg<TCustom>(Expression<Func<TCustom, object>> fieldSelector) {
            return Avg<TCustom>(fieldSelector, null);
        }
        public SelectDeclaration<TSource> Avg<TCustom>(Expression<Func<TCustom, object>> fieldSelector, string alias) {
            return FieldAggregation<TCustom>(AggregateFunction.Avg, fieldSelector, alias);
        }

        public SelectDeclaration<TSource> Min(Expression<Func<TSource, object>> fieldSelector) {
            return Min(fieldSelector, null);
        }
        public SelectDeclaration<TSource> Min(Expression<Func<TSource, object>> fieldSelector, string alias) {
            return Min<TSource>(fieldSelector, alias);
        }
        public SelectDeclaration<TSource> Min<TCustom>(Expression<Func<TCustom, object>> fieldSelector) {
            return Min<TCustom>(fieldSelector, null);
        }
        public SelectDeclaration<TSource> Min<TCustom>(Expression<Func<TCustom, object>> fieldSelector, string alias) {
            return FieldAggregation<TCustom>(AggregateFunction.Min, fieldSelector, alias);
        }

        public SelectDeclaration<TSource> Max(Expression<Func<TSource, object>> fieldSelector) {
            return Max(fieldSelector, null);
        }
        public SelectDeclaration<TSource> Max(Expression<Func<TSource, object>> fieldSelector, string alias) {
            return Max<TSource>(fieldSelector, alias);
        }
        public SelectDeclaration<TSource> Max<TCustom>(Expression<Func<TCustom, object>> fieldSelector) {
            return Max<TCustom>(fieldSelector, null);
        }
        public SelectDeclaration<TSource> Max<TCustom>(Expression<Func<TCustom, object>> fieldSelector, string alias) {
            return FieldAggregation<TCustom>(AggregateFunction.Max, fieldSelector, alias);
        }

        public SelectDeclaration<TSource> Sum(Expression<Func<TSource, object>> fieldSelector) {
            return Sum(fieldSelector, null);
        }
        public SelectDeclaration<TSource> Sum(Expression<Func<TSource, object>> fieldSelector, string alias) {
            return Sum<TSource>(fieldSelector, alias);
        }
        public SelectDeclaration<TSource> Sum<TCustom>(Expression<Func<TCustom, object>> fieldSelector) {
            return FieldAggregation<TCustom>(AggregateFunction.Sum, fieldSelector);
        }
        public SelectDeclaration<TSource> Sum<TCustom>(Expression<Func<TCustom, object>> fieldSelector, string alias) {
            return FieldAggregation<TCustom>(AggregateFunction.Sum, fieldSelector, alias);
        }

        protected SelectDeclaration<TSource> FieldAggregation<TCustom>(AggregateFunction aggregateFunction, Expression<Func<TCustom, object>> fieldSelector, string alias = null) {
            Field field = null;
            if (fieldSelector != null) {
                field = new Field(typeof(TCustom).Name, SqlBuilderHelper.GetMemberNameFromExpression(fieldSelector));
            }
            _selectItems.Add(new FieldAggregateSelectItem(aggregateFunction, field, alias));
            return this;
        }
    }
}
