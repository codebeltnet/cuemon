using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using Cuemon.Reflection;

namespace Cuemon.Globalization;

internal sealed class UnM49DataContainer
{
    internal UnM49DataContainer()
    {
        var data = LoadUnM49Data();
        BuildHierarchy(data);
    }

    internal List<StatisticalRegionInfo> Regions { get; } = new();

    internal Dictionary<string, StatisticalRegionInfo> RegionsByCode { get; } = new();

    internal Dictionary<string, StatisticalRegionInfo> CountriesByCode { get; } = new();

    internal Dictionary<string, StatisticalRegionInfo> CountriesByIsoAlpha2 { get; } = new(StringComparer.OrdinalIgnoreCase);

    internal Unm49Data LoadUnM49Data()
    {
        var resourceName = $"{nameof(Cuemon)}.{nameof(Globalization)}.unm49-data.json";

        using (var stream = Decorator.Enclose(typeof(StatisticalRegionInfo).Assembly).GetManifestResources(resourceName)
                   .Single().Value)
        {
            if (stream == null)
            {
                throw new InvalidOperationException($"Embedded resource '{resourceName}' not found.");
            }

            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                return JsonSerializer.Deserialize<Unm49Data>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }
    }

    private void BuildHierarchy(Unm49Data data)
    {
        // First pass: Create all region objects
        foreach (var regionData in data.Regions)
        {
            var kind = ParseKind(regionData.Kind, regionData.Code, regionData.Name);
            var region = new StatisticalRegionInfo(regionData.Code, regionData.Name, kind, null);
            Regions.Add(region);
            RegionsByCode[regionData.Code] = region;
        }

        // Second pass: Set up parent-child relationships for regions
        foreach (var regionData in data.Regions)
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
        foreach (var countryData in data.Countries)
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
            if (country.Children.Count > 0)
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
}
