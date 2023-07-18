using MyTelegramBot.Utils;

namespace MyTelegramBot.Types;

public interface IListener
{
    public CommandParser ArgumentParser { get; set; }
    /// <summary>Checks if the <c>Update</c> matches the listener condition.</summary>
    public abstract Task<bool> Validate(Context context, CancellationToken cancellationToken);
    /// <summary>Handles the <c>Update</c> if it is successfully validated.</summary>
    public abstract Task Handler(Context context, CancellationToken cancellationToken);

    public abstract Task Handler(Context context, Dictionary<string, string> buttonsList,
        CancellationToken cancellationToken);

    /// <summary>Processes a command synchronously.</summary>
    /// <returns>Command result string.</returns>
    protected abstract string Run(Context context, CancellationToken cancellationToken);

    /// <summary>Processes a command asynchronously.</summary>
    /// <returns>Command result string.</returns>
    protected abstract Task<string> RunAsync(Context context, CancellationToken cancellationToken);
}