using System.Threading;
using System.Threading.Tasks;

namespace Kifreak.MultiThread.Shared.NewVersion
{
    public interface IMultipleModel
    {

        bool IsFinish { get; set; }
        bool IsCompleted();

        bool IsCanceled();

        double Progress();

        object Response { get; }

        Task ActionToExecute(CancellationToken token);

        Task ActionAfterComplete();
    }
}