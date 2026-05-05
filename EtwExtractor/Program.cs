using EtwExtractor.Options;
using EtwExtractor.Filter;
using EtwExtractor.EventListener;
using EtwExtractor.Mapper;
using EtwExtractor.Writer;

var etwTracerOptions = new EtwOptionbuilder()
    .EnableKernelNetworkTCPIP()
    .EnableKernelContextSwitch()
    .AddApplication("chrome")
    .AddApplication("Discord")
    .Build();

var etwFilter = new EtwFilter(etwTracerOptions);
var mapper = new EventToStructMapper();
var writer = new SqliteWalWriter();
var kernelTracer = new KernelEventListener(etwTracerOptions, etwFilter, mapper, writer)


Console.WriteLine("Started...");
kernelTracer.Start();
await Task.Delay(5000);

Console.WriteLine("Stopping...");
kernelTracer.Stop();
Console.WriteLine("Stopped.");

// dispose
writer.Dispose();
kernelTracer.Dispose();