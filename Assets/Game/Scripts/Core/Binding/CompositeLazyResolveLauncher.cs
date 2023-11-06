using System;
using System.Collections.Generic;

namespace Ball.Core.Binding
{
  public class CompositeLazyResolveLauncher
  {
    private readonly List<Action> _resolveActions = new List<Action>();
    private Action _onEverythingResolved;

    private int _unresolvedLeft;

    public CompositeLazyResolveLauncher AddLazyResolve<T>(Action<T> onResolve) where T : class
    {
      _unresolvedLeft++;
      _resolveActions.Add(() => ServiceLocator.LazyResolve<T>(obj =>
      {
        onResolve?.Invoke(obj);
        _unresolvedLeft--;
        if (_unresolvedLeft == 0)
          _onEverythingResolved.Invoke();
      }));

      return this;
    }

    public CompositeLazyResolveLauncher CompleteResolvesAction(Action onEverythingResolved)
    {
      _onEverythingResolved = onEverythingResolved;
      return this;
    }

    public void Launch()
    {
      if (_onEverythingResolved == null)
        throw new ArgumentNullException($"In order to launch sequence you need to set {nameof(_onEverythingResolved)} action");

      if (_resolveActions.Count == 0)
        throw new ArgumentOutOfRangeException($"In order to launch sequence you need to set {nameof(_resolveActions)}");

      foreach (var resolveAction in _resolveActions)
        resolveAction.Invoke();
    }
  }
}
