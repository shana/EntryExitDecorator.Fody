using System;
using System.Reflection;
using EntryExitDecorator.Fody.Tests;
using Xunit;

public class FinallyBlockTest : ClassTestsBase
{

    public FinallyBlockTest() : base("SimpleTest.FinallyBlockTest") { }

    [Fact]
    public void ShouldReportOnEntryAndExit()
    {
        this.TestClass.Test();
        this.CheckMethodSeq(new[] { Method.OnEnter, Method.OnExit });
    }
}
