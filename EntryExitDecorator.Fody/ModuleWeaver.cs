using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EntryExitDecorator.Fody;
using MethodDecoratorInterfaces;
using Mono.Cecil;
using Mono.Cecil.Rocks;

public class ModuleWeaver {
    public ModuleDefinition ModuleDefinition { get; set; }
    public IAssemblyResolver AssemblyResolver { get; set; }
    public Action<string> LogInfo { get; set; }
    public Action<string> LogWarning { get; set; }
    public Action<string> LogError { get; set; }


    
    public void Execute() {
        this.LogInfo = s => { };
        this.LogWarning = s => { };

        var decorator = new EntryExitDecorator.Fody.MethodDecorator(this.ModuleDefinition);
        TypeDefinition idecorator = null;
        foreach (var x in this.ModuleDefinition.AssemblyReferences)
        {
            var def = AssemblyResolver.Resolve(x);
            if (def != null && def.Name.Name == "EntryExitDecoratorInterfaces")
                idecorator = def.MainModule.Types.First(t => t.Name == "IEntryExitDecorator");
        }

        if (idecorator == null)
            LogError("Could not find IEntryExitDecorator");
        this.DecorateDirectlyAttributed(decorator, idecorator);
    }


    private void DecorateDirectlyAttributed(EntryExitDecorator.Fody.MethodDecorator decorator, TypeDefinition idecorator) {

        var res = ModuleDefinition.Types.SelectMany(GetAllTypes).Select(t => GetTypes(idecorator, t)).Where(x => x != null).ToList();
        // if it's not the class the directly implements the interface and if it's the most derived class from it, grab it
        var markerTypeDefinitions = res.Where(t => (t.Item1 != t.Item2 && res.Any(p => p.Item1 == t.Item2) && res.All(p => p.Item1.BaseType != t.Item1))).ToList();

        if (!markerTypeDefinitions.Any())
        {
            if (LogError != null)
                LogError("Could not find any subclass of IEntryExitDecorator");
            //throw new WeavingException("Could not find any method decorator attribute");
        }

        foreach (var tuple in markerTypeDefinitions)
        {
            var methods = this.FindClassMethods(tuple.Item1);
            var entry = tuple.Item2.Methods.First(IsOnEntryMethod);
            var exit = tuple.Item2.Methods.First(IsOnExitMethod);
            foreach (var x in methods)
            {
                decorator.Decorate(x.TypeDefinition, x.MethodDefinition, entry, exit);
            }
        }
    }

    Tuple<TypeDefinition, TypeDefinition> GetTypes(TypeDefinition idecorator, TypeDefinition t)
    {
        Tuple<TypeDefinition, TypeDefinition> res = null;
        if (!t.IsInterface && t.HasInterfaces && t.Interfaces.Any(c => c.Resolve().FullName == idecorator.FullName))
            res = Tuple.Create(t, t);
        else if (t.BaseType != null && t.BaseType.FullName != "System.Object")
            res = GetTypes(idecorator, t.BaseType.Resolve());

        if (res != null && res.Item1 != t)
        {
            res = Tuple.Create(t, HasCorrectMethods(t) ? t : res.Item2);
        }
        return res;
    }

    private static bool HasCorrectMethods(TypeDefinition type) {
        return type.Methods.Any(IsOnEntryMethod) &&
               type.Methods.Any(IsOnExitMethod);
    }

    private static bool IsOnEntryMethod(MethodDefinition m) {
        return m.Name == "OnEntry" &&
               m.Parameters.Count == 0;
    }

    private static bool IsOnExitMethod(MethodDefinition m) {
        return m.Name == "OnExit" &&
               m.Parameters.Count == 0;
    }

    IEnumerable<AttributeMethodInfo> FindClassMethods(IEnumerable<TypeDefinition> markerTypeDefintions)
    {
        return from topLevelType in this.ModuleDefinition.Types
               from type in GetAllTypes(topLevelType)
               from method in type.Methods
               where method.HasBody
                    && !IsOnEntryMethod(method)
                    && !IsOnExitMethod(method)
                    && !method.IsGetter
                    && !method.IsSetter
                    && !method.IsAbstract
                    && !method.IsVirtual
                    && !method.IsConstructor
                    && !method.IsStatic
                    && !method.IsPrivate
               select new AttributeMethodInfo
               {
                   CustomAttribute = null,
                   TypeDefinition = type,
                   MethodDefinition = method
               };
    }

    IEnumerable<AttributeMethodInfo> FindClassMethods(TypeDefinition type)
    {
        return from method in type.Methods
               where method.HasBody
                    && !IsOnEntryMethod(method)
                    && !IsOnExitMethod(method)
                    && !method.IsGetter
                    && !method.IsSetter
                    && !method.IsAbstract
                    && !method.IsVirtual
                    && !method.IsConstructor
                    && !method.IsStatic
               select new AttributeMethodInfo
               {
                   CustomAttribute = null,
                   TypeDefinition = type,
                   MethodDefinition = method
               };
    }

    private bool AreEquals(TypeDefinition attributeTypeDef, TypeDefinition markerTypeDefinition) {
        return attributeTypeDef.FullName == markerTypeDefinition.FullName;
    }

    private static IEnumerable<TypeDefinition> GetAllTypes(TypeDefinition type) {
        yield return type;

        var allNestedTypes = from t in type.NestedTypes
                             from t2 in GetAllTypes(t)
                             select t2;

        foreach (var t in allNestedTypes)
            yield return t;
    }

    private class HostAttributeMapping {
        public TypeDefinition[] AttribyteTypes { get; set; }
        public CustomAttribute HostAttribute { get; set; }
    }

    private class AttributeMethodInfo {
        public TypeDefinition TypeDefinition { get; set; }
        public MethodDefinition MethodDefinition { get; set; }
        public CustomAttribute CustomAttribute { get; set; }
    }
}
