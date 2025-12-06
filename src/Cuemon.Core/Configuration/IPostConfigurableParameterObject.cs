namespace Cuemon.Configuration
{
    /// <summary>
    /// Denotes a Parameter Object that supports post-configuration logic after its public properties have been set.
    /// </summary>
    public interface IPostConfigurableParameterObject : IParameterObject
    {
        /// <summary>
        /// Performs post-configuration logic based on the current state of the options.
        /// </summary>
        void PostConfigureOptions();
    }
}
