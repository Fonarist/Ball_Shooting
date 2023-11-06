using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ball.Core.AsyncSequence
{
  public abstract class BaseAsyncAction : IAsyncAction
  {
    public abstract Task<Result<AsyncActionResult>> InvokeAsync(
      AsyncActionSequence sequence,
      AsyncSequenceContext context,
      CancellationToken cancellationToken);

    protected Result<AsyncActionResult> ResultFromType(AsyncActionResultType resultType, AsyncActionSequence sequence = null)
    {
      return Result<AsyncActionResult>.FromValue(new AsyncActionResult(resultType, sequence));
    }

    protected Result<AsyncActionResult> ResultFromCancellation()
    {
      return Result<AsyncActionResult>.FromError(nameof(OperationCanceledException));
    }

    protected Task<Result<AsyncActionResult>> TaskResultFromTypeAsync(AsyncActionResultType resultType, AsyncActionSequence sequence = null)
    {
      return Task.FromResult(ResultFromType(resultType, sequence));
    }
  }
}