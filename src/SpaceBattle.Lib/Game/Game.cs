using SpaceBattle.Lib.Abstractions;

namespace SpaceBattle.Lib.Game;

public class GameLoop
{
    private readonly Queue<ICommand> _queue = new();

    public void AddCommand(ICommand command)
    {
        _queue.Enqueue(command);
    }

    public void Run()
    {
        while (_queue.Count > 0)
        {
            var command = _queue.Dequeue();
            try
            {
                command.Execute();
            }
            catch
            {
                // Continue executing remaining commands
            }
        }
    }
}
