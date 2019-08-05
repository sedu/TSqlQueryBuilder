using NUnit.Framework;

namespace TSqlQueryBuilder.Tests {
    [TestFixture]
    public class SetTransactionIsolationLevelTests : BaseTest {
        [Test]
        public void SetTransactionIsolationLevel() {
            string expectedQuery = @"
                SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .SetTransactionIsolationLevel(TransactionIsolationLevel.ReadUncommitted)
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }
    }
}