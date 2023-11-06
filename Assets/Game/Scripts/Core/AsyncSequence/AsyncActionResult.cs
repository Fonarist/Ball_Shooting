namespace Ball.Core.AsyncSequence
{
  public struct AsyncActionResult
  {
    public AsyncActionResultType ResultType { get; }
    public AsyncActionSequence ResultSequence { get; }

    public AsyncActionResult(AsyncActionResultType resultType, AsyncActionSequence resultSequence)
    {
      ResultType = resultType;
      ResultSequence = resultSequence;
    }
  }
}
