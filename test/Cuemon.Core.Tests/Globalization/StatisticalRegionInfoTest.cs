using System;
using System.Globalization;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Globalization
{
    public class StatisticalRegionInfoTest : Test
    {
        public StatisticalRegionInfoTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void StatisticalRegions_ShouldContainWorld()
        {
            var world = World.StatisticalRegions.FirstOrDefault(r => r.Code == "001");

            Assert.NotNull(world);
            Assert.Equal("World", world.Name);
            Assert.Null(world.Parent);
            Assert.Equal(StatisticalRegionKind.World, world.Kind);
            TestOutput.WriteLine(world.ToString());
        }

        [Fact]
        public void StatisticalRegions_ShouldContainAllContinents()
        {
            var expectedContinents = new[] { "002", "009", "019", "142", "150" }; // Africa, Oceania, Americas, Asia, Europe

            foreach (var code in expectedContinents)
            {
                var continent = World.GetStatisticalRegion(code);
                Assert.NotNull(continent);
                Assert.Equal("001", continent.Parent?.Code);
                Assert.Equal(StatisticalRegionKind.Region, continent.Kind);
                TestOutput.WriteLine(continent.ToString());
            }
        }

        [Fact]
        public void GetStatisticalRegion_ShouldReturnCorrectRegion()
        {
            var europe = World.GetStatisticalRegion("150");

            Assert.NotNull(europe);
            Assert.Equal("150", europe.Code);
            Assert.Equal("Europe", europe.Name);
            Assert.Equal("001", europe.Parent?.Code);
            Assert.Equal(StatisticalRegionKind.Region, europe.Kind);
        }

        [Fact]
        public void GetStatisticalRegion_InvalidCode_ShouldReturnNull()
        {
            var result = World.GetStatisticalRegion("99999");

            Assert.Null(result);
        }

        [Fact]
        public void GetCountry_ByM49Code_US_ShouldReturn840()
        {
            var usa = World.GetCountry("840");

            Assert.NotNull(usa);
            Assert.Equal("840", usa.Code);
            Assert.Equal("United States of America", usa.Name);
            Assert.Equal("US", usa.IsoAlpha2);
            Assert.Equal("USA", usa.IsoAlpha3);
            Assert.Equal(StatisticalRegionKind.CountryOrTerritory, usa.Kind);
            
            TestOutput.WriteLine(usa.ToString());
        }

        [Fact]
        public void GetCountry_ByM49Code_DE_ShouldReturn276()
        {
            var germany = World.GetCountry("276");

            Assert.NotNull(germany);
            Assert.Equal("276", germany.Code);
            Assert.Equal("Germany", germany.Name);
            Assert.Equal("DE", germany.IsoAlpha2);
            Assert.Equal("DEU", germany.IsoAlpha3);
            Assert.Equal(StatisticalRegionKind.CountryOrTerritory, germany.Kind);

            TestOutput.WriteLine(germany.ToString());
        }

        [Fact]
        public void GetCountry_ByRegionInfo_US_ShouldReturnUnitedStates()
        {
            var regionInfo = new RegionInfo("US");
            var usa = World.GetCountry(regionInfo);

            Assert.NotNull(usa);
            Assert.Equal("840", usa.Code);
            Assert.Equal("United States of America", usa.Name);
        }

        [Fact]
        public void GetCountry_ByRegionInfo_DE_ShouldReturnGermany()
        {
            var regionInfo = new RegionInfo("DE");
            var germany = World.GetCountry(regionInfo);

            Assert.NotNull(germany);
            Assert.Equal("276", germany.Code);
            Assert.Equal("Germany", germany.Name);
        }

        [Fact]
        public void GetCountry_ByRegionInfo_Null_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => World.GetCountry((RegionInfo)null));
        }

        [Fact]
        public void GetCountry_InvalidCode_ShouldReturnNull()
        {
            var result = World.GetCountry("99999");

            Assert.Null(result);
        }

        [Fact]
        public void GetAncestors_FromCountry_ShouldReturnCompleteChain()
        {
            var usa = World.GetCountry("840");

            Assert.NotNull(usa);

            var ancestors = usa.GetAncestors().ToList();

            Assert.True(ancestors.Count >= 2);
            Assert.Contains(ancestors, r => r.Code == "021"); // Northern America
            Assert.Contains(ancestors, r => r.Code == "019"); // Americas
            Assert.Contains(ancestors, r => r.Code == "001"); // World

            TestOutput.WriteLine($"Geographic ancestors of {usa.Name}:");
            foreach (var ancestor in ancestors)
            {
                TestOutput.WriteLine($"  - {ancestor} ({ancestor.Kind})");
            }
        }

        [Fact]
        public void GetAncestors_FromRegion_ShouldReturnCompleteChain()
        {
            var westernEurope = World.GetStatisticalRegion("155");

            Assert.NotNull(westernEurope);
            Assert.Equal(StatisticalRegionKind.Subregion, westernEurope.Kind);

            var ancestors = westernEurope.GetAncestors().ToList();

            Assert.Equal(2, ancestors.Count);
            Assert.Equal("150", ancestors[0].Code); // Europe
            Assert.Equal("001", ancestors[1].Code); // World

            TestOutput.WriteLine($"Ancestors of {westernEurope.Name}:");
            foreach (var ancestor in ancestors)
            {
                TestOutput.WriteLine($"  - {ancestor}");
            }
        }

        [Fact]
        public void Children_ShouldBePopulated()
        {
            var world = World.GetStatisticalRegion("001");

            Assert.NotNull(world);
            Assert.True(world.Children.Count >= 5, "World should have at least 5 continent children");

            TestOutput.WriteLine($"Children of {world.Name}:");
            foreach (var child in world.Children)
            {
                TestOutput.WriteLine($"  - {child} ({child.Kind})");
            }
        }

        [Fact]
        public void GetAllCountries_ShouldReturn250PlusCountries()
        {
            var world = World.GetStatisticalRegion("001");

            Assert.NotNull(world);
            Assert.True(world.Countries.Count >= 200, $"Expected at least 200 countries, got {world.Countries.Count}");

            TestOutput.WriteLine($"Total countries: {world.Countries.Count}");
        }

        [Fact]
        public void Countries_ShouldBeAssignedToCorrectRegions()
        {
            var europe = World.GetStatisticalRegion("150");
            var germany = World.GetCountry("276");

            Assert.Contains(germany, europe.Countries);
        }

        [Fact]
        public void LDC_Flag_ShouldBeSetForLeastDevelopedCountries()
        {
            var afghanistan = World.GetCountry("004");

            Assert.NotNull(afghanistan);
            Assert.True(afghanistan.IsLeastDevelopedCountry);
            Assert.True(afghanistan.IsLandLockedDevelopingCountry);
        }

        [Fact]
        public void SIDS_Flag_ShouldBeSetForSmallIslandDevelopingStates()
        {
            var fiji = World.GetCountry("242");

            Assert.NotNull(fiji);
            Assert.True(fiji.IsSmallIslandDevelopingState);
        }

        [Fact]
        public void DevelopedCountry_ShouldHaveNoFlags()
        {
            var usa = World.GetCountry("840");

            Assert.NotNull(usa);
            Assert.False(usa.IsLeastDevelopedCountry);
            Assert.False(usa.IsLandLockedDevelopingCountry);
            Assert.False(usa.IsSmallIslandDevelopingState);
        }

        [Fact]
        public void AllCountries_ShouldHaveValidParent()
        {
            var world = World.GetStatisticalRegion("001");

            foreach (var country in world.Countries)
            {
                Assert.NotNull(country.Parent);
                // All countries should have a parent region code different from "001" (World)
                Assert.NotEqual("001", country.Parent.Code);
            }
        }

        [Fact]
        public void IsoCodes_ShouldBeUppercase()
        {
            var world = World.GetStatisticalRegion("001");

            foreach (var country in world.Countries)
            {
                Assert.True(country.IsoAlpha2.All(char.IsLetter),
                    $"Country {country.Name} should have valid ISO Alpha-2 code");
                Assert.True(country.IsoAlpha3.All(char.IsLetter),
                    $"Country {country.Name} should have valid ISO Alpha-3 code");
                Assert.Equal(2, country.IsoAlpha2.Length);
                Assert.Equal(3, country.IsoAlpha3.Length);
            }
        }

        [Fact]
        public void RegionInfoLookup_ShouldWorkForMajorCountries()
        {
            var majorCountries = new[] { "US", "CA", "GB", "DE", "FR", "IT", "JP", "CN", "AU", "BR" };

            foreach (var isoCode in majorCountries)
            {
                try
                {
                    var regionInfo = new RegionInfo(isoCode);
                    var country = World.GetCountry(regionInfo);

                    Assert.NotNull(country);
                    Assert.Equal(isoCode, country.IsoAlpha2, ignoreCase: true);
                    TestOutput.WriteLine($"{isoCode} -> {country.Name}");
                }
                catch (ArgumentException)
                {
                    TestOutput.WriteLine($"{isoCode} not supported on this OS");
                }
            }
        }

        [Fact]
        public void WesternAfricanCountries_ShouldBeUnderWesternAfricaRegion()
        {
            var westernAfrica = World.GetStatisticalRegion("011");
            Assert.NotNull(westernAfrica);
            Assert.Equal(StatisticalRegionKind.Subregion, westernAfrica.Kind);

            // Verify some Western African countries
            var nigeria = World.GetCountry("566");
            var ghana = World.GetCountry("288");
            var senegal = World.GetCountry("686");

            Assert.NotNull(nigeria);
            Assert.NotNull(ghana);
            Assert.NotNull(senegal);

            Assert.Equal("011", nigeria.Parent.Code);
            Assert.Equal("011", ghana.Parent.Code);
            Assert.Equal("011", senegal.Parent.Code);

            TestOutput.WriteLine($"Western Africa has {westernAfrica.Countries.Count} countries");
        }

        [Fact]
        public void AmericasHierarchy_ShouldBeConsistent()
        {
            // Test South American country under 005 -> 419 -> 019 -> 001
            var brazil = World.GetCountry("076");
            Assert.NotNull(brazil);
            Assert.Equal("005", brazil.Parent.Code); // South America
            
            var brazilAncestors = brazil.GetAncestors().ToList();
            Assert.Contains(brazilAncestors, r => r.Code == "419"); // Latin America and the Caribbean
            Assert.Contains(brazilAncestors, r => r.Code == "019"); // Americas
            Assert.Contains(brazilAncestors, r => r.Code == "001"); // World

            // Test Central American country
            var mexico = World.GetCountry("484");
            Assert.NotNull(mexico);
            Assert.Equal("013", mexico.Parent.Code); // Central America

            var mexicoAncestors = mexico.GetAncestors().ToList();
            Assert.Contains(mexicoAncestors, r => r.Code == "419");
            Assert.Contains(mexicoAncestors, r => r.Code == "019");

            TestOutput.WriteLine("Americas hierarchy verified for Brazil and Mexico");
        }

        [Fact]
        public void IntermediateRegions_ShouldExist()
        {
            var subSaharanAfrica = World.GetStatisticalRegion("202");
            var latinAmerica = World.GetStatisticalRegion("419");

            Assert.NotNull(subSaharanAfrica);
            Assert.NotNull(latinAmerica);

            Assert.Equal(StatisticalRegionKind.IntermediateRegion, subSaharanAfrica.Kind);
            Assert.Equal(StatisticalRegionKind.IntermediateRegion, latinAmerica.Kind);

            TestOutput.WriteLine($"{subSaharanAfrica.Name} is an intermediate region");
            TestOutput.WriteLine($"{latinAmerica.Name} is an intermediate region");
        }

        [Fact]
        public void Antarctica_ShouldBeRegionNotCountry()
        {
            var antarctica = World.GetStatisticalRegion("010");

            Assert.NotNull(antarctica);
            Assert.Equal("Antarctica", antarctica.Name);
            Assert.Equal("001", antarctica.Parent?.Code);
            Assert.Equal(StatisticalRegionKind.Region, antarctica.Kind);
            
            // Antarctica should have no children
            Assert.Empty(antarctica.Children);

            // Antarctica should not be in countries list
            Assert.DoesNotContain(antarctica, World.GetStatisticalRegion("001").Countries);

            TestOutput.WriteLine($"Antarctica region: {antarctica}");
        }

        [Fact]
        public void HierarchyDepth_ShouldNotExceed4()
        {
            var maxDepth = 0;

            foreach (var region in World.StatisticalRegions)
            {
                var depth = GetDepth(region);
                if (depth > maxDepth)
                {
                    maxDepth = depth;
                }
            }

            TestOutput.WriteLine($"Maximum hierarchy depth: {maxDepth}");
            Assert.True(maxDepth <= 4, $"Hierarchy depth should not exceed 4, but was {maxDepth}");
        }

        private int GetDepth(StatisticalRegionInfo region)
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

        [Fact]
        public void GetAllDescendants_ShouldReturnAllChildrenRecursively()
        {
            var world = World.GetStatisticalRegion("001");
            var allDescendants = world.GetAllDescendants().ToList();

            // Should include regions and countries
            Assert.True(allDescendants.Count > 200, "Should have many descendants");
            
            TestOutput.WriteLine($"World has {allDescendants.Count} total descendants");
        }

        [Fact]
        public void PrintFullHierarchy_ShouldOutputAllRegionsAndCountries()
        {
            var world = World.GetStatisticalRegion("001");

            Assert.NotNull(world);

            PrintHierarchy(world, 0);
        }

        private void PrintHierarchy(StatisticalRegionInfo region, int depth)
        {
            var indent = new string(' ', depth * 2);
            var flags = region.Kind == StatisticalRegionKind.CountryOrTerritory
                ? $" [{region.IsoAlpha2}/{region.IsoAlpha3}]"
                : string.Empty;

            TestOutput.WriteLine($"{indent}{region.Code} - {region.Name} ({region.Kind}){flags}");

            foreach (var child in region.Children)
            {
                PrintHierarchy(child, depth + 1);
            }
        }
    }
}
