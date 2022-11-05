using Xunit;

namespace Sas.Domain.Tests
{
    public class AstronomicalTimeTest
    {
        [Fact]
        public void AstronomicalClockReturnsSiderealTimeForKiruna()
        {
            var longitude = 0.35284797888793762757793179159304;
            DateTime localtime = new DateTime(2012, 2, 13, 2, 30, 0);

            AstronomicalClock astronomicalClock = new(localtime, longitude);

            Assert.Equal(3.4952, astronomicalClock.SiderealTime, 4);
        }

        [Fact]
        public void AstronomicalClockReturnsSiderealTimeForGreenwich()
        {
            var longitude = 0;
            DateTime localtime = new DateTime(2009, 4, 19, 23, 0, 0);
            localtime.ToUniversalTime();
            localtime.ToLocalTime();
            AstronomicalClock astronomicalClock = new(localtime, longitude);
            Assert.Equal(3.3715, astronomicalClock.SiderealTime, 4);


        }
        
        [Fact]
        public void AstronomicalClockReturnsSiderealTime3()
        {
            // Londyn
            var longitude = 0;
            DateTime localtime = new DateTime(2022, 11, 5, 0, 0, 0);

            AstronomicalClock astronomicalClock = new(localtime, longitude);

            Assert.Equal(0.7716, astronomicalClock.SiderealTime, 4);

        }
        
        [Fact]
        public void AstronomicalClockReturnsSiderealTime4()
        {
            // Krakow
            var longitude = 0.2618;
            DateTime localtime = new DateTime(2022, 11, 5, 1, 0, 0);

            AstronomicalClock astronomicalClock = new(localtime, longitude);

            Assert.Equal(1.2959, astronomicalClock.SiderealTime, 4);

        }
    }
}
