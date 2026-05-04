using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using static System.Net.WebRequestMethods;

namespace EtwExtractor.Writer
{
    public interface IWriter<T>
    {
        bool Write(T obj);
    }
}


