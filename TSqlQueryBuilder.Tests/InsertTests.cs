using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TSqlQueryBuilder.Tests {
    [TestFixture]
    public class InsertTests : BaseTest {
        [Test]
        public void Insert() {
            string expectedQuery = @"
                INSERT [TestTable] (Title, FloatVal, DecimalVal, CreationDate)
                VALUES(
                    @TestTable_Title,
                    @TestTable_FloatVal,
                    @TestTable_DecimalVal,
                    @TestTable_CreationDate
                )
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Title", "testTitle" },
                    { "TestTable_FloatVal", 3.14F },
                    { "TestTable_DecimalVal", 2.71m },
                    { "TestTable_CreationDate", new DateTime(2016, 12, 19, 11, 46, 59) }
            };

            TSqlBuilder builder = new TSqlBuilder();

            builder.Insert<TestTable>(
                ins => ins
                    .Set(f => f.Title, "testTitle")
                    .Set(f => f.FloatVal, 3.14F)
                    .Set(f => f.DecimalVal, 2.71m)
                    .Set(f => f.CreationDate, new DateTime(2016, 12, 19, 11, 46, 59))
            );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void InsertWithColumnSpecifiedMoreThanOnce() {
            TSqlBuilder builder = new TSqlBuilder();

            Assert.Throws<ArgumentException>(() => {
                builder.Insert<TestTable>(
                    upd => upd
                        .Set(f => f.Title, "ORIGINAL")
                        .Set(f => f.Title, "DUPLICATE")
                );
            });
        }

        [Test]
        public void InsertWithTsqlStatement() {
            string expectedQuery = @"
                INSERT [TestTable] (Title, CreationDate)
                VALUES(
                    @TestTable_Title,
                    SYSUTCDATETIME()
                )
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Title", "testTitle" },
            };

            TSqlBuilder builder = new TSqlBuilder();

            builder.Insert<TestTable>(
                ins => ins
                    .Set(f => f.Title, "testTitle")
                    .Set(f => f.CreationDate, TSqlStatement.SysUtcDateTimeCall)
            );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void InsertWithDeclarationAsParameter() {
            string expectedQuery = @"
                INSERT [TestTable] (Title, CreationDate)
                VALUES(
                    @TestTable_Title,
                    SYSUTCDATETIME()
                )
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Title", "testTitle" },
            };

            TSqlBuilder builder = new TSqlBuilder();

            InsertDeclaration<TestTable> insertDecalration = new InsertDeclaration<TestTable>();
            insertDecalration.Set(f => f.Title, "testTitle");
            insertDecalration.Set(f => f.CreationDate, TSqlStatement.SysUtcDateTimeCall);

            builder.Insert<TestTable>(insertDecalration);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void InsertEnum() {
            string expectedQuery = @"
                INSERT [TableWithEnum] (Id, Name, Enum)
                VALUES(
                    @TableWithEnum_Id,
                    @TableWithEnum_Name,
                    @TableWithEnum_Enum
                )
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TableWithEnum_Id", 10 },
                    { "TableWithEnum_Name", "Test" },
                    { "TableWithEnum_Enum", TestEnum.One }
            };

            TSqlBuilder builder = new TSqlBuilder();

            builder.Insert<TableWithEnum>(
                ins => ins
                    .Set(f => f.Id, 10)
                    .Set(f => f.Name, "Test")
                    .Set(f => f.Enum, TestEnum.One)
            );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void InsertEnumAsString() {
            string expectedQuery = @"
                INSERT [TableWithEnum] (Id, Name, Enum)
                VALUES(
                    @TableWithEnum_Id,
                    @TableWithEnum_Name,
                    @TableWithEnum_Enum
                )
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TableWithEnum_Id", 10 },
                    { "TableWithEnum_Name", "Test" },
                    { "TableWithEnum_Enum", TestEnum.One.ToString() }
            };

            TSqlBuilder builder = new TSqlBuilder();

            builder.Insert<TableWithEnum>(
                ins => ins
                    .Set(f => f.Id, 10)
                    .Set(f => f.Name, "Test")
                    .Set(f => f.Enum, TestEnum.One.ToString())
            );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }

        [Test]
        public void InsertEnumAsInt() {
            string expectedQuery = @"
                INSERT [TableWithEnum] (Id, Name, Enum)
                VALUES(
                    @TableWithEnum_Id,
                    @TableWithEnum_Name,
                    @TableWithEnum_Enum
                )
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TableWithEnum_Id", 10 },
                    { "TableWithEnum_Name", "Test" },
                    { "TableWithEnum_Enum", (int)TestEnum.One }
            };

            TSqlBuilder builder = new TSqlBuilder();

            builder.Insert<TableWithEnum>(
                ins => ins
                    .Set(f => f.Id, 10)
                    .Set(f => f.Name, "Test")
                    .Set(f => f.Enum, (int)TestEnum.One)
            );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.AreEquivalent(expectedParameters, actualQuery.Parameters);
        }
    }
}