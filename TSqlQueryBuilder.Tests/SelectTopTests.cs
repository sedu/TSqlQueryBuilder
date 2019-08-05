using NUnit.Framework;
using System;

namespace TSqlQueryBuilder.Tests {
    [TestFixture]
    public class SelectTopTests: BaseTest {
        [Test]
        public void SelectTop() {
            string expectedQuery = @"
                SELECT TOP 10
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    sel => sel
                        .Top(10)
                        .Field(f => f.Id)
                        .Field(f => f.Title)
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void SelectTopLessThenOne() {
            TSqlBuilder builder = new TSqlBuilder();

            Assert.Throws<ArgumentException>(() => {
                builder
                    .Select<TestTable>(
                        sel => sel
                            .Top(-1)
                            .Field(f => f.Id)
                            .Field(f => f.Title)
                    );
            });
        }
    }
}
