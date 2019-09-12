// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiTaskFactory.cs" company="Kifreak">
//  Copyright (c) Kifreak. All rights reserved.
// </copyright>
// <summary>
//   Defines the MultiTaskFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Threading;
using System.Threading.Tasks;
using Kifreak.MultiThread.Shared.NewVersion;

namespace Factory
{
    /// <summary>
    /// The MultiTaskFactory class
    /// </summary>
    public class MultiTaskFactory
    {
        public Task ExecuteTask(IMultipleModel model, Action<Task> completeTask, CancellationToken token)
        {
            return Task.Run(async () =>
                {
                    await model.ActionToExecute(token);
                    //taskAction.Wait(token);
                }, token)
                .ContinueWith((task) =>
                {
                    completeTask.Invoke(task);
                    model.IsFinish = true;
                    return task;

                }, token, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.FromCurrentSynchronizationContext());

        }
    }
}