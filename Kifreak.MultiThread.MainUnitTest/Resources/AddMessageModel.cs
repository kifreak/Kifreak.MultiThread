using System.Threading;
using System.Threading.Tasks;
using Kifreak.MultiThread.Shared.NewVersion;

namespace Kifreak.MultiThread.MainUnitTest.Resources
{
    public class AddMessageModel : IMultipleModel
    {
        private readonly string _message;
        private readonly int _milliseconds;

        public AddMessageModel(string message, int milliseconds)
        {
            _message = message;
            _milliseconds = milliseconds;
        }

        public bool IsCompleted()
        {
            return Shared.UrlList.Contains(_message);
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

            lock (Shared.UrlList)
            {
                Shared.UrlList.Add(_message);
            }
        }
    }
}