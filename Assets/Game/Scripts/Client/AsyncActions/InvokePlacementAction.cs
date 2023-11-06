using System.Threading;
using System.Threading.Tasks;
using Ball.Core.AsyncSequence;
using Ball.Core.Binding;
using Ball.Core.Placements;

namespace Ball.Client.AsyncActions
{
  public class InvokePlacementAction<T> : BaseAsyncAction where T : Placement
  {
    private readonly T _placement;

    public InvokePlacementAction(T placement) =>
      _placement = placement;

    public override async Task<Result<AsyncActionResult>> InvokeAsync(
      AsyncActionSequence sequence,
      AsyncSequenceContext context,
      CancellationToken cancellationToken)
    {
      var placementsModule = ServiceLocator.Resolve<IPlacementsModule>();
      placementsModule.InvokePlacement(_placement);

      return ResultFromType(AsyncActionResultType.Complete);
    }
  }
}