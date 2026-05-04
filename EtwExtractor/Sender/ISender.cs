using System;
using System.Collections.Generic;
using System.Text;

namespace EtwExtractor.Sender
{
    public interface ISender<T>
    {
        bool Send(T input);
    }
}
