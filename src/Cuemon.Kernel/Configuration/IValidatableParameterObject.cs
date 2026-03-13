namespace Cuemon.Configuration
{
    /// <summary>
    /// Denotes a Parameter Object where one or more conditions can be verified that they are in a valid state.
    /// </summary>
    /// <seealso cref="IParameterObject" />
    public interface IValidatableParameterObject : IParameterObject
    {
        /// <summary>
        /// Determines whether the public read-write properties of this instance are in a valid state.
        /// </summary>
        /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
        void ValidateOptions();
    }
}
