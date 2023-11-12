using LangChain.Base;
using LangChain.Docstore;

namespace LangChain.Callback;

/// <summary>
/// Callback manager for retriever run.
/// </summary>
public class CallbackManagerForRetrieverRun : ParentRunManager, IRunManagerImplementation<CallbackManagerForRetrieverRun>
{
    public CallbackManagerForRetrieverRun()
    {

    }

    public CallbackManagerForRetrieverRun(
        string runId,
        List<BaseCallbackHandler> handlers,
        List<BaseCallbackHandler> inheritableHandlers,
        string? parentRunId = null,
        List<string>? tags = null,
        List<string>? inheritableTags = null,
        Dictionary<string, object>? metadata = null,
        Dictionary<string, object>? inheritableMetadata = null)
        : base(runId, handlers, inheritableHandlers, parentRunId, tags, inheritableTags, metadata, inheritableMetadata)
    {
    }

    /// <summary>
    /// Run when retriever ends running.
    /// </summary>
    public async Task HandleRetrieverEndAsync(string query, IReadOnlyCollection<Document> docs)
    {
        foreach (var handler in Handlers)
        {
            if (!handler.IgnoreRetriever)
            {
                try
                {
                    await handler.HandleRetrieverEndAsync(query, docs.ToList(), RunId, ParentRunId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error in handler {handler.GetType().Name}, HandleRetrieverEnd: {ex}");
                }
            }
        }
    }

    /// <summary>
    /// Run when retriever errors.
    /// </summary>
    public async Task HandleRetrieverErrorAsync(Exception error, string query)
    {
        foreach (var handler in Handlers)
        {
            if (!handler.IgnoreRetriever)
            {
                try
                {
                    await handler.HandleRetrieverErrorAsync(error, query, RunId, ParentRunId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error in handler {handler.GetType().Name}, HandleRetrieverError: {ex}");
                }
            }
        }
    }
}