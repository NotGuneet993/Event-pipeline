using Microsoft.Diagnostics.Tracing.Parsers;

namespace EtwExtractor.Options
{
    public class EtwOptions
    {

        private KernelTraceEventParser.Keywords KernelOptions;
        private HashSet<String> ApplicationNames;

        public EtwOptions()
        {
            KernelOptions = KernelTraceEventParser.Keywords.None;
            ApplicationNames = new HashSet<String>();
        }

        public void AddKernelOption(KernelTraceEventParser.Keywords option)
        {
            KernelOptions |= option;
        }

        public KernelTraceEventParser.Keywords GetKernelOptions()
        {
            return KernelOptions;
        }

        public void AddApplicationName(String appName)
        {
            ApplicationNames.Add(appName);
        }

        public bool CheckForApplicationName(String appName)
        {
            return ApplicationNames.Contains(appName);
        }
    }
}
