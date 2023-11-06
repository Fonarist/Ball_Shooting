using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ball.Core.AsyncSequence;
using Ball.Core.Binding;

namespace Ball.Client.AsyncActions
{
  public class RunServicesAction : BaseAsyncAction
  {
    private IEnumerable<IRunnable> _initializables;

    public RunServicesAction(IEnumerable<IRunnable> initializables = null) =>
      _initializables = initializables;

    public override Task<Result<AsyncActionResult>> InvokeAsync(
      AsyncActionSequence sequence,
      AsyncSequenceContext context,
      CancellationToken cancellationToken)
    {
      _initializables ??= ServiceLocator.ResolveMany<IRunnable>();
      foreach (var resolvable in _initializables)
      {
        resolvable.Run();
      }

      return TaskResultFromTypeAsync(AsyncActionResultType.Complete);
    }
  }
}