using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TSqlQueryBuilder.Tests {
    [TestFixture]
    public class SelectScopeIdentityTests: BaseTest {
        [Test]
        public void SelectScopeIdentity() {
            string expectedQuery = "SELECT SCOPE_IDENTITY()";
            TSqlBuilder builder = new TSqlBuilder();

            builder.SelectScopeIdentity();

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void InsertAndSelectScopeIdentity() {
            string expectedQuery = @"
                INSERT [TestTable] (Title, FloatVal, DecimalVal, CreationDate)
                VALUES(
                    @TestTable_Title,
                    @TestTable_FloatVal,
                    @TestTable_DecimalVal,
                    @TestTable_CreationDate
                )
                SELECT SCOPE_IDENTITY()
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder.Insert<TestTable>(
                ins => ins
                    .Set(f => f.Title, "testTitle")
                    .Set(f => f.FloatVal, 3.14F)
                    .Set(f => f.DecimalVal, 2.71m)
                    .Set(f => f.CreationDate, new DateTime(2016, 12, 19, 11, 46, 59))
            );
            builder.SelectScopeIdentity();

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
        }
    }
}