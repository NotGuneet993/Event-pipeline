using Microsoft.Diagnostics.Tracing.Parsers;

namespace EtwExtractor.Options
{
    public class EtwOptionbuilder
    {
        private EtwOptions options = new EtwOptions();

        public EtwOptionbuilder EnableKernelDiskFileIO()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.DiskFileIO);
            return this;
        }

        public EtwOptionbuilder EnableKernelDiskIO()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.DiskIO);
            return this;
        }

        public EtwOptionbuilder EnableKernelImageLoad()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.ImageLoad);
            return this;
        }

        public EtwOptionbuilder EnableKernelMemoryHardFaults()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.MemoryHardFaults);
            return this;
        }
            
        public EtwOptionbuilder EnableKernelNetworkTCPIP()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.NetworkTCPIP);
            return this;
        }

        public EtwOptionbuilder EnableKernelProcess()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.Process);
            return this;
        }

        public EtwOptionbuilder EnableKernelProcessCounters()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.ProcessCounters);
            return this;
        }

        public EtwOptionbuilder EnableKernelProfile()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.Profile);
            return this;
        }

        public EtwOptionbuilder EnableKernelThread()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.Thread);
            return this;
        }

        public EtwOptionbuilder EnableKernelContextSwitch()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.ContextSwitch);
            return this;
        }

        public EtwOptionbuilder EnableKernelDiskIOInit()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.DiskIOInit);
            return this;
        }

        public EtwOptionbuilder EnableKernelDispatcher()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.Dispatcher);
            return this;
        }

        public EtwOptionbuilder EnableKernelFileIO()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.FileIO);
            return this;
        }

        public EtwOptionbuilder EnableKernelFileIOInit()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.FileIOInit);
            return this;
        }

        public EtwOptionbuilder EnableKernelMemoryPageFaults()
        {
            // up to date src code has this as 'MemoryPageFaults' (as of 5/4/2026)
            options.AddKernelOption(KernelTraceEventParser.Keywords.Memory);
            return this;
        }

        public EtwOptionbuilder EnableKernelRegistry()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.Registry);
            return this;
        }

        public EtwOptionbuilder EnableKernelSystemCall()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.SystemCall);
            return this;
        }

        public EtwOptionbuilder EnableKernelVirtualAlloc()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.VirtualAlloc);
            return this;
        }

        public EtwOptionbuilder EnableKernelAdvancedLocalProcedureCalls()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.AdvancedLocalProcedureCalls);
            return this;
        }

        public EtwOptionbuilder EnableKernelDeferredProcedureCalls()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.DeferedProcedureCalls);
            return this;
        }

        public EtwOptionbuilder EnableKernelDriver()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.Driver);
            return this;
        }

        public EtwOptionbuilder EnableKernelInterrupt()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.Interrupt);
            return this;
        }

        public EtwOptionbuilder EnableKernelSplitIO()
        {
            options.AddKernelOption(KernelTraceEventParser.Keywords.SplitIO);
            return this;
        }

        public EtwOptionbuilder AddApplication(string applicationName)
        {
            options.AddApplicationName(applicationName);
            return this;
        }

        public EtwOptions Build()
        {
            return options;
        }
    }
}
