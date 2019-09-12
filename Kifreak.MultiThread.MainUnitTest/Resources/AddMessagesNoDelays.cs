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
        private bool _isCancelled;
        private readonly List<string> _urls = new List<string>();

        public AddMessagesNoDelays(int id, int numberOfMessages, int messageToCancel, int delayInEachMessage)
        {
            _id = id;
            _numberOfMessages = numberOfMessages;
            _messageToCancel = messageToCancel;
            _delayInEachMessage = delayInEachMessage;
        }

        public bool IsFinish { get; set; }

        public bool IsCompleted()
        {
            return Progress() >= 100;
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
                        IsFinish = true;
                        token.ThrowIfCancellationRequested();
                    }
                }
                _urls.Add($"{_id} Message {i}");
                lock (Shared.MessageList)
                {
                    Shared.MessageList.Add($"{_id} Message {i}");
                }

                // ReSharper disable once MethodSupportsCancellation
                await Task.Delay(_delayInEachMessage);
            }
        }

        public Task ActionAfterComplete()
        {
            return Task.CompletedTask;
        }
    }
}