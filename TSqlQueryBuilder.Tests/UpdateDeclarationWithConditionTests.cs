using NUnit.Framework;
using System.Collections.Generic;

namespace TSqlQueryBuilder.Tests {
    class UpdateDeclarationWithConditionTests {
        [TestFixture]
        public class UpdateTests : BaseTest {
            [Test]
            public void UpdateIfFalseCondition() {
                string expectedQuery = @"
                UPDATE [TestTable]
                SET
                    [TestTable].[Title] = @TestTable_Title
                ";
                Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Title", "testTitle" },
                };

                TSqlBuilder builder = new TSqlBuilder();
                builder.Update<TestTable>(
                    upd => upd
                        .Set(f => f.Id, null, z => z != null)
                        .Set(f => f.Title, "testTitle")
                );

                TSqlQuery actualQuery = builder.CompileQuery();

                Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
                CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
            }

            [Test]
            public void UpdateIfTrueCondition() {
                string expectedQuery = @"
                UPDATE [TestTable]
                SET
                    [TestTable].[Id] = @TestTable_Id,
                    [TestTable].[Title] = @TestTable_Title
                ";
                Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Id", 1 },
                    { "TestTable_Title", "testTitle" },
                };

                TSqlBuilder builder = new TSqlBuilder();
                builder.Update<TestTable>(
                    upd => upd
                        .Set(f => f.Id, 1, z => z != null)
                        .Set(f => f.Title, "testTitle")
                );

                TSqlQuery actualQuery = builder.CompileQuery();

                Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
                CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
            }
        }
    }     
}