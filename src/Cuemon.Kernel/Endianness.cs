namespace Cuemon
{
    /// <summary>
    /// Defines the order in which a sequence of bytes are represented.
    /// </summary>
    public enum Endianness
    {
        /// <summary>
        /// The big endian format means that data is stored big end first. Many hash standards is represented this way.
        /// </summary>
        BigEndian,
        /// <summary>
        /// The little endian format means that data is stored little end first. Most modern OS and hardware uses this.
        /// </summary>
        LittleEndian
    }
}