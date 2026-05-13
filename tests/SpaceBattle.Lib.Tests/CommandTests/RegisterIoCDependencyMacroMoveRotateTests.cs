using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class RegisterIoCDependencyMacroMoveRotateTests
{
    public RegisterIoCDependencyMacroMoveRotateTests()
    {
        IoC.Clear();
    }

    [Fact]
    public void Execute_WhenResolvingUnregisteredMacroMove_ThrowsInvalidOperationException()
    {
        IoC.Clear();
        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Macro.Move"));
    }

    [Fact]
    public void Execute_WhenResolvingUnregisteredMacroRotate_ThrowsInvalidOperationException()
    {
        IoC.Clear();
        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Macro.Rotate"));
    }

    [Fact]
    public void Execute_RegistersMacroMove_MacroCommandResolvesAndExecutesMoveCommand()
    {
        IoC.Clear();
        var mockCommand = new Mock<ICommand>();

        IoC.Register("Specs.Move", _ => new[] { "Move" });
        IoC.Register("Commands.Move", _ => mockCommand.Object);

        var registerCommand = new RegisterIoCDependencyMacroMoveRotate();
        registerCommand.Execute();

        var macroCommand = IoC.Resolve<ICommand>("Macro.Move");

        Assert.IsType<MacroCommand>(macroCommand);

        macroCommand.Execute();

        mockCommand.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void Execute_RegistersMacroRotate_MacroCommandResolvesAndExecutesRotateCommand()
    {
        IoC.Clear();
        var mockCommand = new Mock<ICommand>();

        IoC.Register("Specs.Rotate", _ => new[] { "Rotate" });
        IoC.Register("Commands.Rotate", _ => mockCommand.Object);

        var registerCommand = new RegisterIoCDependencyMacroMoveRotate();
        registerCommand.Execute();

        var macroCommand = IoC.Resolve<ICommand>("Macro.Rotate");

        Assert.IsType<MacroCommand>(macroCommand);

        macroCommand.Execute();

        mockCommand.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void Execute_WhenSpecsMoveNotRegistered_ThrowsInvalidOperationException()
    {
        IoC.Clear();
        IoC.Register("Commands.Move", _ => new Mock<ICommand>().Object);

        var registerCommand = new RegisterIoCDependencyMacroMoveRotate();
        registerCommand.Execute();

        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Macro.Move"));
    }

    [Fact]
    public void Execute_WhenSpecsRotateNotRegistered_ThrowsInvalidOperationException()
    {
        IoC.Clear();
        IoC.Register("Commands.Rotate", _ => new Mock<ICommand>().Object);

        var registerCommand = new RegisterIoCDependencyMacroMoveRotate();
        registerCommand.Execute();

        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Macro.Rotate"));
    }

    [Fact]
    public void Execute_WhenCommandMoveNotRegistered_ThrowsInvalidOperationException()
    {
        IoC.Clear();
        IoC.Register("Specs.Move", _ => new[] { "Move" });

        var registerCommand = new RegisterIoCDependencyMacroMoveRotate();
        registerCommand.Execute();

        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Macro.Move"));
    }

    [Fact]
    public void Execute_WhenCommandRotateNotRegistered_ThrowsInvalidOperationException()
    {
        IoC.Clear();
        IoC.Register("Specs.Rotate", _ => new[] { "Rotate" });

        var registerCommand = new RegisterIoCDependencyMacroMoveRotate();
        registerCommand.Execute();

        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Macro.Rotate"));
    }

    [Fact]
    public void Execute_RegistersMacroMoveWithMultipleCommands_MacroCommandExecutesAllCommands()
    {
        IoC.Clear();
        var mockMove = new Mock<ICommand>();
        var mockFuel = new Mock<ICommand>();

        IoC.Register("Specs.Move", _ => new[] { "Move", "CheckFuel" });
        IoC.Register("Commands.Move", _ => mockMove.Object);
        IoC.Register("Commands.CheckFuel", _ => mockFuel.Object);

        var registerCommand = new RegisterIoCDependencyMacroMoveRotate();
        registerCommand.Execute();

        var macroCommand = IoC.Resolve<ICommand>("Macro.Move");

        Assert.IsType<MacroCommand>(macroCommand);

        macroCommand.Execute();

        mockMove.Verify(c => c.Execute(), Times.Once);
        mockFuel.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void Execute_RegistersMacroRotateWithMultipleCommands_MacroCommandExecutesAllCommands()
    {
        IoC.Clear();
        var mockRotate = new Mock<ICommand>();
        var mockFuel = new Mock<ICommand>();

        IoC.Register("Specs.Rotate", _ => new[] { "Rotate", "CheckFuel" });
        IoC.Register("Commands.Rotate", _ => mockRotate.Object);
        IoC.Register("Commands.CheckFuel", _ => mockFuel.Object);

        var registerCommand = new RegisterIoCDependencyMacroMoveRotate();
        registerCommand.Execute();

        var macroCommand = IoC.Resolve<ICommand>("Macro.Rotate");

        Assert.IsType<MacroCommand>(macroCommand);

        macroCommand.Execute();

        mockRotate.Verify(c => c.Execute(), Times.Once);
        mockFuel.Verify(c => c.Execute(), Times.Once);
    }
}
