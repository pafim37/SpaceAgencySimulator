// See https://aka.ms/new-console-template for more information
using Sas.Domain;

Console.WriteLine("Hello, World!");

DateTime LocalGreenwichDateTime = new DateTime(2013, 8, 21, 18, 0, 0); // In Greenwich
DateTime LocalCracowDateTime = new DateTime(2013, 8, 21, 19, 0, 0); // In Cracow
DateTime LocalDateTime = new DateTime(2013, 8, 21, 13, 0, 0); // In New York

Observatory Cracow = new Observatory("Kraków", ChangeDegToRad(50.06143), ChangeDegToRad(19.93658));
Observatory Greenwich = new Observatory("Greenwich", ChangeDegToRad(51.47781), ChangeDegToRad(0.00148));
Observatory NewYork = new Observatory("NewYork", ChangeDegToRad(42.41753), ChangeDegToRad(-76.49407));

Greenwich.SetLocalTime(LocalGreenwichDateTime, 0);
Cracow.SetLocalTime(LocalCracowDateTime, 1);
NewYork.SetLocalTime(LocalDateTime, -5);

var glst = Greenwich.GetLocalSiderealTime();
var clst = Cracow.GetLocalSiderealTime();
var nyst = NewYork.GetLocalSiderealTime();

Console.WriteLine(glst);
Console.WriteLine(clst);
Console.WriteLine(nyst);

RadarObservation MoonObserv = Cracow.CreateObservation("Moon", ChangeDegToRad(102.15467), ChangeDegToRad(3.343), 367273905); // [deg, deg, m]

// DateTime LocalCracowDateTime2 = new DateTime(2013, 8, 22, 4, 00, 00); // In Cracow
Cracow.SetLocalTime(LocalCracowDateTime, 1);

var srt = Cracow.GetLocalSiderealTime();
double az = MoonObserv.AzimuthRad;
Console.Write("azimuth: ");
Console.WriteLine(az);
Console.WriteLine(DisplayAsDeg(az));

double dec = MoonObserv.DeclinationRad;
Console.Write("dec: ");
Console.WriteLine(DisplayAsDeg(dec));

Console.Write("srt: ");
Console.WriteLine(DisplayAsDeg(srt));

double t = MoonObserv.HourAngleRad;
Console.Write("t: ");
Console.WriteLine(t);
Console.WriteLine(DisplayAsDeg(t));

double ra = MoonObserv.RightAscensionRad;
Console.Write("ra: ");
Console.WriteLine(DisplayAsDeg(ra));

DateTime d1 = new DateTime(2000,1,1,18,46,31);
DateTime d2 = new DateTime(2000,1,1,22,34,38);
Console.WriteLine();
Console.Write(18+22+1-24);
Console.Write(" ");
Console.Write(46+34+1-60);
Console.Write(" ");
Console.Write(31+38-60);
Console.WriteLine();
string DisplayAsHours(double a)
{
    double dh = 24 * a;
    int h = (int)dh;
    double dm = (dh - h) * 60;
    int m = (int)dm;
    double ds = (dm - m) * 60;
    int s = (int)ds;
    return $"{h}h {m}m {s}s";

}

string DisplayAsDeg(double a)
{
    a = ChangeRadToDeg(a);
    int h = (int)a;
    double dm = (a - h) * 60;
    int m = (int)dm;
    double ds = (dm - m) * 60;
    int s = (int)ds;
    return $"{h}s {m}\' {s}\"";
}
double ChangeRadToDeg(double rad)
{
    return 180 * rad / Math.PI;
}

double ChangeDegToRad(double deg)
{
    return Math.PI * deg / 180;
}