using NUnit.Framework;

namespace TSqlQueryBuilder.Tests {
    [TestFixture]
    public class SelectAggregationTests: BaseTest {
        [Test]
        public void SelectAggregation() {
            string expectedQuery = @"
                SELECT 
                    COUNT(*)
                FROM [TestTable]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    sel => sel
                        .Count()
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void SelectAggregationByField() {
            string expectedQuery = @"
                SELECT 
                    SUM([TestTable].[FloatVal])
                FROM [TestTable]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    sel => sel
                        .Sum(f => f.FloatVal)

                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void SelectAggregationAndField() {
            string expectedQuery = @"
                SELECT
                    [TestTable].[Id],
                    COUNT(*)
                FROM [TestTable]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    sel => sel
                        .Field(f => f.Id)
                        .Count()
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void SelectAggregationFromJoinedTable() {
            string expectedQuery = @"
                SELECT 
                    MIN([AnotherTable].[Id])
                FROM [TestTable]
                INNER JOIN [AnotherTable] ON [AnotherTable].[Id] = [TestTable].[Id]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    sel => sel
                        .Min<AnotherTable>(f => f.Id)
                )
                .Join<AnotherTable>((joined, main) => joined.Id == main.Id);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void SelectAggregationWithAlias() {
            string expectedQuery = @"
                SELECT 
                    COUNT(*) AS [CustomName]
                FROM [TestTable]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    sel => sel
                        .Count("CustomName")
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void SelectAggregationByFieldWithAlias() {
            string expectedQuery = @"
                SELECT 
                    AVG([TestTable].[DecimalVal]) AS [CustomName]
                FROM [TestTable]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    sel => sel
                        .Avg(f => f.DecimalVal, "CustomName")
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void CountAggregation() {
            string expectedQuery = @"
                SELECT 
                    COUNT(*),
                    COUNT(*) AS [Count],
                    COUNT([TestTable].[Id]),
                    COUNT([TestTable].[Id]) AS [CountById],
                    COUNT([AnotherTable].[Id]),
                    COUNT([AnotherTable].[Id]) AS [AnotherTableCountById]
                FROM [TestTable]
                INNER JOIN [AnotherTable] ON [AnotherTable].[Id] = [TestTable].[Id]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    sel => sel
                        .Count()
                        .Count("Count")
                        .Count(f => f.Id)
                        .Count(f => f.Id, "CountById")
                        .Count<AnotherTable>(f => f.Id)
                        .Count<AnotherTable>(f => f.Id, "AnotherTableCountById")
                )
                .Join<AnotherTable>((joined, main) => joined.Id == main.Id);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void SumAggregation() {
            string expectedQuery = @"
                SELECT 
                    SUM([TestTable].[Id]),
                    SUM([TestTable].[FloatVal]) AS [MySum],
                    SUM([AnotherTable].[Id]),
                    SUM([AnotherTable].[Id]) AS [AnotherTableSum]
                FROM [TestTable]
                INNER JOIN [AnotherTable] ON [AnotherTable].[Id] = [TestTable].[Id]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    sel => sel
                        .Sum(f => f.Id)
                        .Sum(f => f.FloatVal, "MySum")
                        .Sum<AnotherTable>(f => f.Id)
                        .Sum<AnotherTable>(f => f.Id, "AnotherTableSum")
                )
                .Join<AnotherTable>((joined, main) => joined.Id == main.Id);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void AvgAggregation() {
            string expectedQuery = @"
                SELECT 
                    AVG([TestTable].[Id]),
                    AVG([TestTable].[FloatVal]) AS [Avg],
                    AVG([AnotherTable].[Id]),
                    AVG([AnotherTable].[Id]) AS [AnotherTableAvg]
                FROM [TestTable]
                INNER JOIN [AnotherTable] ON [AnotherTable].[Id] = [TestTable].[Id]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    sel => sel
                        .Avg(f => f.Id)
                        .Avg(f => f.FloatVal, "Avg")
                        .Avg<AnotherTable>(f => f.Id)
                        .Avg<AnotherTable>(f => f.Id, "AnotherTableAvg")
                )
                .Join<AnotherTable>((joined, main) => joined.Id == main.Id);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void MinAggregation() {
            string expectedQuery = @"
                SELECT 
                    MIN([TestTable].[Id]),
                    MIN([TestTable].[FloatVal]) AS [Min],
                    MIN([AnotherTable].[Id]),
                    MIN([AnotherTable].[Id]) AS [AnotherTableMin]
                FROM [TestTable]
                INNER JOIN [AnotherTable] ON [AnotherTable].[Id] = [TestTable].[Id]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    sel => sel
                        .Min(f => f.Id)
                        .Min(f => f.FloatVal, "Min")
                        .Min<AnotherTable>(f => f.Id)
                        .Min<AnotherTable>(f => f.Id, "AnotherTableMin")
                )
                .Join<AnotherTable>((joined, main) => joined.Id == main.Id);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void MaxAggregation() {
            string expectedQuery = @"
                SELECT 
                    MAX([TestTable].[Id]),
                    MAX([TestTable].[FloatVal]) AS [Max],
                    MAX([AnotherTable].[Id]),
                    MAX([AnotherTable].[Id]) AS [AnotherTableMax]
                FROM [TestTable]
                INNER JOIN [AnotherTable] ON [AnotherTable].[Id] = [TestTable].[Id]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    sel => sel
                        .Max(f => f.Id)
                        .Max(f => f.FloatVal, "Max")
                        .Max<AnotherTable>(f => f.Id)
                        .Max<AnotherTable>(f => f.Id, "AnotherTableMax")
                )
                .Join<AnotherTable>((joined, main) => joined.Id == main.Id);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }
    }
}
