using EtwExtractor.Filter;
using EtwExtractor.Mapper;
using EtwExtractor.Options;
using EtwExtractor.Writer;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;

namespace EtwExtractor.EventListener
{
    public class KernelEventListener : IEventListener
    {
        private TraceEventSession session;
        private bool runningState;

        private Thread? hotPathThread;
        private EtwOptions options;
        private IFilter<TraceEvent> etwFilter;
        private IMapper<TraceEvent, RawEventStruct> converter;
        private IWriter<RawEventStruct> writer;

        public KernelEventListener(EtwOptions options, IFilter<TraceEvent> etwFilter, IMapper<TraceEvent, RawEventStruct> converter, IWriter<RawEventStruct> writer) 
        {
            if (options == null) throw new ArgumentNullException("options");
            if (etwFilter == null) throw new ArgumentNullException("filter");
            if (converter == null) throw new ArgumentNullException("converter");
            if (writer == null) throw new ArgumentNullException("writer");

            this.options = options;
            this.etwFilter = etwFilter;
            this.converter = converter;
            this.writer = writer;

            runningState = false;
            session = new TraceEventSession("KernelTracingSession");
            session.EnableKernelProvider(this.options.GetKernelOptions());
            session.Source.Kernel.All += ProcessTraceEvent;
        }

        public void Start()
        {
            if (runningState)
            {
                return;
            }

            hotPathThread = new Thread(() => session.Source.Process())
            {
                IsBackground = true,
                Name = "ETW-Kernel-Hot-Path-Thread"
            };

            runningState = true;
            hotPathThread.Start();
        }

        public void Stop()
        {
            if (!runningState)
            {
                return;
            }

            session.Source.StopProcessing();
        }

        public void Dispose()
        {
            session.Dispose();
            GC.SuppressFinalize(this);
        }

        private void ProcessTraceEvent(TraceEvent evt)
        {
            if (!etwFilter.ShouldKeep(evt))
            {
                return;
            }

            Console.WriteLine($"Kept: {evt.ProcessName}\t{evt.EventName}");

            var structuredEvt = converter.Convert(evt);
            writer.Write(ref structuredEvt);
        }
    }
}
