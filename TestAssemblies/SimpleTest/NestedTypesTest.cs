using System;
using System.Diagnostics;
using EntryExitDecoratorInterfaces;

namespace SimpleTest
{
    public class NestedTypesTest
    {
        public void Test()
        {
        }

        public class NestedTypesTestClass : BaseClassEntryExit
        {
            public void Test()
            {
            }
        }

    }
}


