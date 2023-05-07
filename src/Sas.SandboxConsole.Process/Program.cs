using Sas.Domain.Models;
using Sas.Domain.Models.Bodies;
using Sas.Domain.Models.Orbits;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;

namespace Sas.SandboxConsole.Process
{
    internal class Program
    {
        static void Main(string[] args)
        {

            BodySystem solarSystem = new();
            Body Sun = new Body("Sun", Constants.SolarMass, Vector.Zero, Vector.Zero);
            Vector earthPosition = new(Constants.EarthPeriapsis, 0, 0);
            Vector earthVelocity = new(0, Constants.EarthMaxVelocity, 0);
            Body Earth = new Body("Earth", Constants.EarthMass, earthPosition, earthVelocity);
            solarSystem.AddBody(Sun);
            solarSystem.AddBody(Earth);
            double u = solarSystem.U;
            Orbit orbit = OrbitFactory.CalculateOrbit(earthPosition, earthVelocity, u);
            Console.WriteLine(orbit.Eccentricity);


            //SolarSystem solarSystem = new SolarSystem();
            //Body body1 = new Body("B1", 1000, new Vector(1, 0, 0), new Vector(0, 1, 0));
            //Body body2 = new Body("B2", 1000, new Vector(-1, 0, 0), new Vector(0, -1, 0));
            //solarSystem.AddBody(body1);
            //solarSystem.AddBody(body2);
            //Orbit orbit = OrbitFactory.CalculateOrbit(body1.AbsolutePosition, body1.AbsoluteVelocity, solarSystem.U);
            //Console.WriteLine(orbit.Eccentricity);


        }
    }
}