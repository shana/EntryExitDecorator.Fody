using System;
using System.Diagnostics;
using EntryExitDecoratorInterfaces;

namespace SimpleTest
{
    public class StaticMethodShouldNotCallTest : BaseClassEntryExit
    {
        public static void Test()
        {
        }
    }
}
