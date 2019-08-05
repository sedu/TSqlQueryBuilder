using NUnit.Framework;

namespace TSqlQueryBuilder.Tests {
    [TestFixture]
    public class SelectTests: BaseTest {
        [Test]
        public void SomeFields() {
            string expectedQuery = @"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
            ";
            
            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    f => f.Id,
                    f => f.Title
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void AllFields() {
            string expectedQuery = @"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title],
                    [TestTable].[FloatVal],
                    [TestTable].[DecimalVal],
                    [TestTable].[CreationDate],
                    [TestTable].[NullableId]
                FROM [TestTable]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder.Select<TestTable>();

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void SomeFieldsWithHints() {
            string expectedQuery = @"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable] WITH (NOLOCK)
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    TableHint.NoLock,
                    f => f.Id,
                    f => f.Title
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void AllFieldsWithHints() {
            string expectedQuery = @"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title],
                    [TestTable].[FloatVal],
                    [TestTable].[DecimalVal],
                    [TestTable].[CreationDate],
                    [TestTable].[NullableId]
                FROM [TestTable] WITH (NOLOCK)
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder.Select<TestTable>(TableHint.NoLock);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void SelecctFieldsWithAliases() {
            string expectedQuery = @"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title] AS [CustomName]
                FROM [TestTable]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(
                    sel => sel
                        .Field(f => f.Id)
                        .Field(f => f.Title, "CustomName")
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void SelectKeywords() {
            string expectedQuery = @"
                SELECT 
                    [ThirdTable].[Key],
                    [ThirdTable].[Index]
                FROM [ThirdTable]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<ThirdTable>(
                    f => f.Key,
                    f => f.Index
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void SelectFromDerivedClass() {
            string expectedQuery = @"
                SELECT 
                    [TestTableDerived].[Id],
                    [TestTableDerived].[Title]
                FROM [TestTableDerived]
            ";

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTableDerived>(
                    f => f.Id,
                    f => f.Title
                );

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void SelectWithDeclarationAsParameter() {
            string expectedQuery = @"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title]
                FROM [TestTable]
            ";

            SelectDeclaration<TestTable> selectDeclaration = new SelectDeclaration<TestTable>();
            selectDeclaration.Field(f => f.Id);
            selectDeclaration.Field(f => f.Title);

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(selectDeclaration);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }

        [Test]
        public void SelectAllFieldsWithDeclarationAsParameter() {
            string expectedQuery = @"
                SELECT 
                    [TestTable].[Id],
                    [TestTable].[Title],
                    [TestTable].[FloatVal],
                    [TestTable].[DecimalVal],
                    [TestTable].[CreationDate],
                    [TestTable].[NullableId]
                FROM [TestTable]
            ";

            SelectDeclaration<TestTable> selectDeclaration = new SelectDeclaration<TestTable>();

            selectDeclaration.AllFields();

            TSqlBuilder builder = new TSqlBuilder();
            builder
                .Select<TestTable>(selectDeclaration);

            TSqlQuery actualQuery = builder.CompileQuery();

            Assert.AreEqual(NormalizeSqlQuery(expectedQuery), NormalizeSqlQuery(actualQuery.Query));
            CollectionAssert.IsEmpty(actualQuery.Parameters);
        }
    }
}