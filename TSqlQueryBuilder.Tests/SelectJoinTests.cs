using NUnit.Framework;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TSqlQueryBuilder.Tests {
    [TestFixture]
    public class SelectJoinTests: BaseTest {
        [Test]
        public void Join() {
            TestJoinType(JoinType.Inner);
        }

        [Test]
        public void LeftJoin() {
            TestJoinType(JoinType.Left);
        }

        [Test]
        public void LeftOuterJoin() {
            TestJoinType(JoinType.LeftOuter);
        }

        [Test]
        public void RightJoin() {
            TestJoinType(JoinType.Right);
        }

        [Test]
        public void RightOuterJoin() {
            TestJoinType(JoinType.RightOuter);
        }

        [Test]
        public void FullJoin() {
            TestJoinType(JoinType.Full);
        }

        [Test]
        public void FullOuterJoin() {
            TestJoinType(JoinType.FullOuter);
        }

        [Test]
        public void JoinWhere() {
            int id = 1;
            string name = "test";

            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                INNER JOIN [AnotherTable] ON [AnotherTable].[Name] = [TestTable].[Title]
                WHERE (
                    [TestTable].[Id] = @TestTable_Id
                    AND [AnotherTable].[Name] = @AnotherTable_Name
                )
            ";
            Dictionary<string, object> expectedParameters = new Dictionary<string, object> {
                    { "TestTable_Id", id },
                    { "AnotherTable_Name", name }
            };

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                )
                .Join<AnotherTable>((joined, main) => joined.Name == main.Title)
                .Where(
                    b => b.And(
                        f => f.Comparison<TestTable>(n => n.Id == id),
                        f => f.Comparison<AnotherTable>(n => n.Name == name)
                    )
                );

            TSqlQuery actualQuery = builder.CompileQuery();
            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
        }

        [Test]
        public void MultipleJoin() {
            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                INNER JOIN [AnotherTable] ON [AnotherTable].[Name] = [TestTable].[Title]
                INNER JOIN [ThirdTable] ON [ThirdTable].[Id] = [TestTable].[Id]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                )
                .Join<AnotherTable>((joined, main) => joined.Name == main.Title)
                .Join<ThirdTable>((joined, main) => joined.Id == main.Id);

            TSqlQuery actualQuery = builder.CompileQuery();
            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
        }

        [Test]
        public void SelectFromJoinedTable() {
            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title],
                    [AnotherTable].[Id],
                    [AnotherTable].[Name]
                FROM [TestTable]
                INNER JOIN [AnotherTable] ON [AnotherTable].[Name] = [TestTable].[Title]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    sel => sel
                        .Field(t => t.Id)
                        .Field(t => t.Title)
                        .Field<AnotherTable>(t => t.Id)
                        .Field<AnotherTable>(t => t.Name)
                )
                .Join<AnotherTable>((joined, main) => joined.Name == main.Title);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
        }

        [Test]
        public void SelectFromJoinedTableWithAlias() {
            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title],
                    [AnotherTable].[Id] AS [Alias],
                    [AnotherTable].[Name]
                FROM [TestTable]
                INNER JOIN [AnotherTable] ON [AnotherTable].[Name] = [TestTable].[Title]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    sel => sel
                        .Field(t => t.Id)
                        .Field(t => t.Title)
                        .Field<AnotherTable>(t => t.Id, "Alias")
                        .Field<AnotherTable>(t => t.Name)
                )
                .Join<AnotherTable>((joined, main) => joined.Name == main.Title);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
        }

        [Test]
        public void JoinWithHint() {
            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                INNER JOIN [AnotherTable] WITH(NOLOCK) ON [AnotherTable].[Name] = [TestTable].[Title]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                )
                .Join<AnotherTable>(TableHint.NoLock, (joined, main) => joined.Name == main.Title);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
        }

        [Test]
        public void SelectAndJoinWithHints() {
            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable] WITH(NOLOCK)
                INNER JOIN [AnotherTable] WITH(NOLOCK) ON [AnotherTable].[Name] = [TestTable].[Title]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    TableHint.NoLock,
                    f => f.Id,
                    f => f.Title
                )
                .Join<AnotherTable>(TableHint.NoLock, (joined, main) => joined.Name == main.Title);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
        }

        [Test]
        public void JoinWithJoined() {
            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                INNER JOIN [AnotherTable] ON [AnotherTable].[Name] = [TestTable].[Title]
                INNER JOIN [ThirdTable] ON [ThirdTable].[Id] = [AnotherTable].[Id]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                )
                .Join<AnotherTable>((joined, main) => joined.Name == main.Title)
                .Join<ThirdTable, AnotherTable>((joined, main) => joined.Id == main.Id);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
        }

        [Test]
        public void JoinWithJoinedWithHint() {
            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                INNER JOIN [AnotherTable] WITH(NOLOCK) ON [AnotherTable].[Name] = [TestTable].[Title]
                INNER JOIN [ThirdTable] WITH(NOLOCK) ON [ThirdTable].[Id] = [AnotherTable].[Id]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                )
                .Join<AnotherTable>(TableHint.NoLock, (joined, main) => joined.Name == main.Title)
                .Join<ThirdTable, AnotherTable>(TableHint.NoLock, (joined, main) => joined.Id == main.Id);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
        }

        [Test]
        public void JoinWithJoinedWithHintAndJoinType() {
            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                INNER JOIN [AnotherTable] WITH(NOLOCK) ON [AnotherTable].[Name] = [TestTable].[Title]
                LEFT JOIN [ThirdTable] WITH(NOLOCK) ON [ThirdTable].[Id] = [AnotherTable].[Id]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                )
                .Join<AnotherTable>(TableHint.NoLock, (joined, main) => joined.Name == main.Title)
                .Join<ThirdTable, AnotherTable>(TableHint.NoLock, (joined, main) => joined.Id == main.Id, JoinType.Left);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
        }

        [Test]
        public void JoinWithKeywords() {
            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                INNER JOIN [ThirdTable] ON [ThirdTable].[Key] = [TestTable].[Title]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                )
                .Join<ThirdTable>((joined, main) => joined.Key == main.Title);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
        }

        [Test]
        public void JoinWithDerivedClass() {
            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id]
                FROM [TestTable]
                INNER JOIN [TestTableDerived] ON [TestTableDerived].[Id] = [TestTable].[Id]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id
                )
                .Join<TestTableDerived>((joined, main) => joined.Id == main.Id);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
        }

        private void TestJoinType(JoinType joinType) {
            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
                {ConvertJoinTypeToString(joinType)} JOIN [AnotherTable] ON [AnotherTable].[Name] = [TestTable].[Title]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                )
                .Join<AnotherTable>((joined, main) => joined.Name == main.Title, joinType);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
        }

        private string ConvertJoinTypeToString(JoinType joinType) {
            Regex regex = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            return regex.Replace(joinType.ToString(), " ").ToUpper();
        }

        [Test]
        public void JoinWithOnNullablePropertyInJoined() {

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    x => x.Sum(f => f.FloatVal)
                )
                .Join<AnotherTable>((joined, main) => joined.SomeNullableId == main.Id);

            TSqlQuery actualQuery = builder.CompileQuery();
        }

        [Test]
        public void JoinWithOnNullablePropertyInMain() {

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id
                )
                .Join<AnotherTable>((joined, main) => joined.Id == main.NullableId);

            TSqlQuery actualQuery = builder.CompileQuery();
        }

        [Test]
        public void JoinWithOnNullablePropertyInBoth() {

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id
                )
                .Join<AnotherTable>((joined, main) => joined.SomeNullableId == main.NullableId);

            TSqlQuery actualQuery = builder.CompileQuery();
        }

        [Test]
        public void SelectAllFieldsFromMainTableAndSomeFromJoined() {
            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title],
                    [TestTable].[FloatVal],
                    [TestTable].[DecimalVal],
                    [TestTable].[CreationDate],
                    [TestTable].[NullableId],
                    [AnotherTable].[Name],
                    [AnotherTable].[SomeNullableId]
                FROM [TestTable]
                INNER JOIN [AnotherTable] ON [AnotherTable].[Id] = [TestTable].[Id]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    sel => sel
                        .AllFields()
                        .Field<AnotherTable>(f => f.Name)
                        .Field<AnotherTable>(f => f.SomeNullableId)
                )
                .Join<AnotherTable>((joined, main) => joined.Id == main.Id);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
        }

        [Test]
        public void SelectSomeFieldsFromMainTableAndAllFromJoined() {
            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title],
                    [TestTable].[FloatVal],
                    [TestTable].[DecimalVal],
                    [TestTable].[CreationDate],
                    [TestTable].[NullableId],
                    [AnotherTable].[Name],
                    [AnotherTable].[SomeNullableId]
                FROM [AnotherTable]
                INNER JOIN [TestTable] ON [TestTable].[Id] = [AnotherTable].[Id]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<AnotherTable>(
                    sel => sel
                        .AllFields<TestTable>()
                        .Field<AnotherTable>(f => f.Name)
                        .Field<AnotherTable>(f => f.SomeNullableId)
                )
                .Join<TestTable>((joined, main) => joined.Id == main.Id);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
        }

        [Test]
        public void JoinWithSwappedMainAndJoinedTablesInExpression() {
            string expectedQuery = $@"
                SELECT 
                    [TestTable].[Id],
                    [AnotherTable].[Name]                    
                FROM [AnotherTable]
                INNER JOIN [TestTable] ON [AnotherTable].[Id] = [TestTable].[Id]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<AnotherTable>(
                    sel => sel
                        .Field<TestTable>(f => f.Id)
                        .Field<AnotherTable>(f => f.Name)
                )
                .Join<TestTable>((joined, main) => main.Id == joined.Id);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
        }
    }
}