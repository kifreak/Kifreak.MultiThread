using System.Collections.Generic;
using System.Threading.Tasks;
using Kifreak.MultiThread.MainUnitTest.Resources;
using Kifreak.MultiThread.Shared.NewVersion;
using Xunit;

namespace Kifreak.MultiThread.MainUnitTest
{
    public class AsyncMultipleUnitTests
    {
        [Fact]
        public async Task CancelTaskBeforeCreateMessages()
        {
            int timeToWait = 1000;
            Resources.Shared.UrlList = new List<string>();
            IMultiple multiple = new Multiple();
            multiple.Run(new IMultipleModel[]
            {
                new AddMessageModel("Message 1",timeToWait),
                new AddMessageModel("Message 2",timeToWait*2),
                new AddMessageModel("Message 3",timeToWait*3),
                new AddMessageModel("Message 4",timeToWait*4),
                new AddMessageModel("Message 5",timeToWait*5),
            });
            await Task.Delay(timeToWait / 2);
            multiple.Dispose();
            await Task.Delay(timeToWait * 6);
            Assert.Empty(Resources.Shared.UrlList);
        }

        [Fact]
        public async Task CreateMessagesOk()
        {
            int timeToWait = 1000;
            Resources.Shared.UrlList = new List<string>();
            IMultiple multiple = new Multiple();
            multiple.Run(new IMultipleModel[]
            {
                new AddMessageModel("Message 1",timeToWait),
                new AddMessageModel("Message 2",timeToWait*2),
                new AddMessageModel("Message 3",timeToWait*3),
                new AddMessageModel("Message 4",timeToWait*4),
                new AddMessageModel("Message 5",timeToWait*5),
            });
            await multiple.Wait();
        }

        [Fact]
        public async Task CreateMessageWithSameTimeOk()
        {
            int timeToWait = 1000;
            Resources.Shared.UrlList = new List<string>();
            IMultiple multiple = new Multiple();
            multiple.Run(new IMultipleModel[]
            {
                new AddMessageModel("Message 1",timeToWait),
                new AddMessageModel("Message 2",timeToWait),
                new AddMessageModel("Message 3",timeToWait),
                new AddMessageModel("Message 4",timeToWait),
                new AddMessageModel("Message 5",timeToWait),
            });
            await multiple.Wait();
        }

        [Fact]
        public async Task CreateMessagesAndCancelNoDelay()
        {
            int messagesToCreate = 50000;
            int messageToCancel = 1000;
            int delay = 1;
            Resources.Shared.UrlList = new List<string>();
            IMultiple multiple = new Multiple();
            var multipleModelsList = new IMultipleModel[]
            {
                new AddMessagesNoDelays(1, messagesToCreate, messageToCancel,delay),
                new AddMessagesNoDelays(2, messagesToCreate, messageToCancel,delay),
                new AddMessagesNoDelays(3, messagesToCreate, messageToCancel,delay),
                new AddMessagesNoDelays(4, messagesToCreate, messageToCancel,delay),
                new AddMessagesNoDelays(5, messagesToCreate, messageToCancel,delay),
                new AddMessagesNoDelays(6, messagesToCreate, messageToCancel,delay),
            };
            multiple.Run(multipleModelsList);
            await Task.Delay(1000);
            multiple.Dispose();
            await multiple.Wait();
            Assert.Equal(messageToCancel * multipleModelsList.Length, Resources.Shared.UrlList.Count);
            Assert.Equal((Resources.Shared.UrlList.Count / (double)(messagesToCreate * multipleModelsList.Length)), multiple.Progress());
        }
    }
}