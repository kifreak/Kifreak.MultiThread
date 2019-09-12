using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kifreak.MultiThread.Shared.Models;

namespace Kifreak.MultiThread.Shared.NewVersion
{
    public interface IMultiple : IDisposable
    {

        List<ThreadModel> TaskList { get; }

        void Run(IEnumerable<IMultipleModel> models);

        Task Wait();

        double Progress();

        void CancelTask(ThreadModel taskModel);

    }
}