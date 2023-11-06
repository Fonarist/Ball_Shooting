using System;
using Ball.Core.Binding;

namespace Ball.Core.Service
{
    public interface IService : IBindable, IResolvable, IInitializable, IRunnable
    {
    }
}