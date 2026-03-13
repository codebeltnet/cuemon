namespace Cuemon
{
    /// <summary>
    /// Provides a set of alphanumeric constant and static fields that consists of both letters, numbers and other symbols (such as punctuation marks and mathematical symbols).
    /// </summary>
    public static class Alphanumeric
    {
        /// <summary>
        /// A representation of a numeric character set consisting of the numbers 0 to 9.
        /// </summary>
        public const string Numbers = "0123456789";

        /// <summary>
        /// An uppercase representation of an alphabetic character set consisting of the letters A to Z.
        /// </summary>
        public const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// A case sensitive representation of an alphabetic character set consisting of the letters Aa to Zz.
        /// </summary>
        public const string Letters = UppercaseLetters + LowercaseLetters;

        /// <summary>
        /// A case sensitive representation of an alphanumeric character set consisting of the numbers 0 to 9 and the letters Aa to Zz.
        /// </summary>
        public const string LettersAndNumbers = Letters + Numbers;

        /// <summary>
        /// A lowercase representation of an alphabetic character set consisting of the letters a to z.
        /// </summary>
        public const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// A representation of the most common punctuation marks consisting of the characters !@#$%^&amp;*()_-+=[{]};:&lt;&gt;|.,/?`~\"'..
        /// </summary>
        public const string PunctuationMarks = "!@#$%^&*()_-+=[{]};:<>|.,/?`~\\\"'";

        /// <summary>
        /// A representation of the most common whitespace characters.
        /// </summary>
        public const string WhiteSpace = "\u0009\u000A\u000B\u000C\u000D\u0020\u0085\u00A0\u1680\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200A\u2028\u2029\u202F\u205F\u3000";

        /// <summary>
        /// A representation of a hexadecimal character set consisting of the numbers 0 to 9 and the letters A to F.
        /// </summary>
        public const string Hexadecimal = Numbers + "ABCDEF";

        /// <summary>
        /// A network-path reference, e.g. two forward slashes (//).
        /// </summary>
        public const string NetworkPathReference = "//";

        /// <summary>
        /// Tab character.
        /// </summary>
        public const string Tab = "\t";

        /// <summary>
        /// Tab character.
        /// </summary>
        public const char TabChar = '\t';

        /// <summary>
        /// Linefeed character.
        /// </summary>
        public const string Linefeed = "\n";

        /// <summary>
        /// Linefeed character.
        /// </summary>
        public const char LinefeedChar = '\n';

        /// <summary>
        /// Carriage-return character.
        /// </summary>
        public const string CarriageReturn = "\r";

        /// <summary>
        /// Carriage-return character.
        /// </summary>
        public const char CarriageReturnChar = '\r';

        /// <summary>
        /// Circumflex accent / Caret character.
        /// </summary>
        public const string Caret = "^";

        /// <summary>
        /// Circumflex accent / Caret character.
        /// </summary>
        public const char CaretChar = '^';
    }
}
