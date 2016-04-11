using System;

namespace BadExample.Service.Models
{
    public sealed class ResponseObject<T>
    {
        public bool HasError => this.Error != null;
        public Exception Error { get; internal set; }
        public T Value { get; internal set; }
    }
}
