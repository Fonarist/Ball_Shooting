using System.Threading;
using System.Threading.Tasks;
using Ball.Core.AsyncSequence;

namespace Ball.Core.Placements
{
  public interface IPlacementsModule
  {
    public void RegisterPlacement<T>(IAsyncSequenceBuilder sequenceBuilder) where T : Placement;
    public void UnregisterPlacement<T>() where T : Placement;
    public Task InvokePlacementAsync<T>(T placement, CancellationToken cancellationToken) where T : Placement;
    public void InvokePlacement<T>(T placement) where T : Placement;
  }
}