namespace Ball.Core.Binding
{
    public interface IResolvable
    {
        /// <summary>
        /// Initialize all services fields in this method
        /// </summary>
        void Resolve();
    }
}