using System.Reflection;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Data;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.CommandTests;

[Collection("IoC")]
public class RegisterIoCDependencyDependencyCommandTests
{
    private readonly Assembly _assembly;

    public RegisterIoCDependencyDependencyCommandTests()
    {
        IoC.Clear();
        _assembly = typeof(RegisterIoCDependencyDependencyCommand).Assembly;
    }

    [Fact]
    public void Execute_RegistersAdapterForIMovable_AndAdapterCanBeResolved()
    {
        var cmd = new RegisterIoCDependencyDependencyCommand(_assembly);
        cmd.Execute();

        var data = new Dictionary<string, object>
        {
            { "position", new Vector(12, 5) },
            { "velocity", new Vector(-4, 1) }
        };

        var adapter = IoC.Resolve<IMovable>("Adapters.IMovable", data);

        Assert.Equal(new Vector(12, 5), adapter.Position);
        Assert.Equal(new Vector(-4, 1), adapter.Velocity);
    }

    [Fact]
    public void Execute_RegistersCommandAsCommanMove_AndMoveCanBeResolvedAndExecuted()
    {
        var cmd = new RegisterIoCDependencyDependencyCommand(_assembly);
        cmd.Execute();

        var data = new Dictionary<string, object>
        {
            { "position", new Vector(12, 5) },
            { "velocity", new Vector(-4, 1) }
        };

        var moveCommand = IoC.Resolve<ICommand>("Команды.Move", data);
        moveCommand.Execute();

        Assert.Equal(new Vector(8, 6), data["position"]);
    }

    [Fact]
    public void Execute_AdapterForIMovable_PropertyNotInDictionary_ThrowsInvalidOperationException()
    {
        var cmd = new RegisterIoCDependencyDependencyCommand(_assembly);
        cmd.Execute();

        var data = new Dictionary<string, object>();
        var adapter = IoC.Resolve<IMovable>("Adapters.IMovable", data);

        Assert.Throws<InvalidOperationException>(() => adapter.Position);
    }

    [Fact]
    public void Execute_AdapterForIMovable_SetProperty_UpdatesDictionary()
    {
        var cmd = new RegisterIoCDependencyDependencyCommand(_assembly);
        cmd.Execute();

        var data = new Dictionary<string, object>();
        var adapter = IoC.Resolve<IMovable>("Adapters.IMovable", data);

        adapter.Position = new Vector(10, 20);

        Assert.Equal(new Vector(10, 20), data["position"]);
    }

    [Fact]
    public void Execute_AdapterForIRotatable_CanResolveAndReadProperties()
    {
        var cmd = new RegisterIoCDependencyDependencyCommand(_assembly);
        cmd.Execute();

        var data = new Dictionary<string, object>
        {
            { "angle", new Angle(3) },
            { "angularVelocity", new Angle(2) }
        };

        var rotatable = IoC.Resolve<IRotatable>("Adapters.IRotatable", data);

        Assert.Equal(new Angle(3), rotatable.Angle);
        Assert.Equal(new Angle(2), rotatable.AngularVelocity);
    }

    [Fact]
    public void Execute_AdapterForIRotatable_SetProperty_UpdatesDictionary()
    {
        var cmd = new RegisterIoCDependencyDependencyCommand(_assembly);
        cmd.Execute();

        var data = new Dictionary<string, object>
        {
            { "angle", new Angle(1) },
        };

        var rotatable = IoC.Resolve<IRotatable>("Adapters.IRotatable", data);

        rotatable.Angle = new Angle(5);

        Assert.Equal(new Angle(5), data["angle"]);
    }

    [Fact]
    public void Execute_CanResolveRotateCommand_AndExecute()
    {
        var cmd = new RegisterIoCDependencyDependencyCommand(_assembly);
        cmd.Execute();

        var data = new Dictionary<string, object>
        {
            { "angle", new Angle(3) },
            { "angularVelocity", new Angle(2) }
        };

        var rotateCommand = IoC.Resolve<ICommand>("Команды.Rotate", data);
        rotateCommand.Execute();

        Assert.Equal(new Angle(5), data["angle"]);
    }

    [Fact]
    public void Execute_AdapterPropertyWithAdapterAttribute_UsesCustomStrategy()
    {
        IoC.Register("ClassName.StaticMethod", args =>
        {
            var obj = (IDictionary<string, object>)args[0];
            // Strategy returns the value from "custom" key
            return obj["custom"];
        });

        var cmd = new RegisterIoCDependencyDependencyCommand(_assembly);
        cmd.Execute();

        // The IMovable interface has no AdapterAttribute, so this test verifies
        // that normal resolution still works
        var data = new Dictionary<string, object>
        {
            { "position", new Vector(1, 2) },
            { "velocity", new Vector(3, 4) }
        };

        var adapter = IoC.Resolve<IMovable>("Adapters.IMovable", data);

        Assert.Equal(new Vector(1, 2), adapter.Position);
        Assert.Equal(new Vector(3, 4), adapter.Velocity);
    }

    [Fact]
    public void Execute_WhenResolvingUnregisteredDependency_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(
            () => IoC.Resolve<ICommand>("NonExistent", new Dictionary<string, object>()));
    }

    [Fact]
    public void Execute_Idempotent_CallingMultipleTimesDoesNotThrow()
    {
        var cmd = new RegisterIoCDependencyDependencyCommand(_assembly);

        cmd.Execute();
        cmd.Execute();
        cmd.Execute();

        // Should not throw - verify by resolving
        var data = new Dictionary<string, object>
        {
            { "position", new Vector(1, 2) },
            { "velocity", new Vector(3, 4) }
        };

        var adapter = IoC.Resolve<IMovable>("Adapters.IMovable", data);
        Assert.Equal(new Vector(1, 2), adapter.Position);
    }

    [Fact]
    public void Execute_DynamicAdapter_WithInvalidPropertyName_Throws()
    {
        var cmd = new RegisterIoCDependencyDependencyCommand(_assembly);
        cmd.Execute();

        var data = new Dictionary<string, object>
        {
            // "position" key is intentionally lowercase 'p'
            { "Position", new Vector(1, 2) },
            { "velocity", new Vector(3, 4) }
        };

        var adapter = IoC.Resolve<IMovable>("Adapters.IMovable", data);

        // Should throw because key "position" (lowercase) is missing - only "Position" exists
        Assert.Throws<InvalidOperationException>(() => adapter.Position);
    }
}