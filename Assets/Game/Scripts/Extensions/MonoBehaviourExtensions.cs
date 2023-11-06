using System;
using System.Collections;
using UnityEngine;

namespace Ball.Extensions
{
  public static class MonoBehaviourExtensions
  {
    public static string GetContext(this MonoBehaviour monoBehaviour) =>
      monoBehaviour.gameObject.GetContext();
  }
}
