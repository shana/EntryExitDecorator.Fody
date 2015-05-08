using System;
using EntryExitDecoratorInterfaces;

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