using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntryExitDecorator.Fody.Tests.Helpers;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = new SimpleTest.NestedTypesTest.NestedTypesTestClass();
            var weaverHelper = new WeaverHelper(@"SimpleTest\SimpleTest.csproj");
            weaverHelper.Weave();
            
        }
    }
}
