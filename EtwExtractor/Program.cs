using System;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;
using Microsoft.Diagnostics.Tracing.Parsers;

using EtwExtractor.Options;

var EtwTracerOptions = new EtwOptionbuilder()
    .EnableKernelNetworkTCPIP()
    .EnableKernelContextSwitch()
    .AddApplication("chrome")
    .Build();

using (var session = new TraceEventSession("KernelProcessSession"))
{
    session.EnableKernelProvider(EtwTracerOptions.GetKernelOptions());
    session.Source.Kernel.All += (data) =>
    {
        if (EtwTracerOptions.CheckForApplicationName(data.ProcessName))
        {
            Console.WriteLine($"{data.EventName}\t{data.FormattedMessage}\t{data.KernelTime}\t{data.Level}\t{data.ProcessID}\t{data.ProcessName}\t{data.ProviderGuid}\t{data.ProviderName}\t");
        }
    };

    session.Source.Process();
}