using System;

namespace TSqlQueryBuilder.Tests {
    public enum TestEnum {
        One = 0,
        Two = 1,
        Three = 2
    }

    public class TestTable {
        public int Id { get; set; }
        public string Title { get; set; }
        public float FloatVal { get; set; }
        public decimal DecimalVal { get; set; }
        public DateTime CreationDate { get; set; }
        public int? NullableId { get; set; }
    }

    public class AnotherTable {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? SomeNullableId { get; set; }
    }

    public class ThirdTable {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Key { get; set; }
        public int Index { get; set; }
    }

    public class TestTableDerived: TestTable {
        public string SomeNewProperty { get; set; }
    }

    public class TableWithEnum {
        public int Id { get; set; }
        public string Name { get; set; }
        public TestEnum Enum { get; set; }
    }
}