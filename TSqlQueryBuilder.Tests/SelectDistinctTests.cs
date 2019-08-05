using NUnit.Framework;

namespace TSqlQueryBuilder.Tests {
    [TestFixture]
    public class SelectDistinctTests: BaseTest {
        [Test]
        public void SelectDistict() {
            string expectedQuery = @"
                SELECT DISTINCT
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    sel => sel
                        .Distinct()
                        .Field(f => f.Id)
                        .Field(f => f.Title)
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void SelectDistictFromJoined() {
            string expectedQuery = @"
                SELECT DISTINCT
                    [TestTable].[Id],
                    [AnotherTable].[Name]
                FROM [TestTable]
                INNER JOIN [AnotherTable] ON [AnotherTable].[Name] = [TestTable].[Title]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    sel => sel
                        .Distinct()
                        .Field(f => f.Id)
                        .Field<AnotherTable>(f => f.Name)
                )
                .Join<AnotherTable>((joined, main) => joined.Name == main.Title);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }
    }
}
