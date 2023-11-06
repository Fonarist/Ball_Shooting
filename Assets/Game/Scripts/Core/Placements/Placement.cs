using Ball.Core.AsyncSequence;

namespace Ball.Core.Placements
{
  public abstract class Placement
  {
    public AsyncSequenceContext Context { get; protected set; }
  }
}