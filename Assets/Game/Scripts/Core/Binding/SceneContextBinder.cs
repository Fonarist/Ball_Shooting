using System;
using System.Linq;
using Ball.Core.Modules;
using UnityEngine;

namespace Ball.Core.Binding
{
  public class SceneContextBinder : MonoBehaviour
  {
    private string Context => gameObject.scene.name;

    protected virtual void Awake()
    {
      var nonMonoBindables = typeof(IBindable);
      var moduleType = typeof(IModule);
      // todoV
      // var repoType = typeof(IRepository);
      var excludeType = typeof(MonoBehaviour);
      var allInheritors = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(x => x.GetTypes())
        .Where(x => nonMonoBindables.IsAssignableFrom(x) && !moduleType.IsAssignableFrom(x) && !excludeType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
        .Select(Activator.CreateInstance);

      foreach (var bindableInstance in allInheritors)
      {
        var bindable = (IBindable)bindableInstance;
        ServiceLocator.Bind(bindable.Contracts, Context, bindableInstance);
      }

      var allMonoBindables = FindObjectsOfType<MonoBindable>(true);
      foreach (var bindableInstance in allMonoBindables)
      {
        ServiceLocator.Bind(bindableInstance.Contracts, Context, bindableInstance);
      }
    }
  }
}