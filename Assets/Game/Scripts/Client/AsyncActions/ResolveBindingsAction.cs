using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ball.Core.AsyncSequence;
using Ball.Core.Binding;

namespace Ball.Client.AsyncActions
{
  public class ResolveBindingsAction : BaseAsyncAction
  {
    private IEnumerable<IResolvable> _resolvables;

    public ResolveBindingsAction(IEnumerable<IResolvable> resolvables = null) =>
      _resolvables = resolvables;

    public override Task<Result<AsyncActionResult>> InvokeAsync(AsyncActionSequence sequence, AsyncSequenceContext context, CancellationToken cancellationToken)
    {
      _resolvables ??= ServiceLocator.ResolveMany<IResolvable>();

      foreach (var resolvable in _resolvables)
      {
        resolvable.Resolve();
      }

      return TaskResultFromTypeAsync(AsyncActionResultType.Complete);
    }
  }
}