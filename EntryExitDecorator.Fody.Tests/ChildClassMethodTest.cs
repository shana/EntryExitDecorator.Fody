using System;
using System.Reflection;
using EntryExitDecorator.Fody.Tests;
using Xunit;

public class ChildClassMethodTest : ClassTestsBase
{

    public ChildClassMethodTest() : base("SimpleTest.ChildClassTest") { }

    [Fact]
    public void ShouldReportOnEntryAndExit()
    {
        this.TestClass.Test();
        this.CheckMethodSeq(new[] { Method.OnEnter, Method.OnExit });
    }
}
