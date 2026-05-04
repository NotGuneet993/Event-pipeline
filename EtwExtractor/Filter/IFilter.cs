using System;
using System.Collections.Generic;
using System.Text;

namespace EtwExtractor.Filter
{
    public interface IFilter<T>
    {
        public bool ShouldKeep(T evt);
    }
}
