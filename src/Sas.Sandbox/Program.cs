using Sas.SolarSystem;
using Spectre.Console;

SolarSystem solarSystem = new SolarSystem();
solarSystem.CreateDefaultSolarSystem();
solarSystem.FindAndAssignAttracted();
solarSystem.AssignOribit();

foreach (var body in solarSystem.GetBodies())
{
    Console.WriteLine(body.Orbit);
}


var earth = solarSystem.GetBodies().Where(b => b.Name.Equals("Earth")).FirstOrDefault();
Console.WriteLine(earth);
Console.WriteLine($"eo: {earth.Orbit.Eccentricity}");

var table = new Table();

// add columns
table.AddColumn("Name");
foreach (var item in solarSystem.GetBodies())
{
    table.AddColumn(item.Name);
}

// add rows
for (int row = 0; row < solarSystem.GetBodies().Count; row++)
{
    string row1 = solarSystem.GetBodies()[row].Name;
    for (int col = 0; col < solarSystem.GetBodies().Count; col++)
    {
        //    string row2 = (SolarSystem[i].AbsolutePosition - SolarSystem[0].AbsolutePosition).Magnitude().ToString();
        //    string row3 = (SolarSystem[i].AbsolutePosition - SolarSystem[1].AbsolutePosition).Magnitude().ToString();
        //    string row4 = (SolarSystem[i].AbsolutePosition - SolarSystem[2].AbsolutePosition).Magnitude().ToString();
        //    string row5 = (SolarSystem[i].AbsolutePosition - SolarSystem[3].AbsolutePosition).Magnitude().ToString();

    }
    table.AddRow(row1);
}
//{
//    string row1 = SolarSystem[i].Name;
//    string row2 = (SolarSystem[i].AbsolutePosition - SolarSystem[0].AbsolutePosition).Magnitude().ToString();
//    string row3 = (SolarSystem[i].AbsolutePosition - SolarSystem[1].AbsolutePosition).Magnitude().ToString();
//    string row4 = (SolarSystem[i].AbsolutePosition - SolarSystem[2].AbsolutePosition).Magnitude().ToString();
//    string row5 = (SolarSystem[i].AbsolutePosition - SolarSystem[3].AbsolutePosition).Magnitude().ToString();

//}

AnsiConsole.Write(table);