using System;
using System.Collections.Generic;

namespace Ball.Core.AsyncSequence
{
  public struct Result<T> : IEquatable<Result<T>>
  {
    public T Value
    {
      get
      {
        ThrowIfError(this);
        return _value;
      }
    }

    public bool IsError { get; }
    public bool IsValue => !IsError;

    public string Error
    {
      get
      {
        ThrowIfNotError(this);
        return _error;
      }
    }

    private readonly T _value;
    private readonly string _error;

    public static Result<T> FromValue(T value) { return new Result<T>(value, string.Empty, false); }

    public static Result<T> FromError(string error) { return new Result<T>(default, error, true); }

    private Result(T value, string error, bool isError)
    {
      _value = value;
      _error = error;
      IsError = isError;
    }

    private void ThrowIfError(Result<T> result)
    {
      if (result.IsError)
      {
        throw new Exception("Trying to use value in error result!");
      }
    }

    private void ThrowIfNotError(Result<T> result)
    {
      if (result.IsValue)
      {
        throw new Exception("Trying to use Error in result with value!");
      }
    }

    public bool Equals(Result<T> other)
    {
      if (IsError)
        return other.IsError && other.Error == Error;

      return EqualityComparer<T>.Default.Equals(_value, other._value);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;

      return obj is Result<T> result && Equals(result);
    }

    public override int GetHashCode()
    {
      return IsError
        ? Error.GetHashCode()
        : EqualityComparer<T>.Default.GetHashCode(_value);
    }

    public static bool operator ==(Result<T> lhs, Result<T> rhs) { return lhs.Equals(rhs); }

    /// <summary>
    /// Check that results are not equal.
    /// </summary>
    public static bool operator !=(Result<T> lhs, Result<T> rhs) { return !(lhs == rhs); }
  }
}
