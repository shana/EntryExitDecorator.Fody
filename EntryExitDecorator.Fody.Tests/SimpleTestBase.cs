using System.Reflection;
using EntryExitDecorator.Fody.Tests.Helpers;

namespace EntryExitDecorator.Fody.Tests {
    public class SimpleTestBase : TestsBase
    {
        private static readonly Assembly _assembly = CreateAssembly();

        public SimpleTestBase() {
            _assembly.GetStaticInstance("SimpleTest.TestRecords").Clear();
        }

        protected override Assembly Assembly {
            get { return _assembly; }
        }

        protected override dynamic RecordHost {
            get { return this.Assembly.GetStaticInstance("SimpleTest.TestRecords"); }
        }

        private static Assembly CreateAssembly() {
            var weaverHelper = new WeaverHelper(@"SimpleTest\SimpleTest.csproj");
            return weaverHelper.Weave();
        }
    }
}