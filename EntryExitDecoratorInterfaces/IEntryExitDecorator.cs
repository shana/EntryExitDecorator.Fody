using System;
using System.Reflection;

namespace MethodDecoratorInterfaces {
    public interface IEntryExitDecorator {
        void OnEntry();
        void OnExit();
    }
}