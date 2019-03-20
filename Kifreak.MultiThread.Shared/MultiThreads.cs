using System;
using System.Threading;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace Kifreak.MultiThread.Shared
{
    public class MultiThreads<T>
    {
        private readonly SynchronizationContext _context;
        private int _count;
        private int _completedCount;
        public double Progress { get; private set; }

        public event EventHandler Completed;

        public MultiThreads() : this(SynchronizationContext.Current)
        {
        }

        public MultiThreads(SynchronizationContext context)
        {
            Progress = 0;
            _context = context ?? new SynchronizationContext();
        }

        public static MultiThreads<T> Init()
        {
            return new MultiThreads<T>();
        }

        public async Task RunAsync(MultiModel<T>[] multiModelArray)
        {
            _count = multiModelArray.Length;
            foreach (MultiModel<T> multiModel in multiModelArray)
            {
                ThreadPool.QueueUserWorkItem(ThreadFunc, multiModel);
            }

            await WaitToFinishAsync();
        }

        public async Task WaitToFinishAsync()
        {
            while (Progress < 100)
            {
                await Task.Delay(1000);
            }
        }

        private void ThreadFunc(object threadContext)
        {
            try
            {
                MultiModel<T> model = (MultiModel<T>) threadContext;
                Thread.Sleep(1000);

                model.ActionToExecute.Invoke(model.ObjectToSend);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (Interlocked.Increment(ref _completedCount) >= _count)
            {
                _context.Post(OnCompleted, null);
            }

            AssignProgress();
        }

        protected virtual void OnCompleted(object state)
        {
            EventHandler handler = Completed;
            handler?.Invoke(this, EventArgs.Empty);
        }

        private void AssignProgress()
        {
            Progress = (_completedCount / (double) _count) * 100;
        }
    }
}
