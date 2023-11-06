using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace Ball.Core.Binding
{
  public sealed class ServiceLocator
  {
    #region Singleton

    private static ServiceLocator _locator;

    private static ServiceLocator Instance
    {
      get
      {
        if (_locator == null)
        {
          _locator = new ServiceLocator();
        }

        return _locator;
      }
    }

    #endregion

    private readonly Dictionary<Type, BindingContract> _bindings;
    private readonly Dictionary<Type, List<Action<object>>> _lazyResolvers;

    private ServiceLocator()
    {
      _bindings = new Dictionary<Type, BindingContract>();
      _lazyResolvers = new Dictionary<Type, List<Action<object>>>();
    }

    public static T Resolve<T>()
    {
      if (Instance._bindings.TryGetValue(typeof(T), out var contract))
      {
        return (T) contract.ContractValue;
      }

      return default;
    }

    public static void LazyResolve<T>(Action<T> onResolved) where T : class
    {
      if (Instance._bindings.TryGetValue(typeof(T), out var contract))
      {
        onResolved.Invoke((T) contract.ContractValue);

        return;
      }

      if (!Instance._lazyResolvers.ContainsKey(typeof(T)))
      {
        Instance._lazyResolvers.Add(typeof(T), new List<Action<object>>());
      }

      Instance._lazyResolvers[typeof(T)].Add(o => onResolved.Invoke((T) o));
    }

    public static IEnumerable<T> ResolveMany<T>(string context = "")
    {
      foreach (var binding in Instance._bindings)
      {
        if (typeof(T).IsAssignableFrom(binding.Value.ContractValue.GetType())
            && (context?.Length == 0 || binding.Value.Context == context))
        {
          yield return (T) binding.Value.ContractValue;
        }
      }
    }

    public static void Bind(Type[] contracts, string context, object instance)
    {
      foreach (var contract in contracts)
      {
        Instance._bindings[contract] = new BindingContract()
        {
          Context = context,
          ContractValue = instance
        };

        Debug.Log($"Bind instance of {instance.GetType().Name} as {contract.Name}");
        if (Instance._lazyResolvers.TryGetValue(contract, out var resolvers))
        {
          foreach (var resolver in resolvers)
          {
            resolver?.Invoke(instance);
          }

          Instance._lazyResolvers.Remove(contract);
        }
      }
    }

    public static void Unbind(Type contract)
    {
      var bindings = Instance._bindings;
      if (bindings.TryGetValue(contract, out var bindingContract))
      {
        bindingContract.ContractValue = null;
        bindings.Remove(contract);
      }
    }

    public static void UnbindContext(string context)
    {
      var bindings = Instance._bindings;
      var contracts = new List<Type>();
      foreach (var binding in bindings)
      {
        if (binding.Value.Context == context)
        {
          contracts.Add(binding.Key);
        }
      }

      foreach (var contract in contracts)
      {
        Unbind(contract);
      }
    }
}
}
