using System;
using System.Collections.Generic;
using System.Text;

namespace EtwExtractor.EventListener
{
    public interface IEventListener
    {
        public void Start();
        public void Stop();
        public void Dispose();
    }
}
