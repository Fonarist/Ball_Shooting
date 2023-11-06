using System;
using Ball.Client.AsyncActions;
using Ball.Core.AsyncSequence;

namespace Ball.Client.Placements
{
  public class ApplicationStartSequenceBuilder : IAsyncSequenceBuilder
  {
    public AsyncActionSequence BuildSequence() =>
      new AsyncActionSequence()
        .AddAction(new ResolveBindingsAction())
        .AddAction(new InitializeServicesAction())
        .AddAction(new RunServicesAction());
  }
}
