using System;
using UnityEngine;

namespace Ball.Core.Binding
{
  public abstract class MonoBindable : MonoBehaviour, IBindable
  {
    public abstract Type[] Contracts { get; }
    public abstract void LateDispose();
  }
}
