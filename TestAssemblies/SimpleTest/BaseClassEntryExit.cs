using System;
using MethodDecoratorInterfaces;

namespace SimpleTest
{
    public class BaseClassEntryExit : IEntryExitDecorator
    {
        public void OnEntry()
        {
            TestRecords.RecordOnEntry();
        }

        public void OnExit()
        {
            TestRecords.RecordOnExit();
        }
    }
}