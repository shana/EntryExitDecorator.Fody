using System;
using System.Reflection;
using EntryExitDecorator.Fody.Tests;
using Xunit;

public class NestedTypesTest : ClassTestsBase
{
    [Fact]
    public void ShouldReportOnEntryAndExit()
    {
        ClassName = "SimpleTest.NestedTypesTest";
        TestClass.Test();
        CheckMethodNotCalledSeq(new[] { Method.OnEnter, Method.OnExit });

        ClassName = "SimpleTest.NestedTypesTest+NestedTypesTestClass";
        TestClass.Test();
        CheckMethodSeq(new[] { Method.OnEnter, Method.OnExit });
    }
}
