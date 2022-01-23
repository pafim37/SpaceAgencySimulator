using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.OrbitDetermination
{
    public class ObservationController
    {
        //public ObservationController(Observation observation)
        //{
        //    Observation = observation;
        //}
        //public Observation Observation { get; }
        //public double AzimuthRad => Observation.AzimuthRad / 360.0;
        //public double Declination() // OK
        //{
        //    double h = Observation.AltitudeRad * Math.PI / 180;
        //    double fi = Observation.CreatedBy.LatitudeRad * Math.PI / 180;
        //    double sinDec = Math.Sin(h) * Math.Sin(fi) - Math.Cos(h) * Math.Cos(fi) * Math.Cos(Azimuth*2*Math.PI+Math.PI);
        //    return Math.Asin(sinDec);
        //}
        //public double HourAngle()
        //{
        //    double h = Observation.AltitudeRad * Math.PI / 180;
        //    double sint = Math.Sin(AzimuthRad * Math.PI / 180) * Math.Cos(h) / Math.Cos(Declination());
        //    return Math.Asin(sint);
        //} 

        //public double Ra() {
        //    double x = Observation.CreatedBy.GetLocalSiderealTime();
        //    var result = x - HourAngle() / 2 / Math.PI < 0 ? x - HourAngle() / 2 / Math.PI + 1 : x - HourAngle() / 2 / Math.PI;
        //    return result;
        //    }

        //public string ConvertToEquinox()
        //{
        //    var t = Observation.AzimuthRad + 180;
        //    return t.ToString();
        //}

    }
}
