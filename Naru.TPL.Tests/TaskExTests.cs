using System;
using System.Threading.Tasks;

using NUnit.Framework;

namespace Naru.TPL.Tests
{
    [TestFixture]
    public class TaskExTests
    {
        [Test]
        public void when_an_exception_is_thrown_then_Finally_is_called()
        {
            var testScheduler = new CurrentThreadTaskScheduler();

            var result = false;

            Task.Factory.StartNew(() => { throw new ArgumentNullException(); }, testScheduler)
                .Finally(() => result = true, testScheduler);

            Assert.That(result, Is.True);
        }

        [Test]
        public void when_an_exception_is_not_thrown_then_Finally_is_called()
        {
            var testScheduler = new CurrentThreadTaskScheduler();

            var result = false;

            Task.Factory.StartNew(() => { }, testScheduler)
                .Finally(() => result = true, testScheduler);

            Assert.That(result, Is.True);
        }

        [Test]
        public void when_an_exception_of_T_is_thrown_then_Catch_is_called()
        {
            var testScheduler = new CurrentThreadTaskScheduler();

            var result = false;

            Task.Factory.StartNew(() => { throw new ArgumentNullException(); }, testScheduler)
                .Catch<ArgumentNullException>(ex => result = true);

            Assert.That(result, Is.True);
        }

        [Test]
        public void when_no_exception_of_T_is_thrown_then_Catch_is_not_called()
        {
            var testScheduler = new CurrentThreadTaskScheduler();

            var result = false;

            Task.Factory.StartNew(() => { }, testScheduler)
                .Catch<ArgumentNullException>(ex => result = true);

            Assert.That(result, Is.False);
        }

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