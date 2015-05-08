using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using MethodDecorator.Fody;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace EntryExitDecorator.Fody {
    public class MethodDecorator
    {
        private readonly ReferenceFinder _referenceFinder;

        public MethodDecorator(ModuleDefinition moduleDefinition)
        {
            this._referenceFinder = new ReferenceFinder(moduleDefinition);
        }

        public void Decorate(TypeDefinition type, MethodDefinition method, MethodReference onEntryMethodRef, MethodReference onExitMethodRef)
        {
            method.Body.InitLocals = true;

            var processor = method.Body.GetILProcessor();
            var methodBodyFirstInstruction = method.Body.Instructions.First();
            
            //if (method.Body.Instructions.Where(i => i.OpCode == OpCodes.Call).Count() == 0)
            //{
            //  throw new ApplicationException(string.Format("Method '{0}': method.Body.Instructions.Where(i => i.OpCode == OpCodes.Call).Count() == 0", method.FullName));
            //}
            //if (method.IsConstructor) methodBodyFirstInstruction = method.Body.Instructions.First(i => i.OpCode == OpCodes.Call).Next;

            if (method.IsConstructor && method.Body.Instructions.Where(i => i.OpCode == OpCodes.Call).Count() > 0)
            {
                methodBodyFirstInstruction = method.Body.Instructions.First(i => i.OpCode == OpCodes.Call).Next;
            }

            var callOnEntryInstructions = GetCallOnEntryInstructions(processor, onEntryMethodRef);
            var callOnExitInstructions = GetCallOnExitInstructions(processor, onExitMethodRef);

            processor.InsertBefore(methodBodyFirstInstruction, callOnEntryInstructions);
            processor.InsertBefore(method.Body.Instructions.Last(), callOnExitInstructions);

        }

        private static IEnumerable<Instruction> GetCallOnEntryInstructions(
            ILProcessor processor,
            MethodReference onEntryMethodRef)
        {
            // Call OnEntry()
            return new List<Instruction>
                {
                    processor.Create(OpCodes.Ldarg_0),
                    processor.Create(OpCodes.Call, onEntryMethodRef),
                };
        }

        private static IList<Instruction> GetCallOnExitInstructions(ILProcessor processor, MethodReference onExitMethodRef)
        {
            // Call OnExit()
            return new List<Instruction>
                {
                    processor.Create(OpCodes.Ldarg_0),
                    processor.Create(OpCodes.Call, onExitMethodRef),
                };

        }
    }
}


