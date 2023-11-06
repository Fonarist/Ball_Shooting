using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Ball.Extensions;
using Ball.Core.AsyncSequence;
using Ball.Core.Modules;
using UnityEngine;

namespace Ball.Core.Placements
{
  public class PlacementsModule : BaseModule, IPlacementsModule
  {
    public override Type[] Contracts => new[] { typeof(IPlacementsModule) };

    private readonly Dictionary<Type, IAsyncSequenceBuilder> _sequenceBuilders;
    private readonly Queue<Placement> _placementsQueue;

    private bool _isPlacementPlaying;

    public PlacementsModule()
    {
      _sequenceBuilders = new Dictionary<Type, IAsyncSequenceBuilder>();
      _placementsQueue = new Queue<Placement>();
    }

    public void RegisterPlacement<T>(IAsyncSequenceBuilder sequenceBuilder) where T : Placement
    {
      _sequenceBuilders[typeof(T)] = sequenceBuilder;
    }

    public void UnregisterPlacement<T>() where T : Placement
    {
      if (_sequenceBuilders.ContainsKey(typeof(T)))
      {
        _sequenceBuilders.Remove(typeof(T));
      }
    }

    public async Task InvokePlacementAsync<T>(T placement, CancellationToken cancellationToken) where T : Placement
    {
      if (_sequenceBuilders.TryGetValue(placement.GetType(), out var queueBuilder))
      {
        var sequence = queueBuilder.BuildSequence();
        _isPlacementPlaying = true;
        var sequenceResult = await sequence.InvokeAsync(cancellationToken, placement.Context).ConfigureAwait(true);
        _isPlacementPlaying = false;
        Debug.Log($"[{Tag}] Queue for placement {placement.GetType()} complete with result {sequenceResult}");
        if (_placementsQueue.Count > 0)
        {
          var nextPlacement = _placementsQueue.Dequeue();
          if (nextPlacement != null)
          {
            await InvokePlacementAsync(nextPlacement, cancellationToken).ConfigureAwait(true);
          }
        }

        return;
      }

      Debug.LogError($"[{Tag}]Failed to build queue for placement {typeof(T).Name}");
    }

    public void InvokePlacement<T>(T placement) where T : Placement
    {
      if (_isPlacementPlaying)
      {
        _placementsQueue.Enqueue(placement);
      }
      else
      {
        InvokePlacementAsync(placement, CancellationToken.None).Forget();
      }
    }
  }
}