using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TSqlQueryBuilder.Tests {
    [TestFixture]
    public class SelectWhereTests : BaseTest {
        [Test]
        public void ConstantParameter() {
            string expectedQuery = @"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE [TestTable].[Id] = @TestTable_Id
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Id", 1 }
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(
                    f => f.Id == 1
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void VariableParameter() {
            int id = 1;

            string expectedQuery = @"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE [TestTable].[Id] = @TestTable_Id
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Id", id }
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(
                    f => f.Id == id
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void StringParameter() {
            string title = "sometitle";

            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE [TestTable].[Title] = @TestTable_Title
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Title", title }
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(f => f.Title == title);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void DateParameter() {
            DateTime date = new DateTime(2016, 1, 1);

            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE [TestTable].[CreationDate] = @TestTable_CreationDate
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_CreationDate", date }
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(f => f.CreationDate == date);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void FloatParameter() {
            float floatVal = 3.14F;

            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE [TestTable].[FloatVal] = @TestTable_FloatVal
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_FloatVal", floatVal }
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(f => f.FloatVal == floatVal);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void DecimalParameter() {
            decimal decimalVal = 3.14m;

            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE [TestTable].[DecimalVal] = @TestTable_DecimalVal
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_DecimalVal", decimalVal }
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(f => f.DecimalVal == decimalVal);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void And() {
            int id = 10;
            string title = "sometitle";
            float floatVal = 3.14F;
            decimal decimalVal = 2.71m;
            DateTime date = new DateTime(2016, 1, 1);

            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE (
                    [TestTable].[CreationDate] = @TestTable_CreationDate
                    AND [TestTable].[Id] = @TestTable_Id
                    AND [TestTable].[Title] = @TestTable_Title
                    AND [TestTable].[FloatVal] = @TestTable_FloatVal
                    AND [TestTable].[DecimalVal] = @TestTable_DecimalVal
                )
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Id", id },
                    { "TestTable_Title", title },
                    { "TestTable_FloatVal", floatVal },
                    { "TestTable_DecimalVal", decimalVal },
                    { "TestTable_CreationDate", date }
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(
                    b => b.And(
                        f => f.Comparison(n => n.CreationDate == date),
                        f => f.Comparison(n => n.Id == id),
                        f => f.Comparison(n => n.Title == title),
                        f => f.Comparison(n => n.FloatVal == floatVal),
                        f => f.Comparison(n => n.DecimalVal == decimalVal)
                    )
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void And_NewNotation() {
            int id = 10;
            string title = "sometitle";
            float floatVal = 3.14F;
            decimal decimalVal = 2.71m;
            DateTime date = new DateTime(2016, 1, 1);

            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE (
                            (
                                (
                                    (
                                        [TestTable].[CreationDate] = @TestTable_CreationDate
                                        AND [TestTable].[Id] = @TestTable_Id
                                    )
                                    AND [TestTable].[Title] = @TestTable_Title
                                )
                                AND [TestTable].[FloatVal] = @TestTable_FloatVal
                            )
                            AND [TestTable].[DecimalVal] = @TestTable_DecimalVal
                    )
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Id", id },
                    { "TestTable_Title", title },
                    { "TestTable_FloatVal", floatVal },
                    { "TestTable_DecimalVal", decimalVal },
                    { "TestTable_CreationDate", date }
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(
                    f => f.CreationDate == date
                    && f.Id == id
                    && f.Title == title
                    && f.FloatVal == floatVal
                    && f.DecimalVal == decimalVal
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void Or() {
            int id = 10;
            string title = "sometitle";
            float floatVal = 3.14F;
            decimal decimalVal = 2.71m;
            DateTime date = new DateTime(2016, 1, 1);

            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE (
                    [TestTable].[CreationDate] = @TestTable_CreationDate
                    OR [TestTable].[Id] = @TestTable_Id
                    OR [TestTable].[Title] = @TestTable_Title
                    OR [TestTable].[FloatVal] = @TestTable_FloatVal
                    OR [TestTable].[DecimalVal] = @TestTable_DecimalVal
                )
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Id", id },
                    { "TestTable_Title", title },
                    { "TestTable_FloatVal", floatVal },
                    { "TestTable_DecimalVal", decimalVal },
                    { "TestTable_CreationDate", date }
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(
                    b => b.Or(
                        f => f.Comparison(n => n.CreationDate == date),
                        f => f.Comparison(n => n.Id == id),
                        f => f.Comparison(n => n.Title == title),
                        f => f.Comparison(n => n.FloatVal == floatVal),
                        f => f.Comparison(n => n.DecimalVal == decimalVal)
                    )
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void Nested() {
            int id = 10;
            string title = "sometitle";
            float floatVal = 3.14F;
            decimal decimalVal = 2.71m;
            DateTime date = new DateTime(2016, 1, 1);

            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE (
                    [TestTable].[CreationDate] = @TestTable_CreationDate
                    OR [TestTable].[Id] = @TestTable_Id
                    OR [TestTable].[Title] = @TestTable_Title
                    OR (
                        [TestTable].[FloatVal] > @TestTable_FloatVal
                        AND [TestTable].[DecimalVal] < @TestTable_DecimalVal
                        AND (
                            [TestTable].[FloatVal] <= @TestTable_FloatVal_1
                            OR [TestTable].[DecimalVal] >= @TestTable_DecimalVal_1
                        )
                    )
                )
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Id", id },
                    { "TestTable_Title", title },
                    { "TestTable_FloatVal", floatVal },
                    { "TestTable_DecimalVal", decimalVal },
                    { "TestTable_FloatVal_1", floatVal },
                    { "TestTable_DecimalVal_1", decimalVal },
                    { "TestTable_CreationDate", date }
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(
                    b => b.Or(
                        f => f.Comparison(n => n.CreationDate == date),
                        f => f.Comparison(n => n.Id == id),
                        f => f.Comparison(n => n.Title == title),
                        nb => nb.And(
                                f => f.Comparison(n => n.FloatVal > floatVal),
                                f => f.Comparison(n => n.DecimalVal < decimalVal),
                                nnb => nnb.Or(
                                    f => f.Comparison(n => n.FloatVal <= floatVal),
                                    f => f.Comparison(n => n.DecimalVal >= decimalVal)
                                )
                        )
                    )
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void Nested_NewNotation() {
            int id = 10;
            string title = "sometitle";
            float floatVal = 3.14F;
            decimal decimalVal = 2.71m;
            DateTime date = new DateTime(2016, 1, 1);

            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE (
		                (
			                (
				                [TestTable].[CreationDate] = @TestTable_CreationDate OR [TestTable].[Id] = @TestTable_Id
			                )
			                OR [TestTable].[Title] = @TestTable_Title
		                )
		                OR (
			                (
				                [TestTable].[FloatVal] > @TestTable_FloatVal
				                AND [TestTable].[DecimalVal] > @TestTable_DecimalVal
			                ) 
			                AND (
				                [TestTable].[FloatVal] <= @TestTable_FloatVal_1
				                OR [TestTable].[DecimalVal] >= @TestTable_DecimalVal_1
			                )
		                )
                )
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Id", id },
                    { "TestTable_Title", title },
                    { "TestTable_FloatVal", floatVal },
                    { "TestTable_DecimalVal", decimalVal },
                    { "TestTable_FloatVal_1", floatVal },
                    { "TestTable_DecimalVal_1", decimalVal },
                    { "TestTable_CreationDate", date }
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(
                    f =>
                        f.CreationDate == date
                        || f.Id == id
                        || f.Title == title
                        || (
                            f.FloatVal > floatVal
                            && f.DecimalVal > decimalVal
                            && (
                                f.FloatVal <= floatVal
                                || f.DecimalVal >= decimalVal
                            )
                        )
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void AnotherTableInWhere() {
            int id = 10;
            string name = "someName";

            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                INNER JOIN [AnotherTable] ON [AnotherTable].[Id] = [TestTable].[Id]
                WHERE (
                    [TestTable].[Id] = @TestTable_Id
                    AND [AnotherTable].[Name] = @AnotherTable_Name
                )
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Id", id },
                    { "AnotherTable_Name", name },
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                )
                .Join<AnotherTable>((joined, main) => joined.Id == main.Id)
                .Where(
                    b => b.And(
                        f => f.Comparison<TestTable>(n => n.Id == id),
                        f => f.Comparison<AnotherTable>(n => n.Name == name)
                    )
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void WhereIn() {
            int[] values = new int[] { 1, 2, 3, 4, 5 };

            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE [TestTable].[Id] IN @TestTable_Id
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Id", values }
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(
                    d => d.In(f => f.Id, values)
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void ConditionalWhere() {
            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE (
                    [TestTable].[Title] = @TestTable_Title
                    OR (
                        [TestTable].[Id] > @TestTable_Id
                        AND [TestTable].[Id] < @TestTable_Id_1
                        AND [TestTable].[Title] IN @TestTable_Title_1
                    )
                )
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Title", "Test" },
                    { "TestTable_Id", 1 },
                    { "TestTable_Id_1", 10 },
                    { "TestTable_Title_1", new [] { "one", "two", "three" } }
            };

            List<Clause> clauses = new List<Clause>();

            bool conditionOne = true;
            if (conditionOne) {
                clauses.Add(
                    new ComparisonClause<TestTable>(t => t.Title == "Test")
                );
            }

            bool conditionTwo = false;
            if (!conditionTwo) {
                clauses.Add(
                    new LogicalClause(
                        LogicalOperator.And,
                        new ComparisonClause<TestTable>(t => t.Id > 1),
                        new ComparisonClause<TestTable>(t => t.Id < 10),
                        new InClause<TestTable, string>(t => t.Title, new[] { "one", "two", "three" })
                    )
                );
            }

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(
                    new LogicalClause(LogicalOperator.Or, clauses)
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void WhereIsNull() {
            string expectedQuery = @"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE [TestTable].[FloatVal] IS NULL
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(
                    f => f.FloatVal == null
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void WhereIsNotNull() {
            string expectedQuery = @"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE [TestTable].[FloatVal] IS NOT NULL
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(
                    f => f.FloatVal != null
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void InvalidOperatorWithNull() {
            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(
                    f => f.FloatVal > null
                );

            Assert.Throws<InvalidOperationException>(() => {
                builder.CompileQuery();
            });
        }

        [Test]
        public void WhereIsNullWithParameter() {
            string expectedQuery = @"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE [TestTable].[Title] IS NULL
            ";

            string title = null;
            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(
                    f => f.Title == title
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void WhereWithTsqlStatement() {
            string expectedQuery = @"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE [TestTable].[Id] = SCOPE_IDENTITY()
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(
                    whr => whr
                        .Comparison(f => f.Id, ComparisonOperator.Equal, TSqlStatement.ScopeIdentityCall)
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void WhereWithKeywords() {
            string expectedQuery = @"
                SELECT 
                    [ThirdTable].[Key],
                    [ThirdTable].[Index]
                FROM [ThirdTable]
                WHERE [ThirdTable].[Index] = @ThirdTable_Index
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "ThirdTable_Index", 1 }
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<ThirdTable>(
                    f => f.Key,
                    f => f.Index
                ).Where(
                    f => f.Index == 1
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void WhereWithDerivedClass() {
            string expectedQuery = @"
                SELECT 
                    [TestTableDerived].[Title]
                FROM [TestTableDerived]
                WHERE [TestTableDerived].[Id] = @TestTableDerived_Id
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTableDerived_Id", 1 }
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTableDerived>(
                    f => f.Title
                ).Where(
                    f => f.Id == 1
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void AnotherDerivedTableInWhere() {
            int id = 10;
            string title = "someName";

            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                INNER JOIN [TestTableDerived] ON [TestTableDerived].[Id] = [TestTable].[Id]
                WHERE (
                    [TestTable].[Id] = @TestTable_Id
                    AND [TestTableDerived].[Title] = @TestTableDerived_Title
                )
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Id", id },
                    { "TestTableDerived_Title", title },
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                )
                .Join<TestTableDerived>((joined, main) => joined.Id == main.Id)
                .Where(
                    b => b.And(
                        f => f.Comparison<TestTable>(n => n.Id == id),
                        f => f.Comparison<TestTableDerived>(n => n.Title == title)
                    )
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void WhereWithDeclarationAsParameter() {
            string expectedQuery = @"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE [TestTable].[Id] = @TestTable_Id
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Id", 1 }
            };

            WhereDeclaration<TestTable> whereDeclaration = new WhereDeclaration<TestTable>();
            whereDeclaration.Comparison(f => f.Id == 1);

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(whereDeclaration);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void WhereCondifionalOperation() {
            string expectedQuery = @"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                WHERE [TestTable].[CreationDate] >= sysutcdatetime()
            ";

            bool condition = true;
            var comparisionOperator = condition
                ? ComparisonOperator.GreaterOrEqual
                : ComparisonOperator.Less;

            ComparisonClause<TestTable> clause = new ComparisonClause<TestTable>(
                                  comparisionOperator,
                                  f => f.CreationDate,
                                  TSqlStatement.SysUtcDateTimeCall
            );

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                ).Where(clause);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
        }
    }
}