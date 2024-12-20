using FluentAssertions;
using Sas.Mathematica.Service.Converters;

namespace Sas.Mathematica.Tests
{
    public class ConvertAngleTest
    {
        [Theory]
        [InlineData(90)]
        [InlineData(180)]
        [InlineData(270)]
        [InlineData(360)]
        [InlineData(720)]
        public void DoubleConvertionShouldReturnsInitialInput(double deg)
        {
            // Act
            double rad = ConvertAngle.DegToRad(deg);
            double result = ConvertAngle.RadToDeg(rad);

            // Assert
            result.Should().Be(deg);

        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(90, Math.PI / 2)]
        [InlineData(180, Math.PI)]
        [InlineData(360, 2 * Math.PI)]
        public void ConvertAngleDegToRad(double angle, double result)
        {
            ConvertAngle.DegToRad(angle).Should().Be(result);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(Math.PI / 2, 90)]
        [InlineData(Math.PI, 180)]
        [InlineData(2 * Math.PI, 360)]
        public void ConvertAngleRadToDeg(double angle, double result)
        {
            ConvertAngle.RadToDeg(angle).Should().Be(result);
        }
    }
}