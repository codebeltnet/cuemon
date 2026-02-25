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
        public void Regions_ShouldPrintAllRegionsAndHighlightIsoCodeFrequency()
        {
            var sut1 = World.Regions.ToList();

            foreach (var r in sut1)
            {
                TestOutput.WriteLine($"{r.Name,-5} {r.EnglishName}");
            }

            var grouped = sut1.GroupBy(r => r.Name).OrderBy(g => g.Key).ToList();
            var multiEntry = grouped.Where(g => g.Count() > 1).OrderByDescending(g => g.Count()).ThenBy(g => g.Key).ToList();

            TestOutput.WriteLine($"Total: {sut1.Count}, Unique ISO codes: {grouped.Count}, ISO codes with multiple entries: {multiEntry.Count}");

            foreach (var g in multiEntry)
            {
                var first = g.First();
                var allEqual = g.All(r => r.Equals(first));
                var distinctNativeNames = g.Select(r => r.NativeName).Distinct().ToList();
                TestOutput.WriteLine($"  {g.Key} ({first.EnglishName}): {g.Count()} entries | all Equals: {allEqual} | distinct NativeNames: {distinctNativeNames.Count}");
                if (distinctNativeNames.Count > 1)
                {
                    foreach (var name in distinctNativeNames)
                    {
                        TestOutput.WriteLine($"    NativeName: {name}");
                    }
                }
            }

            Assert.NotEmpty(sut1);
#if NET48_OR_GREATER
            Assert.True(sut1.Count > 100, "sut1.Count > 100");
#else
            Assert.True(sut1.Count > 400, "sut1.Count > 400");
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

        [Theory]
        [InlineData("001")]
        [InlineData("002")]
        [InlineData("004")]
        [InlineData("005")]
        [InlineData("008")]
        [InlineData("009")]
        [InlineData("010")]
        [InlineData("011")]
        [InlineData("012")]
        [InlineData("013")]
        [InlineData("014")]
        [InlineData("015")]
        [InlineData("016")]
        [InlineData("017")]
        [InlineData("018")]
        [InlineData("019")]
        [InlineData("020")]
        [InlineData("021")]
        [InlineData("024")]
        [InlineData("028")]
        [InlineData("029")]
        [InlineData("030")]
        [InlineData("031")]
        [InlineData("032")]
        [InlineData("034")]
        [InlineData("035")]
        [InlineData("036")]
        [InlineData("039")]
        [InlineData("040")]
        [InlineData("044")]
        [InlineData("048")]
        [InlineData("050")]
        [InlineData("051")]
        [InlineData("052")]
        [InlineData("053")]
        [InlineData("054")]
        [InlineData("056")]
        [InlineData("057")]
        [InlineData("060")]
        [InlineData("061")]
        [InlineData("064")]
        [InlineData("068")]
        [InlineData("070")]
        [InlineData("072")]
        [InlineData("074")]
        [InlineData("076")]
        [InlineData("084")]
        [InlineData("086")]
        [InlineData("090")]
        [InlineData("092")]
        [InlineData("096")]
        [InlineData("100")]
        [InlineData("104")]
        [InlineData("108")]
        [InlineData("112")]
        [InlineData("116")]
        [InlineData("120")]
        [InlineData("124")]
        [InlineData("132")]
        [InlineData("136")]
        [InlineData("140")]
        [InlineData("142")]
        [InlineData("143")]
        [InlineData("144")]
        [InlineData("145")]
        [InlineData("148")]
        [InlineData("150")]
        [InlineData("151")]
        [InlineData("152")]
        [InlineData("154")]
        [InlineData("155")]
        [InlineData("156")]
        [InlineData("162")]
        [InlineData("166")]
        [InlineData("170")]
        [InlineData("174")]
        [InlineData("175")]
        [InlineData("178")]
        [InlineData("180")]
        [InlineData("184")]
        [InlineData("188")]
        [InlineData("191")]
        [InlineData("192")]
        [InlineData("196")]
        [InlineData("202")]
        [InlineData("203")]
        [InlineData("204")]
        [InlineData("208")]
        [InlineData("212")]
        [InlineData("214")]
        [InlineData("218")]
        [InlineData("222")]
        [InlineData("226")]
        [InlineData("231")]
        [InlineData("232")]
        [InlineData("233")]
        [InlineData("234")]
        [InlineData("238")]
        [InlineData("239")]
        [InlineData("242")]
        [InlineData("246")]
        [InlineData("248")]
        [InlineData("250")]
        [InlineData("254")]
        [InlineData("258")]
        [InlineData("260")]
        [InlineData("262")]
        [InlineData("266")]
        [InlineData("268")]
        [InlineData("270")]
        [InlineData("275")]
        [InlineData("276")]
        [InlineData("288")]
        [InlineData("292")]
        [InlineData("296")]
        [InlineData("300")]
        [InlineData("304")]
        [InlineData("308")]
        [InlineData("312")]
        [InlineData("316")]
        [InlineData("320")]
        [InlineData("324")]
        [InlineData("328")]
        [InlineData("332")]
        [InlineData("334")]
        [InlineData("336")]
        [InlineData("340")]
        [InlineData("344")]
        [InlineData("348")]
        [InlineData("352")]
        [InlineData("356")]
        [InlineData("360")]
        [InlineData("364")]
        [InlineData("368")]
        [InlineData("372")]
        [InlineData("376")]
        [InlineData("380")]
        [InlineData("384")]
        [InlineData("388")]
        [InlineData("392")]
        [InlineData("398")]
        [InlineData("400")]
        [InlineData("404")]
        [InlineData("408")]
        [InlineData("410")]
        [InlineData("414")]
        [InlineData("417")]
        [InlineData("418")]
        [InlineData("419")]
        [InlineData("422")]
        [InlineData("426")]
        [InlineData("428")]
        [InlineData("430")]
        [InlineData("434")]
        [InlineData("438")]
        [InlineData("440")]
        [InlineData("442")]
        [InlineData("446")]
        [InlineData("450")]
        [InlineData("454")]
        [InlineData("458")]
        [InlineData("462")]
        [InlineData("466")]
        [InlineData("470")]
        [InlineData("474")]
        [InlineData("478")]
        [InlineData("480")]
        [InlineData("484")]
        [InlineData("492")]
        [InlineData("496")]
        [InlineData("498")]
        [InlineData("499")]
        [InlineData("500")]
        [InlineData("504")]
        [InlineData("508")]
        [InlineData("512")]
        [InlineData("516")]
        [InlineData("520")]
        [InlineData("524")]
        [InlineData("528")]
        [InlineData("531")]
        [InlineData("533")]
        [InlineData("534")]
        [InlineData("535")]
        [InlineData("540")]
        [InlineData("548")]
        [InlineData("554")]
        [InlineData("558")]
        [InlineData("562")]
        [InlineData("566")]
        [InlineData("570")]
        [InlineData("574")]
        [InlineData("578")]
        [InlineData("580")]
        [InlineData("581")]
        [InlineData("583")]
        [InlineData("584")]
        [InlineData("585")]
        [InlineData("586")]
        [InlineData("591")]
        [InlineData("598")]
        [InlineData("600")]
        [InlineData("604")]
        [InlineData("608")]
        [InlineData("612")]
        [InlineData("616")]
        [InlineData("620")]
        [InlineData("624")]
        [InlineData("626")]
        [InlineData("630")]
        [InlineData("634")]
        [InlineData("638")]
        [InlineData("642")]
        [InlineData("643")]
        [InlineData("646")]
        [InlineData("652")]
        [InlineData("654")]
        [InlineData("659")]
        [InlineData("660")]
        [InlineData("662")]
        [InlineData("663")]
        [InlineData("666")]
        [InlineData("670")]
        [InlineData("674")]
        [InlineData("678")]
        [InlineData("682")]
        [InlineData("686")]
        [InlineData("688")]
        [InlineData("690")]
        [InlineData("694")]
        [InlineData("702")]
        [InlineData("703")]
        [InlineData("704")]
        [InlineData("705")]
        [InlineData("706")]
        [InlineData("710")]
        [InlineData("716")]
        [InlineData("724")]
        [InlineData("728")]
        [InlineData("729")]
        [InlineData("732")]
        [InlineData("740")]
        [InlineData("744")]
        [InlineData("748")]
        [InlineData("752")]
        [InlineData("756")]
        [InlineData("760")]
        [InlineData("762")]
        [InlineData("764")]
        [InlineData("768")]
        [InlineData("772")]
        [InlineData("776")]
        [InlineData("780")]
        [InlineData("784")]
        [InlineData("788")]
        [InlineData("792")]
        [InlineData("795")]
        [InlineData("796")]
        [InlineData("798")]
        [InlineData("800")]
        [InlineData("804")]
        [InlineData("807")]
        [InlineData("818")]
        [InlineData("826")]
        [InlineData("831")]
        [InlineData("832")]
        [InlineData("833")]
        [InlineData("834")]
        [InlineData("840")]
        [InlineData("850")]
        [InlineData("854")]
        [InlineData("858")]
        [InlineData("860")]
        [InlineData("862")]
        [InlineData("876")]
        [InlineData("882")]
        [InlineData("887")]
        [InlineData("894")]
        public void GetStatisticalRegion_ShouldReturnNonNullResult_ForAllUnM49Codes(string code)
        {
            var sut1 = World.GetStatisticalRegion(code);

            Assert.NotNull(sut1);
        }
    }
}
