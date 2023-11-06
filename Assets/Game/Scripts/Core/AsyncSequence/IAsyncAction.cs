using System.Threading;
using System.Threading.Tasks;

namespace Ball.Core.AsyncSequence
{
    public interface IAsyncAction
    {
        Task<Result<AsyncActionResult>> InvokeAsync(AsyncActionSequence sequence, AsyncSequenceContext context, CancellationToken cancellationToken);
    }
}