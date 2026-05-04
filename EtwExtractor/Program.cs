using EtwExtractor.Options;
using EtwExtractor.Filter;
using EtwExtractor.EventListener;

var etwTracerOptions = new EtwOptionbuilder()
    .EnableKernelNetworkTCPIP()
    .EnableKernelContextSwitch()
    .AddApplication("chrome")
    .AddApplication("Discord")
    .Build();

var etwFilter = new EtwFilter(etwTracerOptions);

// create the mapper 

// create the writer


using (var kernelTracer = new KernelEventListener(etwTracerOptions, etwFilter))
{
    Console.WriteLine("Started...");
    kernelTracer.Start();
    await Task.Delay(5000);

    Console.WriteLine("Stopping...");
    kernelTracer.Stop();
    Console.WriteLine("Stopped.");
}
