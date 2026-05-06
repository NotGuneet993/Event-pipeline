using EtwExtractor.Options;
using EtwExtractor.Filter;
using EtwExtractor.EventListener;
using EtwExtractor.Mapper;
using EtwExtractor.Writer;
using EtwExtractor.Reader;
using EtwExtractor.Sender;

var etwTracerOptions = new EtwOptionbuilder()
    .EnableKernelNetworkTCPIP()
    .AddApplication("chrome")
    .AddApplication("Discord")
    .Build();

var etwFilter = new EtwFilter(etwTracerOptions);
var mapper = new EventToStructMapper();
var writer = new SqliteWalWriter();
var kernelTracer = new KernelEventListener(etwTracerOptions, etwFilter, mapper, writer);

var sqlReader = new SqliteWalReader();
var sender = new KafkaSender(sqlReader);
var senderThread = new Thread(() => sender.RunAsync().GetAwaiter().GetResult())
{
    IsBackground = true,
    Name = "ETW-Sender-Thread"
};

Console.WriteLine("Started...");
senderThread.Start();
kernelTracer.Start();

await Task.Delay(Timeout.Infinite);
