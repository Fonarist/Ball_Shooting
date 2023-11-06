using System;

namespace Ball.Core.Binding
{
    public interface IBindable : ILateDisposable
    {
        Type[] Contracts { get; }
    }
}