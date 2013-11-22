using System;
using System.Threading.Tasks;

using NUnit.Framework;

namespace Naru.TPL.Tests
{
    [TestFixture]
    public class TaskExTests
    {
        [Test]
        public void Retry_return_value_of_a_previously_failing_task_is_propogated_through()
        {
            var currentThreadScheduler = new CurrentThreadTaskScheduler();

            var exceptionOccured = false;

            var triggerToNotThrowException = 2;
            var index = 0;

            Func<Task<int>> taskProvider = () => Task.Factory.StartNew(() =>
                                                                       {
                                                                           index++;
                                                                           if (index <= triggerToNotThrowException)
                                                                           {
                                                                               throw new Exception("Problem");
                                                                           }

                                                                           return index;
                                                                       }, currentThreadScheduler);

            taskProvider()
                .Retry(taskProvider, triggerToNotThrowException + 1, _ => true, currentThreadScheduler)
                .CatchAndHandle(ex => exceptionOccured = true, currentThreadScheduler);

            Assert.That(exceptionOccured, Is.False);
            Assert.That(index, Is.EqualTo(triggerToNotThrowException + 1));
        }

        [Test]
        public void Retry_exception_is_propogated_through()
        {
            var currentThreadScheduler = new CurrentThreadTaskScheduler();

            var exceptionOccured = false;

            Func<Task<int>> taskProvider = () => Task.Factory.StartNew<int>(() =>
                                                                            {
                                                                                throw new Exception("Problem");
                                                                            }, currentThreadScheduler);

            taskProvider()
                .Retry(taskProvider, 3, _ => true, currentThreadScheduler)
                .CatchAndHandle(ex => exceptionOccured = true, currentThreadScheduler);

            Assert.That(exceptionOccured, Is.True);
        }
    }
}