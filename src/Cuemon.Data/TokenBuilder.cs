using System;
using System.Linq;
using System.Text;

namespace Cuemon.Data;
/// <summary>
/// Represents a mutable string of characters optimized for tokens. This class cannot be inherited.
/// </summary>
public sealed class TokenBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TokenBuilder"/> class.
    /// </summary>
    /// <param name="delimiter">The delimiter used to separate tokens of this instance.</param>
    /// <param name="qualifier">The qualifier that surrounds a token.</param>
    /// <param name="tokens">The total number of tokens.</param>
    /// <exception cref="FormatException">
    /// The length of <paramref name="delimiter"/> is not 1 -or-
    /// The length of <paramref name="qualifier"/> is not 1.
    /// </exception>
    public TokenBuilder(string delimiter, string qualifier, int tokens) : this(char.Parse(delimiter), char.Parse(qualifier), tokens)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenBuilder"/> class.
    /// </summary>
    /// <param name="delimiter">The delimiter used to separate tokens of this instance.</param>
    /// <param name="qualifier">The qualifier that surrounds a token.</param>
    /// <param name="tokens">The total number of tokens.</param>
    public TokenBuilder(char delimiter, char qualifier, int tokens)
    {
        Delimiter = delimiter;
        Qualifier = qualifier;
        Tokens = tokens;
    }

    private bool InsideQuotedToken { get; set; }

    /// <summary>
    /// Gets the total number of tokens this builder represents.
    /// </summary>
    /// <value>The total number of tokens this builder represents.</value>
    public int Tokens { get; }

    /// <summary>
    /// Gets the delimiter used to separate tokens of this builder.
    /// </summary>
    /// <value>The delimiter used to separate tokens of this builder.</value>
    public char Delimiter { get; }

    /// <summary>
    /// Gets the qualifier that surrounds a token of this builder.
    /// </summary>
    /// <value>The qualifier that surrounds a token of this builder.</value>
    public char Qualifier { get; }

    private int CurrentTokens { get; set; }

    private StringBuilder Result { get; } = new();

    /// <summary>
    /// Returns a value indicating whether the current state of this builder is valid.
    /// </summary>
    /// <value><c>true</c> if the current state of this builder is valid; otherwise, <c>false</c>.</value>
    public bool IsValid { get; private set; }

    /// <summary>
    /// Appends the specified value to this builder.
    /// </summary>
    /// <param name="value">The value to tokenize.</param>
    /// <returns>A reference to this instance.</returns>
    public TokenBuilder Append(string value)
    {
        if (value != null)
        {
            Parse(value.ToCharArray());
        }
        return this;
    }

    private void Parse(char[] input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            var current = input[i];
            if ((current == Delimiter || i == (input.Length - 1)) && !InsideQuotedToken)
            {
                CurrentTokens++;
            }

            if (current == Delimiter && input.ElementAtOrDefault(i + 1) == Qualifier)
            {
                InsideQuotedToken = true;
            }

            if (current == Qualifier && input.ElementAtOrDefault(i + 1) == Delimiter)
            {
                InsideQuotedToken = false;
            }

            Result.Append(current);

            if (CurrentTokens == Tokens)
            {
                IsValid = true;
                break;
            }
        }
    }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return Result.ToString();
    }
}
