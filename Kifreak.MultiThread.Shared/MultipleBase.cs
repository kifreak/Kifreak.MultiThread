using Factory;
using Kifreak.MultiThread.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kifreak.MultiThread.Shared.NewVersion
{
    public class MultipleBase : IMultiple
    {
        private readonly MultiTaskFactory _taskFactory;
        public List<ThreadModel> TaskList { get; }

        public MultipleBase(MultiTaskFactory factory)
        {
            _taskFactory = factory;
            TaskList = new List<ThreadModel>();
        }

        public void Run(IEnumerable<IMultipleModel> threadModels)
        {
            TaskList.AddRange(threadModels.Select(CreateNewTask));
        }

        public async Task Wait()
        {
            while (true)
            {
                if (TaskList.Count(t => t.Model.IsFinish) == TaskList.Count)
                {
                    break;
                }
                await Task.Delay(100);
            }
        }

        public virtual double Progress()
        {
            return TaskList.Sum(t => t.Model.Progress()) / TaskList.Count;
        }

        public void CancelTask(ThreadModel taskModel)
        {
            taskModel.Token.Cancel();
        }

        public void Dispose()
        {
            TaskList.ForEach(CancelTask);
        }

        #region Private Methods

        private ThreadModel CreateNewTask(IMultipleModel model)
        {
            CancellationTokenSource token = new CancellationTokenSource();
            return new ThreadModel
            {
                Model = model,
                Task = _taskFactory.ExecuteTask(model,token.Token),
                Token = token
            };
        }

        #endregion Private Methods
    }
}