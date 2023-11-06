using UnityEngine;

namespace Ball.Extensions
{
  public static class LayerMaskExtensions
  {
    public static int ToLayer(this LayerMask bitMask)
    {
      var result = bitMask > 0 ? 0 : 31;
      while (bitMask > 1)
      {
        bitMask >>= 1;
        result++;
      }

      return result;
    }
  }
}
