namespace Cuemon.Text
{
    /// <summary>
    /// Specifies what action to take in regards to encoding preamble sequences.
    /// </summary>
    public enum PreambleSequence
    {
        /// <summary>
        /// Any encoding preamble sequences will be preserved.
        /// </summary>
        Keep = 0,
        /// <summary>
        /// Any encoding preamble sequences will be removed.
        /// </summary>
        Remove = 1
    }
}