// See https://aka.ms/new-console-template for more information
using Sas.Db.ObservationDatabase.Data;

Console.WriteLine("Hello, World!");

using (var ctx = new ObservationDb())
{
    ctx.Observatories.ToList().ForEach(observator => global::System.Console.WriteLine(observator.Name));

    ctx.Observations.ToList().ForEach(observator => global::System.Console.WriteLine(observator.Observatory.Name));
}
Console.WriteLine("Demo completed.");
