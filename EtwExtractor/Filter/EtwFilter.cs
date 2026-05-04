using EtwExtractor.Options;
using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;
using System.Text;

namespace EtwExtractor.Filter
{
    public class EtwFilter : IFilter<TraceEvent>
    {
        private readonly EtwOptions options;

        public EtwFilter(EtwOptions option)
        {
            options = option;
        }

        public bool ShouldKeep(TraceEvent evt)
        {
            return ValidateName(evt);
        }

        private bool ValidateName(TraceEvent evt)
        {
            return options.CheckForApplicationName(evt.ProcessName);
        }
    }
}
