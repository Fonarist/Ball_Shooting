using System.Collections.Generic;
using Ball.Core.Features;
using Ball.Extensions;
using UnityEngine;

namespace Ball.Core.Binding
{
  public abstract class BaseSceneContextBinder : MonoBehaviour
  {
    [SerializeField] private List<BaseMonoFeature> _monoFeatures;

    protected virtual string ContextId => this.GetContext();

    protected abstract IEnumerable<IFeature> Features { get; }

    private void Awake() { BindInstances(); }

    protected virtual void BindInstances()
    {
      foreach (var monoFeature in _monoFeatures)
      {
        ServiceLocator.Bind(monoFeature.Contracts, ContextId, monoFeature);
      }

      foreach (var feature in Features)
      {
        ServiceLocator.Bind(feature.Contracts, ContextId, feature);
      }
    }

    protected virtual void UnbindInstances()
    {
      foreach (var monoFeature in _monoFeatures)
      {
        foreach (var contract in monoFeature.Contracts)
        {
          ServiceLocator.Unbind(contract);
        }
      }

      foreach (var bindableInstance in Features)
      {
        if (bindableInstance is IResolvable resolvable)
        {
          resolvable.Resolve();
        }
      }
    }
  }
}
