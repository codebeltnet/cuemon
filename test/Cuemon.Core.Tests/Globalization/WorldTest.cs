using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Globalization
{
    public class WorldTest : Test
    {
        public WorldTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Regions_ShouldContainAllExpectedIsoRegionCodes_ForBackwardCompatibility()
        {
            var expectedTwoLetterIsoCodes = new HashSet<string>(StringComparer.Ordinal)
            {
                "AD", "AE", "AF", "AG", "AI", "AL", "AM", "AO", "AR", "AS", "AT", "AU", "AW", "AX", "AZ",
                "BA", "BB", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BL", "BM", "BN", "BO", "BQ", "BR", "BS", "BT", "BW", "BY", "BZ",
                "CA", "CC", "CD", "CF", "CG", "CH", "CI", "CK", "CL", "CM", "CN", "CO", "CR", "CU", "CV", "CW", "CX", "CY", "CZ",
                "DE", "DJ", "DK", "DM", "DO", "DZ",
                "EC", "EE", "EG", "ER", "ES", "ET",
                "FI", "FJ", "FK", "FM", "FO", "FR",
                "GA", "GB", "GD", "GE", "GF", "GG", "GH", "GI", "GL", "GM", "GN", "GP", "GQ", "GR", "GT", "GU", "GW", "GY",
                "HK", "HN", "HR", "HT", "HU",
                "ID", "IE", "IL", "IM", "IN", "IO", "IQ", "IR", "IS", "IT",
                "JE", "JM", "JO", "JP",
                "KE", "KG", "KH", "KI", "KM", "KN", "KP", "KR", "KW", "KY", "KZ",
                "LA", "LB", "LC", "LI", "LK", "LR", "LS", "LT", "LU", "LV", "LY",
                "MA", "MC", "MD", "ME", "MF", "MG", "MH", "MK", "ML", "MM", "MN", "MO", "MP", "MQ", "MR", "MS", "MT", "MU", "MV", "MW", "MX", "MY", "MZ",
                "NA", "NC", "NE", "NF", "NG", "NI", "NL", "NO", "NP", "NR", "NU", "NZ",
                "OM",
                "PA", "PE", "PF", "PG", "PH", "PK", "PL", "PM", "PN", "PR", "PS", "PT", "PW", "PY",
                "QA",
                "RE", "RO", "RS", "RU", "RW",
                "SA", "SB", "SC", "SD", "SE", "SG", "SH", "SI", "SJ", "SK", "SL", "SM", "SN", "SO", "SR", "SS", "ST", "SV", "SX", "SY", "SZ",
                "TC", "TD", "TG", "TH", "TJ", "TK", "TL", "TM", "TN", "TO", "TR", "TT", "TV", "TW", "TZ",
                "UA", "UG", "UM", "US", "UY", "UZ",
                "VA", "VC", "VE", "VG", "VI", "VN", "VU",
                "WF", "WS",
                "XK",
                "YE", "YT",
                "ZA", "ZM", "ZW"
            };

            var sut1 = World.Regions.ToList();
            var actualCodes = new HashSet<string>(sut1.Select(r => r.Name), StringComparer.Ordinal);

#if NET48_OR_GREATER
            Assert.NotEmpty(actualCodes);
            Assert.True(actualCodes.Count > 100, "actualCodes.Count > 100");
#else
            var missing = expectedTwoLetterIsoCodes.Except(actualCodes).OrderBy(c => c).ToList();
            var added = actualCodes.Except(expectedTwoLetterIsoCodes).OrderBy(c => c).ToList();
            foreach (var code in missing)
            {
                TestOutput.WriteLine($"Missing: {code} - {World.Regions.SingleOrDefault(info => info.Name == code).EnglishName}");
            }
            foreach (var code in added)
            {
                TestOutput.WriteLine($"Added: {code} - {World.Regions.Last(info => info.Name == code).EnglishName}");
            }
            TestOutput.WriteLine($"Expected: {expectedTwoLetterIsoCodes.Count}, Actual: {actualCodes.Count}, Missing: {missing.Count}, Added: {added.Count}");
            Assert.Empty(missing);
#endif
        }

        [Fact]
        public void Regions_ShouldReturnAllRegions()
        {
            var sut1 = World.Regions.ToList();

            TestOutput.WriteLine(sut1.Count.ToString());

            Assert.NotNull(sut1);
#if NET48_OR_GREATER
            Assert.True(sut1.Count > 100, "sut1.Count > 100");
#else
            Assert.True(sut1.Count > 220, "sut1.Count > 220");
#endif
        }

        [Fact]
        public void Regions_ShouldReturnAllCultures_FromRegions()
        {
            var sut1 = World.Regions.ToList();
            var sut2 = new List<CultureInfo>();

            foreach (var region in sut1)
            {
                foreach (var culture in World.GetCultures(region))
                {
                    sut2.Add(culture);
                    if (culture.IsNeutralCulture)
                    {
                        TestOutput.WriteLine(culture.Name);
                    }
                }
            }

            TestOutput.WriteLine(sut2.Count.ToString());

            Assert.NotNull(sut2);
#if NET48_OR_GREATER
            Assert.True(sut2.Count > 200, "sut1.Count > 200");
#else
            Assert.True(sut2.Count > 500, "sut1.Count > 500");
#endif
        }
    }
}
