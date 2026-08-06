namespace Cuemon;
internal sealed class StringReplaceCoordinate
{
    internal StringReplaceCoordinate(int startIndex, int length, string value)
    {
        StartIndex = startIndex;
        Length = length;
        Value = value;
    }

    internal int StartIndex { get; set; }
    internal int Length { get; set; }
    internal string Value { get; set; }
}
