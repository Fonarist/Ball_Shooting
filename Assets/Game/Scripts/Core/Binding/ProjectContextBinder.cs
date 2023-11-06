using System;
using System.Linq;
using Ball.Core.Modules;
using UnityEngine;

namespace Ball.Core.Binding
{
    public static class ProjectContextBinder
    {
        private const string ContextID = "Project";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void BindInstances()
        {
          var moduleType = typeof(IModule);
          // todoV
          // var repoType = typeof(IRepository);
          var allInheritors = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => moduleType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(Activator.CreateInstance);

          foreach (var bindableInstance in allInheritors)
          {
            var bindable = (IBindable)bindableInstance;
            ServiceLocator.Bind(bindable.Contracts, ContextID, bindableInstance);
          }
        }
    }
}
