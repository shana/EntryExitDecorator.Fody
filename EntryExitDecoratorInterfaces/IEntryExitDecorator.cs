using System;
using System.Reflection;

namespace EntryExitDecoratorInterfaces {
    public interface IEntryExitDecorator {
        void OnEntry();
        void OnExit();
    }
}