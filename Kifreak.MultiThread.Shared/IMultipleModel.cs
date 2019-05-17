using System.Threading;
using System.Threading.Tasks;

namespace Kifreak.MultiThread.Shared.NewVersion
{
    public interface IMultipleModel
    {
        bool IsCompleted();

        bool IsCanceled();

        double Progress();

        Task ActionToExecute(CancellationToken token);
    }
}