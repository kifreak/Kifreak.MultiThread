using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kifreak.MultiThread.Shared.Models;

namespace Kifreak.MultiThread.Shared.NewVersion
{
    public interface IMultiple : IDisposable
    {
        void Run(IEnumerable<IMultipleModel> models);

        Task Wait();

        double Progress();

        void CancelTask(ThreadModel taskModel);
    }
}