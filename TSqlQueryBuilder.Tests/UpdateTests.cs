using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TSqlQueryBuilder.Tests {
    [TestFixture]
    public class UpdateTests : BaseTest {
        [Test]
        public void Update() {
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
                    .Set(f => f.Id, 1)
                    .Set(f => f.Title, "testTitle")
            );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void UpdateWhere() {
            string expectedQuery = @"
                UPDATE [TestTable]
                SET
                    [TestTable].[Id] = @TestTable_Id,
                    [TestTable].[Title] = @TestTable_Title
                WHERE [TestTable].[Id] = @TestTable_Id_1
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Id", 1 },
                    { "TestTable_Title", "testTitle" },
                    { "TestTable_Id_1", 10 },
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder.Update<TestTable>(
                upd => upd
                    .Set(f => f.Id, 1)
                    .Set(f => f.Title, "testTitle")
            ).Where(t => t.Id == 10);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void CompoundAssignment() {
            string expectedQuery = @"
                UPDATE [TestTable]
                SET
                    [TestTable].[Id] *= @TestTable_Id,
                    [TestTable].[Title] += @TestTable_Title,
                    [TestTable].[DecimalVal] = @TestTable_DecimalVal
                WHERE [TestTable].[Id] = @TestTable_Id_1
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Id", 1 },
                    { "TestTable_Title", "testTitle" },
                    { "TestTable_Id_1", 10 },
                    { "TestTable_DecimalVal", 3.14m }
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder.Update<TestTable>(
                upd => upd
                    .Set(f => f.Id, 1, AssignmentOperator.Multiplication)
                    .Set(f => f.Title, "testTitle", AssignmentOperator.Addition)
                    .Set(f => f.DecimalVal, 3.14m)
            ).Where(t => t.Id == 10);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void UpdateWithColumnSpecifiedMoreThanOnce() {
            TSqlBuilder builder = new TSqlBuilder();

            Assert.Throws<ArgumentException>(() => {
                builder.Update<TestTable>(
                    upd => upd
                        .Set(f => f.Title, "ORIGINAL")
                        .Set(f => f.Title, "DUPLICATE")
                );
            });
        }

        [Test]
        public void UpdateWithTsqlStaement() {
            string expectedQuery = @"
                UPDATE [TestTable]
                SET
                    [TestTable].[Title] = @TestTable_Title,
                    [TestTable].[CreationDate] = SYSUTCDATETIME()
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Title", "testTitle" },
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder.Update<TestTable>(
                upd => upd
                    .Set(f => f.Title, "testTitle")
                    .Set(f => f.CreationDate, TSqlStatement.SysUtcDateTimeCall)
            );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void UpdateWithDeclarationAsParameter() {
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

            UpdateDeclaration<TestTable> updDeclaration = new UpdateDeclaration<TestTable>();

            updDeclaration.Set(f => f.Id, 1);
            updDeclaration.Set(f => f.Title, "testTitle");

            builder.Update<TestTable>(updDeclaration);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void UpdateTableWithEnumField() {
            string expectedQuery = @"
                UPDATE [TableWithEnum]
                SET
                    [TableWithEnum].[Enum] = @TableWithEnum_Enum
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TableWithEnum_Enum", TestEnum.One }
            };

            TSqlBuilder builder = new TSqlBuilder();

            builder.Update<TableWithEnum>(
                upd => upd
                    .Set(f => f.Enum, TestEnum.One)
            );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void UpdateTableWithEnumFieldAsString() {
            string expectedQuery = @"
                UPDATE [TableWithEnum]
                SET
                    [TableWithEnum].[Enum] = @TableWithEnum_Enum
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TableWithEnum_Enum", TestEnum.One.ToString() }
            };

            TSqlBuilder builder = new TSqlBuilder();

            builder.Update<TableWithEnum>(
                upd => upd
                    .Set(f => f.Enum, TestEnum.One.ToString())
            );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void UpdateTableWithEnumFieldAsInt() {
            string expectedQuery = @"
                UPDATE [TableWithEnum]
                SET
                    [TableWithEnum].[Enum] = @TableWithEnum_Enum
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TableWithEnum_Enum", (int)TestEnum.One }
            };

            TSqlBuilder builder = new TSqlBuilder();

            builder.Update<TableWithEnum>(
                upd => upd
                    .Set(f => f.Enum, (int)TestEnum.One)
            );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }
    }
}