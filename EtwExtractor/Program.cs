using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;
using Microsoft.Diagnostics.Tracing.Parsers;

//var DotPids = Process.GetProcesses()
//    .Where(p => p.ProcessName.Contains("Dot", StringComparison.OrdinalIgnoreCase))
//    .Select(p => p.Id)
//    .ToHashSet();

//Console.WriteLine($"Found {DotPids.Count} Dot process(es): {string.Join(", ", DotPids)}");

using var session = new TraceEventSession("DotEtwSession");

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    session.Stop();
};

session.Source.Dynamic.All += evt =>
{
    //if (!DotPids.Contains(evt.ProcessID)) return;

    Console.WriteLine($"[{evt.TimeStamp:HH:mm:ss.fff}] PID={evt.ProcessID,-6} Provider={evt.ProviderName,-40} Event={evt.EventName}");
    evt.EventName
    evt.FormattedMessage
    evt.KernelTime
    evt.Level
    evt.ProcessID
    evt.ProcessName
    evt.ProviderGuid
    evt.ProviderName

};

session.EnableProvider(
    new Guid("DAF0B914-9C1C-450A-81B2-FEA7244F6FFA"),
    TraceEventLevel.Verbose);

Console.WriteLine("Listening for ETW events... Press Ctrl+C to stop.\n");
session.Source.Process();