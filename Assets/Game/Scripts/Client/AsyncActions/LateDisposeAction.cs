using System.Threading;
using System.Threading.Tasks;
using Ball.Core.AsyncSequence;
using Ball.Core.Binding;

namespace Ball.Client.AsyncActions
{
  public class LateDisposeAction : BaseAsyncAction
  {
    private readonly string _context;

    public LateDisposeAction(string context = "") =>
      _context = context;

    public override Task<Result<AsyncActionResult>> InvokeAsync(
      AsyncActionSequence sequence,
      AsyncSequenceContext context,
      CancellationToken cancellationToken)
    {
      var lateDisposables = ServiceLocator.ResolveMany<ILateDisposable>(_context);
      foreach (var disposable in lateDisposables)
      {
        disposable.LateDispose();
      }

      ServiceLocator.UnbindContext(_context);

      return TaskResultFromTypeAsync(AsyncActionResultType.Complete);
    }
  }
}
