
using Sas.Mathematica;
using Sas.Sandbox.Models;
using Spectre.Console;
using System.Collections.Generic;

Body Sun = new Body.Builder()
    .Name("Sun")
    .Mass(Constants.SolarMass)
    .AbsolutePosition(new Vector(0, 0, 0))
    .AbsoluteVelocity(new Vector(0, 0, 0))
    .Build();

Body Earth = new Body.Builder()
    .Name("Earth")
    .Mass(Constants.EarthMass)
    .AbsolutePosition(new Vector( 147098291000, 0, 0 ))
    .AbsoluteVelocity(new Vector( 0, 29291, 0 ))
    .Build();

Body Moon = new Body.Builder()
    .Name("Moon")
    .Mass(Constants.MoonMass)
    .AbsolutePosition(new Vector(147098291000 - 363104, 0, 0))
    .AbsoluteVelocity(new Vector(0, 968, 0))
    .Build();

Body Probe = new Body.Builder()
    .Name("Probe")
    .Mass(100)
    .AbsolutePosition(new Vector(147098291000 - 363104 - 10000000000, 0, 0))
    .AbsoluteVelocity(new Vector(0, 968, 0))
    .Build();

Sun.Attracted = Sun;
Earth.Attracted = Sun;
Moon.Attracted = Earth;

IList<Body> bodies = new List<Body>();

bodies.Add(Sun);
bodies.Add(Moon);
bodies.Add(Earth);
bodies.Add(Probe);

FindAttracted(Probe, bodies);

Console.WriteLine(Probe.Attracted.Name);

void FindAttracted(Body body, IList<Body> bodies)
{
    // find closest
    body.Attracted = Sun;
    var closestList = bodies.OrderBy(x => (body.AbsolutePosition - x.AbsolutePosition).Magnitude()).ToList();
    for (int i = 1; i < closestList.Count - 1; i++)
    {
        var closest = closestList[i];
        CoordinateSystem cs = new CoordinateSystem(closest.AbsolutePosition);
        cs.Cartesian(body.AbsolutePosition);
        if (closest.GetSphereOfInfluence(cs.Phi) > (body.AbsolutePosition - closest.AbsolutePosition).Magnitude())
        {
            body.Attracted = closest;
            break;
        }
    }
}


var table = new Table();

// add columns
table.AddColumn("Name");
foreach (var item in bodies)
{
    table.AddColumn(item.Name);
}

// add rows
for (int i = 0; i < bodies.Count; i++)
{
    string row1 = bodies[i].Name;
    string row2 = (bodies[i].AbsolutePosition - bodies[0].AbsolutePosition).Magnitude().ToString();
    string row3 = (bodies[i].AbsolutePosition - bodies[1].AbsolutePosition).Magnitude().ToString();
    string row4 = (bodies[i].AbsolutePosition - bodies[2].AbsolutePosition).Magnitude().ToString();
    string row5 = (bodies[i].AbsolutePosition - bodies[3].AbsolutePosition).Magnitude().ToString();

    table.AddRow(row1, row2, row3, row4, row5 );
}

AnsiConsole.Write(table);