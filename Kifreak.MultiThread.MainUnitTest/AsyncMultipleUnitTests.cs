using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Factory;
using Kifreak.MultiThread.MainUnitTest.Resources;
using Kifreak.MultiThread.Shared.NewVersion;
using Xunit;

namespace Kifreak.MultiThread.MainUnitTest
{
    public class AsyncMultipleUnitTests
    {
        [Fact]
        public async Task CreateMessagesOk()
        {
            int timeToWait = 100;
            Resources.Shared.MessageList = new List<string>();
            IMultiple multiple = GetMultiple();
            IMultipleModel[] models = {
                new AddMessageModel("Message 1", timeToWait),
                new AddMessageModel("Message 2", timeToWait * 2),
                new AddMessageModel("Message 3", timeToWait * 3),
                new AddMessageModel("Message 4", timeToWait * 4),
                new AddMessageModel("Message 5", timeToWait * 5),
            };
            multiple.Run(models);
            await multiple.Wait();
            Assert.Equal(models.Length, Resources.Shared.MessageList.Count);
            Assert.Equal(models.Length, Resources.Shared.MessageList.GroupBy(t => t).Count()); //Must be Different messages
            Assert.Equal(multiple.TaskList.Count,multiple.TaskList.Count(t => t.Task.IsCompleted)); //All task must end with completed status
        }

        [Fact]
        public async Task CreateMessageCancelAll()
        {
            Resources.Shared.MessageList = new List<string>();
            int timeToWait = 1000;
            Resources.Shared.MessageList = new List<string>();
            IMultiple multiple = GetMultiple();
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
            Assert.Empty(Resources.Shared.MessageList);
            Assert.Equal(multiple.TaskList.Count, multiple.TaskList.Count(t => t.Token.IsCancellationRequested));
        }

        [Fact]
        public async Task CreateMessageWithSameTimeOk()
        {
            int timeToWait = 1000;
            Resources.Shared.MessageList = new List<string>();
            IMultiple multiple = GetMultiple();
            IMultipleModel[] models = {
                new AddMessageModel("Message 1", timeToWait),
                new AddMessageModel("Message 2", timeToWait),
                new AddMessageModel("Message 3", timeToWait),
                new AddMessageModel("Message 4", timeToWait),
                new AddMessageModel("Message 5", timeToWait),
            };
            multiple.Run(models);
            await multiple.Wait();
            Assert.Equal(models.Length, Resources.Shared.MessageList.Count);
            Assert.Equal(models.Length, Resources.Shared.MessageList.GroupBy(t => t).Count()); //Must be Different
        }

        [Fact]
        public async Task CreateMessagesAndCancelNoDelay()
        {
            int messagesToCreate = 50000;
            int messageToCancel = 1000;
            int delay = 1;
            Resources.Shared.MessageList = new List<string>();
            IMultiple multiple = GetMultiple();
            IMultipleModel[] multipleModelsList = {
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
            Assert.Equal(messageToCancel * multipleModelsList.Length, Resources.Shared.MessageList.Count);
            Assert.Equal((Resources.Shared.MessageList.Count / (double)(messagesToCreate * multipleModelsList.Length)), multiple.Progress());
        }

        [Fact]
        public async Task CreateMessageWithThrow()
        {
            int timeToWait = 1000;
            Resources.Shared.MessageList = new List<string>();
            IMultiple multiple = GetMultiple();
            IMultipleModel[] model = {
                new AddMessageModelException("Message 1", timeToWait),
                new AddMessageModelException("Message 2", timeToWait * 2),
                new AddMessageModelException("Message 3", timeToWait * 3),
                new AddMessageModelException("Message 4", timeToWait * 4),
                new AddMessageModelException("Message 5", timeToWait * 5),
            }
            ;
            multiple.Run(model);
            await multiple.Wait();
            Assert.Empty(Resources.Shared.MessageList);
        }

        private IMultiple GetMultiple()
        {
            return new MultipleBase(new MultiTaskFactory());
        }
        

    }




}