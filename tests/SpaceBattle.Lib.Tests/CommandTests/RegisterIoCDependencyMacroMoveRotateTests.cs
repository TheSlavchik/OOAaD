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
        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Macro.Move"));
    }

    [Fact]
    public void Execute_WhenResolvingUnregisteredMacroRotate_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Macro.Rotate"));
    }

    [Fact]
    public void Execute_RegistersMacroMove_MacroCommandResolvesAndExecutesMoveCommand()
    {
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
        IoC.Register("Commands.Move", _ => new Mock<ICommand>().Object);

        var registerCommand = new RegisterIoCDependencyMacroMoveRotate();
        registerCommand.Execute();

        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Macro.Move"));
    }

    [Fact]
    public void Execute_WhenSpecsRotateNotRegistered_ThrowsInvalidOperationException()
    {
        IoC.Register("Commands.Rotate", _ => new Mock<ICommand>().Object);

        var registerCommand = new RegisterIoCDependencyMacroMoveRotate();
        registerCommand.Execute();

        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Macro.Rotate"));
    }

    [Fact]
    public void Execute_WhenCommandMoveNotRegistered_ThrowsInvalidOperationException()
    {
        IoC.Register("Specs.Move", _ => new[] { "Move" });

        var registerCommand = new RegisterIoCDependencyMacroMoveRotate();
        registerCommand.Execute();

        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Macro.Move"));
    }

    [Fact]
    public void Execute_WhenCommandRotateNotRegistered_ThrowsInvalidOperationException()
    {
        IoC.Register("Specs.Rotate", _ => new[] { "Rotate" });

        var registerCommand = new RegisterIoCDependencyMacroMoveRotate();
        registerCommand.Execute();

        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Macro.Rotate"));
    }

    [Fact]
    public void Execute_RegistersMacroMoveWithMultipleCommands_MacroCommandExecutesAllCommands()
    {
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
