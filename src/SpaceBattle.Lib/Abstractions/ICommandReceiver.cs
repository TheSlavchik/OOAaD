namespace SpaceBattle.Lib.Abstractions;

public interface ICommandReceiver
{
    public void Receive(ICommand command);
}
