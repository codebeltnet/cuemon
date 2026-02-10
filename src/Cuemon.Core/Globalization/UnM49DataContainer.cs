using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Cuemon.Reflection;

namespace Cuemon.Globalization;

internal sealed class UnM49DataContainer
{
    internal UnM49DataContainer()
    {
        var (regions, countries) = LoadUnM49Data();
        BuildHierarchy(regions, countries);
    }

    internal List<StatisticalRegionInfo> Regions { get; } = new();

    internal Dictionary<string, StatisticalRegionInfo> RegionsByCode { get; } = new();

    internal Dictionary<string, StatisticalRegionInfo> CountriesByCode { get; } = new();

    internal Dictionary<string, StatisticalRegionInfo> CountriesByIsoAlpha2 { get; } = new(StringComparer.OrdinalIgnoreCase);

    private (List<Unm49RegionData> Regions, List<Unm49CountryData> Countries) LoadUnM49Data()
    {
        var resourceName = $"{nameof(Cuemon)}.{nameof(Globalization)}.unm49-data.csv";
        var regions = new List<Unm49RegionData>();
        var countries = new List<Unm49CountryData>();

        using (var stream = Decorator.Enclose(typeof(StatisticalRegionInfo).Assembly).GetManifestResources(resourceName)
                   .Single().Value)
        {
            if (stream == null)
            {
                throw new InvalidOperationException($"Embedded resource '{resourceName}' not found.");
            }

            using (var reader = new StreamReader(stream))
            {
                // Skip header line
                reader.ReadLine();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = ParseCsvLine(line);
                    if (parts.Length < 5) continue;

                    var type = parts[0];
                    var code = parts[1];
                    var name = parts[2];
                    var parentCode = parts[3];
                    var kind = parts[4];

                    if (type == "Region")
                    {
                        regions.Add(new Unm49RegionData
                        {
                            Code = code,
                            Name = name,
                            ParentCode = string.IsNullOrEmpty(parentCode) ? null : parentCode,
                            Kind = kind
                        });
                    }
                    else if (type == "Country" && parts.Length >= 10)
                    {
                        countries.Add(new Unm49CountryData
                        {
                            Code = code,
                            Name = name,
                            ParentCode = parentCode,
                            Kind = kind,
                            IsoAlpha2 = string.IsNullOrEmpty(parts[5]) ? null : parts[5],
                            IsoAlpha3 = string.IsNullOrEmpty(parts[6]) ? null : parts[6],
                            Ldc = parts[7] == "true",
                            Lldc = parts[8] == "true",
                            Sids = parts[9] == "true"
                        });
                    }
                }
            }
        }

        return (regions, countries);
    }

    private static string[] ParseCsvLine(string line)
    {
        var parts = new List<string>();
        var current = new System.Text.StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    // Escaped quote
                    current.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                parts.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        parts.Add(current.ToString());
        return parts.ToArray();
    }

    private void BuildHierarchy(List<Unm49RegionData> regions, List<Unm49CountryData> countries)
    {
        // First pass: Create all region objects
        foreach (var regionData in regions)
        {
            var kind = ParseKind(regionData.Kind, regionData.Code, regionData.Name);
            var region = new StatisticalRegionInfo(regionData.Code, regionData.Name, kind, null);
            Regions.Add(region);
            RegionsByCode[regionData.Code] = region;
        }

        // Second pass: Set up parent-child relationships for regions
        foreach (var regionData in regions)
        {
            if (!string.IsNullOrEmpty(regionData.ParentCode) &&
                RegionsByCode.TryGetValue(regionData.ParentCode, out var parent))
            {
                var region = RegionsByCode[regionData.Code];
                region.Parent = parent;
                parent.AddChild(region);
            }
        }

        // Third pass: Create country objects as children of their parent regions
        foreach (var countryData in countries)
        {
            if (RegionsByCode.TryGetValue(countryData.ParentCode, out var parent))
            {
                // Validate kind is CountryOrTerritory
                var kind = ParseKind(countryData.Kind, countryData.Code, countryData.Name);
                if (kind != StatisticalRegionKind.CountryOrTerritory)
                {
                    throw new InvalidOperationException(
                        $"Country {countryData.Name} ({countryData.Code}) must have kind 'CountryOrTerritory', but was '{countryData.Kind}'.");
                }

                // Try to find matching RegionInfo
                RegionInfo regionInfo = null;
                try
                {
                    regionInfo = World.Regions.FirstOrDefault(r => string.Equals(r.TwoLetterISORegionName, countryData.IsoAlpha2, StringComparison.OrdinalIgnoreCase));
                }
                catch
                {
                    // Some territories may not be supported by the OS
                }

                var country = new StatisticalRegionInfo(
                    countryData.Code,
                    countryData.Name,
                    countryData.IsoAlpha2,
                    countryData.IsoAlpha3,
                    parent,
                    countryData.Ldc,
                    countryData.Lldc,
                    countryData.Sids,
                    regionInfo);

                CountriesByCode[countryData.Code] = country;
                if (!string.IsNullOrEmpty(countryData.IsoAlpha2))
                {
                    CountriesByIsoAlpha2[countryData.IsoAlpha2] = country;
                }

                // Add country as child of its immediate parent region
                parent.AddChild(country);
            }
        }

        // Validate hierarchy integrity
        ValidateHierarchy();
    }

    private static StatisticalRegionKind ParseKind(string kindString, string code, string name)
    {
        if (string.IsNullOrEmpty(kindString))
        {
            throw new InvalidOperationException($"Kind is missing for entry {name} ({code}).");
        }

        if (!Enum.TryParse<StatisticalRegionKind>(kindString, out var kind))
        {
            throw new InvalidOperationException(
                $"Unknown kind '{kindString}' for entry {name} ({code}).");
        }

        return kind;
    }

    private void ValidateHierarchy()
    {
        // Validate World has no parent
        if (!RegionsByCode.TryGetValue("001", out var world) || world == null)
        {
            throw new InvalidOperationException("World region (code '001') is missing.");
        }

        if (world.Parent != null)
        {
            throw new InvalidOperationException("World region must not have a parent.");
        }

        if (world.Kind != StatisticalRegionKind.World)
        {
            throw new InvalidOperationException("World region must have kind 'World'.");
        }

        // Validate all non-World regions have a parent
        foreach (var region in Regions)
        {
            if (region.Code != "001" && region.Parent == null)
            {
                throw new InvalidOperationException(
                    $"Region {region.Name} ({region.Code}) must have a parent.");
            }
        }

        // Validate countries have no children
        foreach (var country in CountriesByCode.Values)
        {
            if (country.Children.Any())
            {
                throw new InvalidOperationException(
                    $"Country {country.Name} ({country.Code}) cannot have children.");
            }

            if (country.Kind != StatisticalRegionKind.CountryOrTerritory)
            {
                throw new InvalidOperationException(
                    $"Country {country.Name} ({country.Code}) must have kind 'CountryOrTerritory'.");
            }
        }

        // Validate hierarchy depth does not exceed reasonable limits
        foreach (var region in Regions)
        {
            var depth = GetDepth(region);
            if (depth > 4)
            {
                throw new InvalidOperationException(
                    $"Region {region.Name} ({region.Code}) exceeds maximum hierarchy depth of 4 (actual: {depth}).");
            }
        }
    }

    private static int GetDepth(StatisticalRegionInfo region)
    {
        int depth = 0;
        var current = region;
        while (current.Parent != null)
        {
            depth++;
            current = current.Parent;
        }
        return depth;
    }

    /// <summary>
    /// Internal class for CSV deserialization of UN M.49 region data.
    /// </summary>
    private sealed class Unm49RegionData
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentCode { get; set; }
        public string Kind { get; set; }
    }

    /// <summary>
    /// Internal class for CSV deserialization of UN M.49 country data.
    /// </summary>
    private sealed class Unm49CountryData
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentCode { get; set; }
        public string IsoAlpha2 { get; set; }
        public string IsoAlpha3 { get; set; }
        public bool Ldc { get; set; }
        public bool Lldc { get; set; }
        public bool Sids { get; set; }
        public string Kind { get; set; }
    }
}
