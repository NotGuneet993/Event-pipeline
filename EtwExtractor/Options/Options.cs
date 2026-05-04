using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Diagnostics.Tracing.Parsers;

namespace EtwExtractor.Options
{
    public class Options
    {
        readonly KernelTraceEventParser.Keywords KernelOptions = 
            KernelTraceEventParser.Keywords.NetworkTCPIP |
            KernelTraceEventParser.Keywords.FileIO |
            KernelTraceEventParser.Keywords.DiskIO;

           
    }
}
