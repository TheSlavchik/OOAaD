using System.Reflection;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Commands;

public class RegisterIoCDependencyDependencyCommand : ICommand
{
    private readonly Assembly _assembly;

    public RegisterIoCDependencyDependencyCommand(Assembly assembly)
    {
        _assembly = assembly;
    }

    public void Execute()
    {
        RegisterAdapterDependencies();
        RegisterCommandDependencies();
    }

    private void RegisterCommandDependencies()
    {
        var commandTypes = _assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(ICommand).IsAssignableFrom(t))
            .ToList();

        foreach (var commandType in commandTypes)
        {
            var ctors = commandType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (ctors.Length == 0) continue;

            var ctor = ctors[0];
            var parameters = ctor.GetParameters();

            if (parameters.Length != 1) continue;

            var paramType = parameters[0].ParameterType;
            if (!paramType.IsInterface) continue;

            var commandName = GetCommandName(commandType);
            if (commandName is null) continue;

            var adapterKey = $"Adapters.{paramType.Name}";

            try
            {
                IoC.Register(commandName, args =>
                {
                    var obj = (IDictionary<string, object>)args[0];
                    var adapter = IoC.Resolve<object>(adapterKey, obj);
                    return Activator.CreateInstance(commandType, adapter)!;
                });
            }
            catch (ArgumentException)
            {
                // Already registered, skip
            }
        }
    }

    private void RegisterAdapterDependencies()
    {
        var interfaceTypes = _assembly.GetTypes()
            .Where(t => t.IsInterface
                && t.GetProperties(BindingFlags.Public | BindingFlags.Instance).Length > 0)
            .ToList();

        foreach (var interfaceType in interfaceTypes)
        {
            var adapterKey = $"Adapters.{interfaceType.Name}";

            try
            {
                IoC.Register(adapterKey, args =>
                {
                    var obj = (IDictionary<string, object>)args[0];
                    return DynamicAdapterFactory.Create(interfaceType, obj);
                });
            }
            catch (ArgumentException)
            {
                // Already registered, skip
            }
        }
    }

    private static string? GetCommandName(Type commandType)
    {
        var name = commandType.Name;

        if (name.EndsWith("Command") && name.Length > 7)
        {
            name = name[..^7];
        }

        return $"Команды.{name}";
    }
}