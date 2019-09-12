﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiTaskFactory.cs" company="Kifreak">
//  Copyright (c) Kifreak. All rights reserved.
// </copyright>
// <summary>
//   Defines the MultiTaskFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
        public Task ExecuteTask(IMultipleModel model, CancellationToken token)
        {
            return Task.Run(async () =>
                {
                    await model.ActionToExecute(token);
                    //taskAction.Wait(token);
                }, token)
                .ContinueWith((task) =>
                {
                    model.ActionAfterComplete();
                    model.IsFinish = true;
                    return task;

                }, token, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.FromCurrentSynchronizationContext());

        }
    }
}