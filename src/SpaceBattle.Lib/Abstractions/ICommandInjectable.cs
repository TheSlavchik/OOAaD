namespace SpaceBattle.Lib.Abstractions;

public interface ICommandInjectable
{
    void Inject(ICommand command);
}
