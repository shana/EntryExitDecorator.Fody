using System;
using System.Reflection;
using EntryExitDecorator.Fody.Tests;
using EntryExitDecorator.Fody.Tests.Helpers;
using Xunit;

public class StaticMethodShouldNotCallTest : SimpleTestBase
{

    [Fact]
    public void ShouldNotReport()
    {
        var klass = Assembly.GetStaticInstance("SimpleTest.StaticMethodShouldNotCallTest");
        klass.Test();
        this.CheckMethodNotCalledSeq(new[] { Method.OnEnter, Method.OnExit });
    }
}
