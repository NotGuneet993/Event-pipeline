using EtwExtractor.Filter;
using EtwExtractor.Mapper;
using EtwExtractor.Options;
using EtwExtractor.Writer;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;

namespace EtwExtractor.EventListener
{
    public class KernelEventListener : IEventListener, IDisposable
    {
        private TraceEventSession session;
        private bool runningState;

        private Thread? hotPathThread;
        private EtwOptions options;
        private IFilter<TraceEvent> etwFilter;
        private IMapper<TraceEvent, RawEventStruct> converter;
        //private IWriter abc

        public KernelEventListener(EtwOptions options, IFilter<TraceEvent> etwFilter, IMapper<TraceEvent, RawEventStruct> converter) 
        {
            if (options == null) throw new ArgumentNullException("options");
            if (etwFilter == null) throw new ArgumentNullException("filter");

            this.options = options;
            this.etwFilter = etwFilter;
            this.converter = converter;

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

            var structuredEvt = converter.Convert(evt);
            // write the item 

            Console.WriteLine($"{evt.EventName}\t{evt.TimeStamp}\t{evt.ProcessName}\t{evt.ProcessID}");
        }
    }
}
