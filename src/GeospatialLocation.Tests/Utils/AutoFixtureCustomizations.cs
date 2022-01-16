using AutoFixture;
using AutoFixture.AutoMoq;

namespace GeospatialLocation.Tests.Utils
{
    public class AutoFixtureCustomizations : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new AutoMoqCustomization());
        }
    }
}