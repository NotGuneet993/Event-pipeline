using System;
using System.Collections.Generic;
using System.Text;

namespace EtwExtractor.Reader
{
    public interface IReader <T> : IDisposable
    {
        IAsyncEnumerable<IReadOnlyList<T>> ReadAsync(CancellationToken ct = default);
    }
}
