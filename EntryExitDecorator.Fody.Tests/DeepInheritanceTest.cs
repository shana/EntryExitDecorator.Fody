using EntryExitDecorator.Fody.Tests;
using Xunit;

public class DeepInheritanceTest : ClassTestsBase
{
    [Fact]
    public void ShouldReportOnEntryAndExit()
    {
        ClassName = "SimpleTest.DeepInheritanceChildClass1";
        TestClass.ShouldNotCallEntryExit1();
        CheckMethodNotCalledSeq(new[] { Method.OnEnter, Method.OnExit });

        ClassName = "SimpleTest.DeepInheritanceChildClass2";
        TestClass.ShouldNotCallEntryExit2();
        CheckMethodNotCalledSeq(new[] { Method.OnEnter, Method.OnExit });

        ClassName = "SimpleTest.DeepInheritanceChildClass";
        TestClass.Test();
        CheckMethodSeq(new[] { Method.OnEnter, Method.OnExit });
    }
}
