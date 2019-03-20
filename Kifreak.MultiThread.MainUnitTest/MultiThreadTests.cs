using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kifreak.MultiThread.Shared;
using Xunit;

namespace Kifreak.MultiThread.MainUnitTest
{
    public class MultiThreadTests
    {
        private readonly List<string> _results = new List<string>();

        [Fact]
        public async Task CreateMessagesInMultiThreadsOk()
        {
            void Action(string s) => _results.Add(s);

            List<MultiModel<string>> multiModels = new List<MultiModel<string>>
            {
                new MultiModel<string>{ObjectToSend = "Message1",ActionToExecute = Action},
                new MultiModel<string>{ObjectToSend = "Message2",ActionToExecute = Action},
                new MultiModel<string>{ObjectToSend = "Message3",ActionToExecute = Action},
                new MultiModel<string>{ObjectToSend = "Message4",ActionToExecute = Action},
                new MultiModel<string>{ObjectToSend = "Message5",ActionToExecute = Action},
            };
            MultiThreads<string> multiThreads = MultiThreads<string>.Init();
            await multiThreads.RunAsync(multiModels.ToArray());
            Assert.Equal(_results.Count, multiModels.Count);
        }

        [Fact]
        public async Task CreateMessagesInMultiThreadsKo()
        {
            void Action(string s) => throw new Exception("Not work");

            List<MultiModel<string>> multiModels = new List<MultiModel<string>>
            {
                new MultiModel<string>{ObjectToSend = "Message1",ActionToExecute = Action},
                new MultiModel<string>{ObjectToSend = "Message2",ActionToExecute = Action},
                new MultiModel<string>{ObjectToSend = "Message3",ActionToExecute = Action},
                new MultiModel<string>{ObjectToSend = "Message4",ActionToExecute = Action},
                new MultiModel<string>{ObjectToSend = "Message5",ActionToExecute = Action},
            };
            MultiThreads<string> multiThreads = MultiThreads<string>.Init();
            await multiThreads.RunAsync(multiModels.ToArray());
            Assert.Equal(0, _results.Count);
        }
    }
}
