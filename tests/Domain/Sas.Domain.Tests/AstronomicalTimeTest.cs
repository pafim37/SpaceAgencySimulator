using Sas.Domain.Models.Clocks;
using Sas.Mathematica.Service.Converters;
using Xunit;

namespace Sas.Domain.Tests
{
    public class AstronomicalTimeTest
    {

        public static IEnumerable<object[]> AstronomicalClockDataTest =>
            new List<object[]>
            {
                new object[] { 0.35284797888793762757793179159304,  new DateTime(2012, 2, 13, 2, 30, 0), 3.4952 },
                new object[] { 0, new DateTime(2022, 11, 5, 0, 0, 0), 0.7716 },
                new object[] { 0.2618, new DateTime(2022, 11, 5, 1, 0, 0), 1.2959 },
                //new object[] { 0.1171,  new DateTime(2009, 4, 19, 23, 0, 0), 3.4886 },
                //new object[]
                //{
                //    ConvertNumberToAngle.ToRads(ConvertAngleToNumber.GetDeg(19, 58, 53.4f)),
                //    new DateTime(2009, 4, 19, 23, 0, 0),
                //    ConvertNumberToAngle.ToRads(ConvertAngleToNumber.GetHour(13, 12, 27.1f))
                //},
                new object[]
                {
                    ConvertAngle.DegToRad(20.2167f),
                    new DateTime(2012, 2, 13, 2, 30, 0),
                    ConvertAngle.DegToRad(200.26241475864445f)
                }
            };

        [Theory]
        [MemberData(nameof(AstronomicalClockDataTest))]
        public void AstronomicalClockReturnsSiderealTime(double longitude, DateTime localtime, double expectedSiderealTime)
        {
            // Arange
            AstronomicalClock astronomicalClock = new(localtime, longitude);

            // Act
            double actualSiderealTime = astronomicalClock.SiderealTime;

            // Assert
            Assert.Equal(expectedSiderealTime, actualSiderealTime, 4);
        }
    }
}
