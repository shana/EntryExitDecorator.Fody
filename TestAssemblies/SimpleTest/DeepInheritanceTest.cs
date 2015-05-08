using System;
using System.Diagnostics;
using EntryExitDecoratorInterfaces;

namespace SimpleTest
{
    public class DeepInheritanceChildClass1 : BaseClassEntryExit
    {
        public void ShouldNotCallEntryExit1()
        {
        }
    }

    public class DeepInheritanceChildClass2 : DeepInheritanceChildClass1
    {
        public void ShouldNotCallEntryExit2()
        {
        }
    }

    public class DeepInheritanceChildClass : DeepInheritanceChildClass2
    {
        public void Test()
        {
        }
    }
}


