using FluentAssertions;
using Sas.Mathematica.Service.Converters;

namespace Sas.Mathematica.XUnit.Tests
{
    public class ConvertAngleTest
    {
        [InlineData(0)]
        [InlineData(90)]
        [InlineData(180)]
        [InlineData(270)]
        [InlineData(360)]
        [InlineData(720)]
        [Theory]
        public void DoubleConvertionShouldReturnsInitialInput(double deg)
        {
            // Act
            var rad = ConvertAngle.DegToRad(deg);
            var result = ConvertAngle.RadToDeg(rad);     

            // Assert
            result.Should().Be(deg);

        }
    }
}