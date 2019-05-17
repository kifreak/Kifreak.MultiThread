using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kifreak.MultiThread.Shared.NewVersion;

namespace Kifreak.MultiThread.MainUnitTest.Resources
{
    public class AddMessagesNoDelays : IMultipleModel
    {
        private readonly int _id;
        private readonly int _numberOfMessages;
        private readonly int _messageToCancel;
        private readonly int _delayInEachMessage;
        private bool _isCompleted;
        private bool _isCancelled;
        private readonly List<string> _urls = new List<string>();

        public AddMessagesNoDelays(int id, int numberOfMessages, int messageToCancel, int delayInEachMessage)
        {
            _id = id;
            _numberOfMessages = numberOfMessages;
            _messageToCancel = messageToCancel;
            _delayInEachMessage = delayInEachMessage;
        }

        public bool IsCompleted()
        {
            return _isCompleted;
        }

        public bool IsCanceled()
        {
            return _isCancelled;
        }

        public double Progress()
        {
            return _urls.Count / (double)_numberOfMessages;
        }

        public async Task ActionToExecute(CancellationToken token)
        {
            for (var i = 0; i < _numberOfMessages; i++)
            {
                if (i == _messageToCancel)
                {
                    if (token.IsCancellationRequested)
                    {
                        _isCancelled = true;
                        token.ThrowIfCancellationRequested();
                    }
                }
                _urls.Add($"{_id} Message {i}");
                lock (Shared.UrlList)
                {
                    Shared.UrlList.Add($"{_id} Message {i}");
                }

                // ReSharper disable once MethodSupportsCancellation
                await Task.Delay(_delayInEachMessage);
            }

            _isCompleted = true;
        }
    }
}