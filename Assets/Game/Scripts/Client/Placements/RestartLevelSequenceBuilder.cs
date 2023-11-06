using Ball.Client.AsyncActions;
using Ball.Core.AsyncSequence;

namespace Ball.Client.Placements
{
  public class RestartLevelSequenceBuilder : IAsyncSequenceBuilder
  {
    public AsyncActionSequence BuildSequence() =>
      new AsyncActionSequence()
        .AddAction(new LateDisposeAction("MainScene"))
        .AddAction(new LoadSceneAction("MainScene"))
        .AddAction(new InvokePlacementAction<ApplicationStartPlacement>(new ApplicationStartPlacement()));
  }
}
