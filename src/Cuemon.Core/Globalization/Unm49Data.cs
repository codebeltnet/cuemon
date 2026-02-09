using System.Collections.Generic;

namespace Cuemon.Globalization
{
    /// <summary>
    /// Internal class for JSON deserialization of UN M.49 data.
    /// </summary>
    internal sealed class Unm49Data
    {
        /// <summary>
        /// Gets or sets the regions.
        /// </summary>
        public List<Unm49RegionData> Regions { get; set; }

        /// <summary>
        /// Gets or sets the countries.
        /// </summary>
        public List<Unm49CountryData> Countries { get; set; }
    }

    /// <summary>
    /// Internal class for JSON deserialization of UN M.49 region data.
    /// </summary>
    internal sealed class Unm49RegionData
    {
        /// <summary>
        /// Gets or sets the UN M.49 code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the region name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parent region code.
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        /// Gets or sets the kind of region.
        /// </summary>
        public string Kind { get; set; }
    }

    /// <summary>
    /// Internal class for JSON deserialization of UN M.49 country data.
    /// </summary>
    internal sealed class Unm49CountryData
    {
        /// <summary>
        /// Gets or sets the UN M.49 code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the country name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parent region code.
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        /// Gets or sets the ISO 3166-1 alpha-2 code.
        /// </summary>
        public string IsoAlpha2 { get; set; }

        /// <summary>
        /// Gets or sets the ISO 3166-1 alpha-3 code.
        /// </summary>
        public string IsoAlpha3 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a Least Developed Country.
        /// </summary>
        public bool Ldc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a Land Locked Developing Country.
        /// </summary>
        public bool Lldc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a Small Island Developing State.
        /// </summary>
        public bool Sids { get; set; }

        /// <summary>
        /// Gets or sets the kind of region (always "CountryOrTerritory" for countries).
        /// </summary>
        public string Kind { get; set; }
    }
}
