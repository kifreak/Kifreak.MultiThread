using System;

namespace Kifreak.MultiThread.Shared
{
    public class MultiModel<T>
    {
        public T ObjectToSend { get; set; }
        public Action<T> ActionToExecute { get; set; }
    }
}
