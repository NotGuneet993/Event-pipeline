using System;
using System.Collections.Generic;
using System.Text;

namespace EtwExtractor.EventListener
{
    public interface IEventListener : IDisposable
    {
        public void Start();
        public void Stop();
    }
}
