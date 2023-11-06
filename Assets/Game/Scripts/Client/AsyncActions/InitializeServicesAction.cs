using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ball.Core.AsyncSequence;
using Ball.Core.Binding;

namespace Ball.Client.AsyncActions
{
  public class InitializeServicesAction : BaseAsyncAction
  {
    private IEnumerable<IInitializable> _initializables;

    public InitializeServicesAction(IEnumerable<IInitializable> initializables = null) =>
      _initializables = initializables;

    public override async Task<Result<AsyncActionResult>> InvokeAsync(
      AsyncActionSequence sequence,
      AsyncSequenceContext context,
      CancellationToken cancellationToken)
    {
      _initializables ??= ServiceLocator.ResolveMany<IInitializable>();
      foreach (var resolvable in _initializables)
      {
        await resolvable.InitializeAsync(cancellationToken).ConfigureAwait(true);
        if (cancellationToken.IsCancellationRequested)
        {
          return ResultFromCancellation();
        }
      }

      return ResultFromType(AsyncActionResultType.Complete);
    }
  }
}