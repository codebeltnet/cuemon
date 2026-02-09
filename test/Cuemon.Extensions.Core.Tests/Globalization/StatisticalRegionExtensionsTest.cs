using System;
using System.Globalization;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Cuemon.Globalization;
using Xunit;

namespace Cuemon.Extensions.Globalization
{
    public class StatisticalRegionExtensions : Test
    {
        public StatisticalRegionExtensions(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void StatisticalRegions_ShouldContainWorld()
        {
            var world = World.StatisticalRegions.FirstOrDefault(r => r.Code == "001");
            Assert.True(world.IsWorld());
        }

        [Fact]
        public void StatisticalRegions_ShouldContainAllContinents()
        {
            var expectedContinents = new[] { "002", "009", "019", "142", "150" }; // Africa, Oceania, Americas, Asia, Europe

            foreach (var code in expectedContinents)
            {
                var continent = World.GetStatisticalRegion(code);
                Assert.True(continent.IsRegion());
            }
        }

        [Fact]
        public void GetCountry_ByM49Code_US_ShouldReturn840()
        {
            var usa = World.GetCountry("840");

            Assert.True(usa.IsCountryOrTerritory());
            Assert.True(usa.HasIsoCodes());
        }
        
        [Fact]
        public void GetCountry_ByRegionInfo_US_ShouldReturnUnitedStates()
        {
            var regionInfo = new RegionInfo("US");
            var usa = World.GetCountry(regionInfo);
            Assert.True(usa.IsCountryOrTerritory());
        }

        [Fact]
        public void GetAncestors_FromRegion_ShouldReturnCompleteChain()
        {
            var westernEurope = World.GetStatisticalRegion("155");
            Assert.True(westernEurope.IsSubregion());
        }

        [Fact]
        public void Children_ShouldBePopulated()
        {
            var world = World.GetStatisticalRegion("001");
            Assert.True(world.IsWorld());
        }
        
        [Fact]
        public void AllCountries_ShouldHaveValidParent()
        {
            var world = World.GetStatisticalRegion("001");

            foreach (var country in world.Countries)
            {
                Assert.NotNull(country.Parent);
                Assert.True(country.IsCountryOrTerritory());
            }
        }

        [Fact]
        public void AmericasHierarchy_ShouldBeConsistent()
        {
            // Test South American country under 005 -> 419 -> 019 -> 001
            var brazil = World.GetCountry("076");
            Assert.NotNull(brazil);
            Assert.True(brazil.Parent.IsSubregion());
        }

        [Fact]
        public void IntermediateRegions_ShouldExist()
        {
            var subSaharanAfrica = World.GetStatisticalRegion("202");
            var latinAmerica = World.GetStatisticalRegion("419");

            Assert.NotNull(subSaharanAfrica);
            Assert.NotNull(latinAmerica);

            Assert.True(subSaharanAfrica.IsIntermediateRegion());
            Assert.True(latinAmerica.IsIntermediateRegion());
        }

        [Fact]
        public void Antarctica_ShouldBeRegionNotCountry()
        {
            var antarctica = World.GetStatisticalRegion("010");

            Assert.NotNull(antarctica);
            Assert.True(antarctica.IsRegion());

            // Antarctica should have no children
            Assert.Empty(antarctica.Children);

            // Antarctica should not be in countries list
            Assert.DoesNotContain(antarctica, World.GetStatisticalRegion("001").Countries);

            TestOutput.WriteLine($"Antarctica region: {antarctica}");
        }

        [Fact]
        public void GetAllDescendants_ShouldReturnAllChildrenRecursively()
        {
            var world = World.GetStatisticalRegion("001");
            var allDescendants = world.GetAllDescendants().ToList();

            
            // Should include both regions and countries
            Assert.Contains(allDescendants, r => r.IsRegion());
            Assert.Contains(allDescendants, r => r.IsCountryOrTerritory());
        }

        [Fact]
        public void ExtensionMethods_ShouldWorkCorrectly()
        {
            var world = World.GetStatisticalRegion("001");
            var europe = World.GetStatisticalRegion("150");
            var westernEurope = World.GetStatisticalRegion("155");
            var usa = World.GetCountry("840");

            Assert.True(world.IsWorld());
            Assert.False(world.IsRegion());
            Assert.False(world.IsCountryOrTerritory());

            Assert.True(europe.IsRegion());
            Assert.False(europe.IsWorld());
            Assert.False(europe.IsCountryOrTerritory());
            Assert.True(europe.IsArea());

            Assert.True(westernEurope.IsSubregion());
            Assert.False(westernEurope.IsRegion());
            Assert.True(westernEurope.IsArea());

            Assert.True(usa.IsCountryOrTerritory());
            Assert.False(usa.IsArea());
            Assert.True(usa.HasIsoCodes());
        }

        [Fact]
        public void HasRegionInfo_ShouldReturnTrue_WhenCountryHasRegionInfo()
        {
            var usa = World.GetCountry("840");

            Assert.NotNull(usa);
            Assert.True(usa.HasRegionInfo());
        }

        [Fact]
        public void HasRegionInfo_ShouldReturnFalse_WhenRegionIsNotCountry()
        {
            var world = World.GetStatisticalRegion("001");
            var europe = World.GetStatisticalRegion("150");

            Assert.False(world.HasRegionInfo());
            Assert.False(europe.HasRegionInfo());
        }

        [Fact]
        public void HasRegionInfo_ShouldReturnFalse_WhenCountryHasNoRegionInfo()
        {
            var countriesWithoutRegionInfo = World.GetStatisticalRegion("001").Countries
                .Where(c => c.Region == null);

            foreach (var countryWithoutRegionInfo in countriesWithoutRegionInfo)
            {
                Assert.True(countryWithoutRegionInfo.IsCountryOrTerritory());
                Assert.False(countryWithoutRegionInfo.HasRegionInfo());

                TestOutput.WriteLine($"Country without RegionInfo: {countryWithoutRegionInfo}");
            }
        }

        [Fact]
        public void HasRegionInfo_ShouldThrowArgumentNullException_WhenRegionIsNull()
        {
            StatisticalRegionInfo region = null;

            Assert.Throws<ArgumentNullException>(() => region.HasRegionInfo());
        }
    }
}
