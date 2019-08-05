using NUnit.Framework;

namespace TSqlQueryBuilder.Tests {
    [TestFixture]
    public class SelectRowCountTests: BaseTest {
        [Test]
        public void SelectRowCount() {
            string expectedQuery = "SELECT @@ROWCOUNT";
            TSqlBuilder builder = new TSqlBuilder();

            builder.SelectRowCount();

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void UpdateAndSelectRowCount() {
            string expectedQuery = @"
                UPDATE [TestTable]
                SET
                    [TestTable].[Id] = @TestTable_Id,
                    [TestTable].[Title] = @TestTable_Title
                SELECT @@ROWCOUNT
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder.Update<TestTable>(
                upd => upd
                    .Set(f => f.Id, 1)
                    .Set(f => f.Title, "testTitle")
            );
            builder.SelectRowCount();

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
        }
    }
}