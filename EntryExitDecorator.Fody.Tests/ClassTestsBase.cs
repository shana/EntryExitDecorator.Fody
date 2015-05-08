
using EntryExitDecorator.Fody.Tests.Helpers;

namespace EntryExitDecorator.Fody.Tests {
    public class ClassTestsBase : SimpleTestBase {

        protected ClassTestsBase() { }

        protected ClassTestsBase(string className) {
            ClassName = className;
        }
        
        protected string ClassName { get; set; }

        protected dynamic TestClass {
            get { return this.Assembly.GetInstance(ClassName); }
        }
    }
}