using AutoFixture;
using AutoFixture.Xunit2;

namespace GeospatialLocation.Tests.Utils
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(() => new Fixture().Customize(new AutoFixtureCustomizations()))
        {
        }
    }
}