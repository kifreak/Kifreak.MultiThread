using System;
using System.Threading;
using System.Threading.Tasks;
using Kifreak.MultiThread.Shared.NewVersion;

namespace Kifreak.MultiThread.MainUnitTest.Resources
{
    public class AddMessageModelException : IMultipleModel
    {
        private readonly string _message;
        private readonly int _milliseconds;

        public AddMessageModelException(string message, int milliseconds)
        {
            _message = message;
            _milliseconds = milliseconds;
        }

        public bool IsFinish { get; set; }

        public object Response { get; private set; }
        public bool IsCompleted()
        {
            return Shared.MessageList.Contains(_message);
        }

        public bool IsCanceled()
        {
            return false;
        }

        public double Progress()
        {
            return IsCompleted() ? 100 : 0;
        }
        
        public async Task ActionToExecute(CancellationToken token)
        {
            await Task.Delay(_milliseconds, token);
            Response = new Exception("Fail inserted message");
            throw (Exception)Response;
        }

        public Task ActionAfterComplete()
        {
            return Task.CompletedTask;
        }
    }
}