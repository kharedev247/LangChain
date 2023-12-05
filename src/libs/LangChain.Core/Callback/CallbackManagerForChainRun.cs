using LangChain.Abstractions.Schema;
using LangChain.Base;

namespace LangChain.Callback;

public class CallbackManagerForChainRun : ParentRunManager, IRunManagerImplementation<CallbackManagerForChainRun>
{
    public CallbackManagerForChainRun()
    {

    }

    public CallbackManagerForChainRun(
        string runId,
        List<BaseCallbackHandler> handlers,
        List<BaseCallbackHandler> inheritableHandlers,
        string? parentRunId = null)
        : base(runId, handlers, inheritableHandlers, parentRunId)
    {
    }

    public async Task HandleChainEndAsync(IChainValues input, IChainValues output)
    {
        foreach (var handler in Handlers)
        {
            if (!handler.IgnoreChain)
            {
                try
                {
                    await handler.HandleChainEndAsync(
                        input.Value,
                        output.Value,
                        RunId,
                        ParentRunId).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error in handler {handler.GetType().Name}, HandleChainEnd: {ex}");
                }
            }
        }
    }

    public async Task HandleChainErrorAsync(Exception error, IChainValues input)
    {
        foreach (var handler in Handlers)
        {
            if (!handler.IgnoreLlm)
            {
                try
                {
                    await handler.HandleChainErrorAsync(error, RunId, input.Value, ParentRunId).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error in handler {handler.GetType().Name}, HandleChainError: {ex}");
                }
            }
        }
    }

    public async Task HandleTextAsync(string text)
    {
        foreach (var handler in Handlers)
        {
            if (!handler.IgnoreLlm)
            {
                try
                {
                    await handler.HandleTextAsync(text, RunId, ParentRunId).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error in handler {handler.GetType().Name}, HandleText: {ex}");
                }
            }
        }
    }
}