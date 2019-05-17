using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kifreak.MultiThread.Shared.Models;

namespace Kifreak.MultiThread.Shared.NewVersion
{
    public class Multiple : IMultiple
    {
        #region Internal State

        private readonly List<ThreadModel> _taskList = new List<ThreadModel>();

        #endregion Internal State

        #region Public Methods (IMultiple and IDisposable)

        public void Run(IEnumerable<IMultipleModel> models)
        {
            foreach (var multiModel in models)
            {
                _taskList.Add(CreateNewTask(multiModel));
            }
        }

        public async Task Wait()
        {
            while (!AllCompletedOrCanceled())
            {
                await Task.Delay(100);
            }
        }

        public double Progress()
        {
            var sum = _taskList.Sum(t => t.Model.Progress());
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return sum == 0 ? 0 : sum / _taskList.Count;
        }

        public void CancelTask(ThreadModel taskModel)
        {
            taskModel.Token.Cancel();
        }

        public void Dispose()
        {
            foreach (ThreadModel task in _taskList)
            {
                CancelTask(task);
            }
        }

        #endregion Public Methods (IMultiple and IDisposable)

        #region Private Methods

        private ThreadModel CreateNewTask(IMultipleModel multiModel)
        {
            CancellationTokenSource token = new CancellationTokenSource();
            return new ThreadModel
            {
                Model = multiModel,
                Task = Task.Run(() => { multiModel.ActionToExecute(token.Token); }, token.Token),
                Token = token
            };
        }

        private bool AllCompletedOrCanceled()
        {
            foreach (var task in _taskList)
            {
                if (!task.Model.IsCompleted() && !task.Model.IsCanceled() && task.Task.Status != TaskStatus.Canceled)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion Private Methods
    }
}