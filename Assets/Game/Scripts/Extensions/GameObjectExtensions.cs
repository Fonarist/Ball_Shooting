using System.Linq;
using System.Text;
using UnityEngine;

namespace Ball.Extensions
{
  public static class GameObjectExtensions
  {
    public static string GetContext(this GameObject gameObject) =>
      gameObject.scene.name;

    public static void SetLayerRecursively(this GameObject go, LayerMask layer, Transform[] exclude)
    {
      go.layer = layer.ToLayer();
      foreach (Transform child in go.transform)
      {
        if (!exclude.Contains(child))
          child.gameObject.layer = layer.ToLayer();

        if (child.childCount != 0)
          child.gameObject.SetLayerRecursively(layer, exclude);
      }
    }

    public static T GetComponentOrAdd<T>(this GameObject gameObject)
      where T : Component
    {
      var t = gameObject.GetComponent<T>();
      if (!t)
        t = gameObject.AddComponent<T>();

      return t;
    }

    public static string GetPathFromRoot(this GameObject gameObject, string separator = "\\")
    {
      var stringBuilder = new StringBuilder(100);

      FetchPathFromRoot(stringBuilder, gameObject, separator);

      return stringBuilder.ToString();
    }

    private static void FetchPathFromRoot(StringBuilder stringBuilder, GameObject gameObject, string separator)
    {
      var currentTransform = gameObject.transform;

      stringBuilder.Insert(0, currentTransform.name);

      while (currentTransform.parent)
      {
        stringBuilder.Insert(0, separator);
        stringBuilder.Insert(0, currentTransform.parent.name);

        currentTransform = currentTransform.parent;
      }
    }
  }
}
