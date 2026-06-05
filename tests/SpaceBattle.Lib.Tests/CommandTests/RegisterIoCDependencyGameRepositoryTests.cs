using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.CommandTests;

[Collection("IoC")]
public class RegisterIoCDependencyGameRepositoryTests
{
    public RegisterIoCDependencyGameRepositoryTests()
    {
        IoC.Clear();
    }

    [Fact]
    public void Execute_WhenResolvingUnregisteredDependency_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() =>
            IoC.Resolve<IGameObjectRepository>("Game.Repository"));
    }

    [Fact]
    public void Execute_AfterRegistration_ResolvesIGameObjectRepository()
    {
        var registerCommand = new RegisterIoCDependencyGameRepository();
        registerCommand.Execute();

        var repository = IoC.Resolve<IGameObjectRepository>("Game.Repository");

        Assert.NotNull(repository);
        Assert.IsAssignableFrom<IGameObjectRepository>(repository);
    }

    [Fact]
    public void Execute_MultipleResolves_ReturnsSameSingletonInstance()
    {
        var registerCommand = new RegisterIoCDependencyGameRepository();
        registerCommand.Execute();

        var first = IoC.Resolve<IGameObjectRepository>("Game.Repository");
        var second = IoC.Resolve<IGameObjectRepository>("Game.Repository");

        Assert.Same(first, second);
    }

    [Fact]
    public void Execute_SingletonInstance_StoresAndRetrievesData()
    {
        var registerCommand = new RegisterIoCDependencyGameRepository();
        registerCommand.Execute();

        var repository = IoC.Resolve<IGameObjectRepository>("Game.Repository");
        var id = Guid.NewGuid();
        var obj = new Dictionary<string, object> { { "x", 10 } };

        repository.Add(id, obj);

        var retrieved = repository.GetById(id);
        Assert.Same(obj, retrieved);
    }
}
