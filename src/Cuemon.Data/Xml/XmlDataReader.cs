using System;
using System.Collections.Specialized;
using System.Xml;
using Cuemon.Text;

namespace Cuemon.Data.Xml;
/// <summary>
/// Provides a way of reading a forward-only stream of rows from an XML based data source. This class cannot be inherited.
/// </summary>
public sealed class XmlDataReader : DataReader<bool>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XmlDataReader"/> class.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
    /// <param name="parser">The function delegate that returns a primitive object whose value is equivalent to the provided <see cref="string"/> value. Default is <see cref="ParserFactory.FromValueType"/>.</param>
    /// <param name="setup">The <see cref="FormattingOptions"/> which may be configured.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="reader"/> is null.
    /// </exception>
    public XmlDataReader(XmlReader reader, Func<string, object> parser = null, Action<FormattingOptions> setup = null)
    {
        Validator.ThrowIfNull(reader);
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        Reader = reader;
        Parser = parser ?? (s => ParserFactory.FromValueType().Parse(s, o => o.FormatProvider = options.FormatProvider));
    }

    private Func<string, object> Parser { get; set; }

    private XmlReader Reader { get; }

    /// <summary>
    /// Gets a value indicating the depth of nesting for the current element.
    /// </summary>
    /// <value>The level of nesting.</value>
    public override int Depth => CurrentDepth;

    private int CurrentDepth { get; set; }

    /// <summary>
    /// Gets the currently processed row count of this instance.
    /// </summary>
    /// <value>The currently processed row count of this instance.</value>
    /// <remarks>This property is incremented when the invoked <see cref="Read"/> method returns <c>true</c>.</remarks>
    public override int RowCount { get; protected set; }

    /// <summary>
    /// Gets the value that indicates that no more rows exists.
    /// </summary>
    /// <value>The value that indicates that no more rows exists.</value>
    protected override bool NullRead => false;

    /// <summary>
    /// Advances this instance to the next element of the XML data source.
    /// </summary>
    /// <returns><c>true</c> if there are more elements; otherwise, <c>false</c>.</returns>
    /// <exception cref="ObjectDisposedException">
    /// This instance has been disposed.
    /// </exception>
    public override bool Read()
    {
        Validator.ThrowIfDisposed(Disposed, GetType());
        return ReadNext(default);
    }

    /// <summary>
    /// Advances this instance to the next element of the XML data source.
    /// </summary>
    /// <returns><c>true</c> if there are more elements; otherwise, <c>false</c>.</returns>
    /// <exception cref="ObjectDisposedException">
    /// This instance has been disposed.
    /// </exception>
    protected override bool ReadNext(bool columns)
    {
        Validator.ThrowIfDisposed(Disposed, GetType());
        var fields = new OrderedDictionary(StringComparer.OrdinalIgnoreCase);
        var skipIterateForward = false;
        string elementName = null;

        while (Reader.Read())
        {
            CurrentDepth = Reader.Depth;

            if (Reader.NodeType == XmlNodeType.Element)
            {
                elementName = Reader.LocalName;
                if (Reader.HasAttributes && Reader.MoveToFirstAttribute())
                {
                    skipIterateForward = CopyAllAttributesFromCurrentNodeToFields(fields);
                }
            }
            else if (Reader.NodeType == XmlNodeType.Text || Reader.NodeType == XmlNodeType.CDATA)
            {
                PopulateFields(elementName, fields);
            }
            else
            {
                skipIterateForward = fields.Count > 0;
            }

            if (skipIterateForward) { break; }
        }

        return ReadNextIncrementRows(fields);
    }

    private bool CopyAllAttributesFromCurrentNodeToFields(IOrderedDictionary fields)
    {
        PopulateFields(fields);
        while (Reader.MoveToNextAttribute())
        {
            PopulateFields(fields);
        }
        return Reader.MoveToElement();
    }

    private bool ReadNextIncrementRows(IOrderedDictionary fields)
    {
        SetFields(fields);
        var hasRows = fields.Count > 0;
        if (hasRows) { RowCount++; }
        return hasRows;
    }

    private void PopulateFields(IOrderedDictionary fields)
    {
        PopulateFields(Reader.LocalName, fields);
    }

    private void PopulateFields(string localName, IOrderedDictionary fields)
    {
        if (fields.Contains(localName))
        {
            fields[localName] = Parser(Reader.Value);
        }
        else
        {
            fields.Add(localName, Parser(Reader.Value));
        }
    }

    /// <summary>
    /// Called when this object is being disposed by either <see cref="Disposable.Dispose()" /> or <see cref="Disposable.Dispose(bool)" /> having <c>disposing</c> set to <c>true</c> and <see cref="Disposable.Disposed" /> is <c>false</c>.
    /// </summary>
    protected override void OnDisposeManagedResources()
    {
        Reader?.Dispose();
    }
}
