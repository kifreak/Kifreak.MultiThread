using System.Threading;
using System.Threading.Tasks;
using Kifreak.MultiThread.Shared.NewVersion;

namespace Kifreak.MultiThread.Shared.Models
{
    public class ThreadModel
    {
        public CancellationTokenSource Token { get; set; }
        public Task Task { get; set; }

        public IMultipleModel Model { get; set; }
    }
}