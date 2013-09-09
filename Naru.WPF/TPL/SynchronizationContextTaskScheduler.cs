using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Naru.WPF.TPL
{
    /// <summary>
    /// Provides a task scheduler that targets a specific SynchronizationContext.
    /// </summary>
    public sealed class SynchronizationContextTaskScheduler : DispatcherTaskSchedulerBase
    {
        /// <summary>
        /// The queue of tasks to execute, maintained for debugging purposes.
        /// </summary>
        private readonly ConcurrentQueue<Task> _tasks;

        /// <summary>
        /// The target context under which to execute the queued tasks.
        /// </summary>
        private readonly SynchronizationContext _context;

        public override void ExecuteSync(Action action)
        {
            if (_context == SynchronizationContext.Current)
            {
                action();
                return;
            }

            var taskCompletionSource = new TaskCompletionSource<object>();

            _context.Post(delegate
            {
                action();
                taskCompletionSource.TrySetResult(null);
            }, null);

            taskCompletionSource.Task.Wait();
        }

        /// <summary>
        /// Initializes an instance of the SynchronizationContextTaskScheduler class
        /// with the specified SynchronizationContext.
        /// </summary>
        /// <param name="context">The SynchronizationContext under which to execute tasks.</param>
        public SynchronizationContextTaskScheduler(SynchronizationContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            _context = context;
            _tasks = new ConcurrentQueue<Task>();
        }

        /// <summary>
        /// Queues a task to the scheduler for execution on the I/O ThreadPool.
        /// </summary>
        /// <param name="task">The Task to queue.</param>
        protected override void QueueTask(Task task)
        {
            _tasks.Enqueue(task);
            _context.Post(delegate
            {
                Task nextTask;
                if (_tasks.TryDequeue(out nextTask))
                {
                    TryExecuteTask(nextTask);
                }
            }, null);
        }

        /// <summary>
        /// Tries to execute a task on the current thread.
        /// </summary>
        /// <param name="task">The task to be executed.</param>
        /// <param name="taskWasPreviouslyQueued">Ignored.</param>
        /// <returns>Whether the task could be executed.</returns>
        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return _context == SynchronizationContext.Current && TryExecuteTask(task);
        }

        /// <summary>
        /// Gets an enumerable of tasks queued to the scheduler.
        /// </summary>
        /// <returns>An enumerable of tasks queued to the scheduler.</returns>
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return _tasks.ToArray();
        }

        /// <summary>
        /// Gets the maximum concurrency level supported by this scheduler.
        /// </summary>
        public override int MaximumConcurrencyLevel { get { return 1; } }
    }
}