using System;
using System.Collections.Generic;
using System.Text;

namespace EtwExtractor.Mapper
{
    public interface IMapper <T, U>
    {
        public U Convert(T input);
    }
}
