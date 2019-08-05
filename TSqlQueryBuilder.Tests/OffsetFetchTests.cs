using NUnit.Framework;
using System;

namespace TSqlQueryBuilder.Tests {
    [TestFixture]
    public class OffsetFetchTests: BaseTest {
        [Test]
        public void OffsetFetch() {
            string expectedQuery = @"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                ORDER BY [TestTable].[Id] DESC
                OFFSET 5 ROWS FETCH NEXT 100 ROWS ONLY
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
                )
                .OffsetFetch(5, 100);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void OffsetLessThanZero() {
            TSqlBuilder builder = new TSqlBuilder();

            Assert.Throws<ArgumentException>(() => {
                builder
                   .Select<TestTable>()
                   .OrderBy(
                       ord => ord
                           .Desc(f => f.Id)
                   )
                   .OffsetFetch(-1, 100);
            });
        }

        [Test]
        public void FetchLessThanOne() {
            TSqlBuilder builder = new TSqlBuilder();

            Assert.Throws<ArgumentException>(() => {
                builder
                   .Select<TestTable>()
                   .OrderBy(
                       ord => ord
                           .Desc(f => f.Id)
                   )
                   .OffsetFetch(0, 0);
            });
        }
    }
}
