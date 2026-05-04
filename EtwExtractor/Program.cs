using System;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;
using Microsoft.Diagnostics.Tracing.Parsers;

using EtwExtractor.Options;
using EtwExtractor.Filter;

var etwTracerOptions = new EtwOptionbuilder()
    .EnableKernelNetworkTCPIP()
    .EnableKernelContextSwitch()
    .AddApplication("Discord")
    .Build();

var etwFilter = new EtwFilter(etwTracerOptions);

using (var session = new TraceEventSession("KernelProcessSession"))
{

    session.EnableKernelProvider(etwTracerOptions.GetKernelOptions());
    session.Source.Kernel.All += (data) =>
    {
        if (etwFilter.ShouldKeep(data))
        {
            Console.WriteLine($"{data.EventName}\t{data.FormattedMessage}\t{data.KernelTime}\t{data.Level}\t{data.ProcessID}\t{data.ProcessName}\t{data.ProviderGuid}\t{data.ProviderName}\t");
        }
    };

    session.Source.Process();
}