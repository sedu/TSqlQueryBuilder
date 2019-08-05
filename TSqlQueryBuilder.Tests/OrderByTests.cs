using NUnit.Framework;

namespace TSqlQueryBuilder.Tests {
    [TestFixture]
    public class OrderByTests: BaseTest {
        [Test]
        public void OrderBy() {
            string expectedQuery = @"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                ORDER BY [TestTable].[Id] DESC
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                )
                .OrderBy(
                    ord => ord
                        .Desc(f => f.Id)
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void OrderByMultipleColumns() {
            string expectedQuery = @"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                ORDER BY [TestTable].[Id] DESC, [TestTable].[CreationDate] ASC
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                )
                .OrderBy(
                    ord => ord
                        .Desc(f => f.Id)
                        .Asc(f => f.CreationDate)
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }
    }
}
