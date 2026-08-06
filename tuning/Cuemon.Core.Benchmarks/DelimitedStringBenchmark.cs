using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Collections.Generic;
using System.Text;

namespace Cuemon;
[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class DelimitedStringBenchmark
{
    [Params(10, 100, 1000)]
    public int Count { get; set; }

    private List<string> _items = null!;
    private string _delimited = null!;
    private readonly char _delimiter = ',';
    private readonly char _qualifier = '"';

    [GlobalSetup]
    public void Setup()
    {
        _items = new List<string>(Count);
        for (var i = 0; i < Count; i++)
        {
            // include some items that contain delimiters to exercise quoting behavior
            _items.Add(i % 10 == 0 ? $"value {i},with,commas" : $"value{i}");
        }

        var sb = new StringBuilder();
        for (var i = 0; i < Count; i++)
        {
            // mix quoted and unquoted fields to resemble realistic CSV input
            var item = i % 7 == 0
                ? $"{_qualifier}value {i},has,commas{_qualifier}"
                : $"value{i}";
            sb.Append(item).Append(_delimiter);
        }
        _delimited = sb.Length > 0 ? sb.ToString(0, sb.Length - 1) : sb.ToString();
    }

    [Benchmark]
    public string Create() => DelimitedString.Create(_items, o =>
    {
        o.Delimiter = _delimiter.ToString();
    });

    [Benchmark]
    public string[] Split() => DelimitedString.Split(_delimited, o =>
    {
        o.Delimiter = _delimiter.ToString();
        o.Qualifier = _qualifier.ToString();
    });
}
