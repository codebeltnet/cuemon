namespace Cuemon
{
    /// <summary>
    /// Defines a decorator that exposes the inner wrapped type.
    /// </summary>
    /// <typeparam name="T">The type of the inner wrapped object. This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived.</typeparam>
    /// <seealso cref="Decorator"/>
    /// <seealso cref="Decorator{T}"/>
    public interface IDecorator<out T>
    {
        /// <summary>
        /// Gets the inner object of this decorator.
        /// </summary>
        /// <value>The inner object of this decorator.</value>
        T Inner { get; }

        /// <summary>
        /// Gets the name of the argument from which this decorator originated.
        /// </summary>
        /// <value>The name of the argument from which this decorator originated.</value>
        string ArgumentName { get; }
    }
}
