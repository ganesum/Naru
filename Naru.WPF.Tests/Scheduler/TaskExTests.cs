using System;
using System.Threading;
using System.Threading.Tasks;

using Naru.TPL;

using NUnit.Framework;

namespace Naru.WPF.Tests.Scheduler
{
    [TestFixture]
    public class TaskExTests
    {
        [Test]
        public void catch_happens_before_finally_with_TaskScheduler()
        {
            var autoResetEvent = new AutoResetEvent(false);

            var task1HasRun = false;
            var task2HasRun = false;
            var task3HasRun = false;

            var scheduler = TaskScheduler.Default;

            new TaskFactory(scheduler)
                .StartNew(() =>
                          {
                              Assert.That(task1HasRun, Is.False);
                              Assert.That(task2HasRun, Is.False);
                              Assert.That(task3HasRun, Is.False);
                              task1HasRun = true;

                              throw new Exception();
                          })
                .CatchAndHandle(_ =>
                                {
                                    Assert.That(task1HasRun, Is.True);
                                    Assert.That(task2HasRun, Is.False);
                                    Assert.That(task3HasRun, Is.False);
                                    task2HasRun = true;
                                }, scheduler)
                .Finally(() =>
                         {
                             Assert.That(task1HasRun, Is.True);
                             Assert.That(task2HasRun, Is.True);
                             Assert.That(task3HasRun, Is.False);
                             task3HasRun = true;

                             autoResetEvent.Set();
                         }, scheduler);

            autoResetEvent.WaitOne();

            Assert.That(task1HasRun, Is.True);
            Assert.That(task2HasRun, Is.True);
            Assert.That(task3HasRun, Is.True);
        }

        [Test]
        public void catch_happens_before_finally_with_CurrentThreadScheduler()
        {
            var task1HasRun = false;
            var task2HasRun = false;
            var task3HasRun = false;

            var scheduler = new CurrentThreadTaskScheduler();

            new TaskFactory(scheduler)
                .StartNew(() =>
                          {
                              Assert.That(task1HasRun, Is.False);
                              Assert.That(task2HasRun, Is.False);
                              Assert.That(task3HasRun, Is.False);
                              task1HasRun = true;

                              throw new Exception();
                          })
                .CatchAndHandle(_ =>
                                {
                                    Assert.That(task1HasRun, Is.True);
                                    Assert.That(task2HasRun, Is.False);
                                    Assert.That(task3HasRun, Is.False);
                                    task2HasRun = true;
                                }, scheduler)
                .Finally(() =>
                         {
                             Assert.That(task1HasRun, Is.True);
                             Assert.That(task2HasRun, Is.True);
                             Assert.That(task3HasRun, Is.False);
                             task3HasRun = true;
                         }, scheduler);

            Assert.That(task1HasRun, Is.True);
            Assert.That(task2HasRun, Is.True);
            Assert.That(task3HasRun, Is.True);
        }

        [Test]
        public void catch_happens_before_finally_with_two_schedulers()
        {
            var autoResetEvent = new AutoResetEvent(false);

            var task1HasRun = false;
            var task2HasRun = false;
            var task3HasRun = false;

            var taskScheduler = TaskScheduler.Default;
            var currentTaskScheduler = new CurrentThreadTaskScheduler();

            new TaskFactory(taskScheduler)
                .StartNew(() =>
                          {
                              Assert.That(task1HasRun, Is.False);
                              Assert.That(task2HasRun, Is.False);
                              Assert.That(task3HasRun, Is.False);
                              task1HasRun = true;

                              throw new Exception();
                          })
                .CatchAndHandle(_ =>
                                {
                                    Assert.That(task1HasRun, Is.True);
                                    Assert.That(task2HasRun, Is.False);
                                    Assert.That(task3HasRun, Is.False);
                                    task2HasRun = true;
                                }, taskScheduler)
                .Finally(() =>
                         {
                             Assert.That(task1HasRun, Is.True);
                             Assert.That(task2HasRun, Is.True);
                             Assert.That(task3HasRun, Is.False);
                             task3HasRun = true;

                             autoResetEvent.Set();
                         }, currentTaskScheduler);

            autoResetEvent.WaitOne();

            Assert.That(task1HasRun, Is.True);
            Assert.That(task2HasRun, Is.True);
            Assert.That(task3HasRun, Is.True);
        }

        [Test]
        public void when_SelectMany_and_two_schedulers_then_they_are_called_in_order_overload1()
        {
            var autoResetEvent = new AutoResetEvent(false);

            var task1HasRun = false;
            var task2HasRun = false;

            var taskScheduler = TaskScheduler.Default;
            var currentThreadTaskScheduler = new CurrentThreadTaskScheduler();

            new TaskFactory(taskScheduler)
                .StartNew(() =>
                          {
                              Assert.That(task1HasRun, Is.False);

                              task1HasRun = true;

                              return 0;
                          }, taskScheduler)
                .Then(_ =>
                      {
                          Assert.That(task1HasRun, Is.True);
                          Assert.That(task2HasRun, Is.False);

                          task2HasRun = true;

                          return CompletedTask<int>.Default;
                      }, currentThreadTaskScheduler)
                .Finally(() => autoResetEvent.Set(), taskScheduler);

            autoResetEvent.WaitOne();

            Assert.That(task1HasRun, Is.True);
            Assert.That(task2HasRun, Is.True);
        }

        [Test]
        public void when_SelectMany_and_two_schedulers_then_they_are_called_in_order_overload2()
        {
            var autoResetEvent = new AutoResetEvent(false);

            var task1HasRun = false;
            var task2HasRun = false;

            var taskScheduler = TaskScheduler.Default;
            var currentThreadTaskScheduler = new CurrentThreadTaskScheduler();

            new TaskFactory(taskScheduler)
                .StartNew(() =>
                          {
                              Assert.That(task1HasRun, Is.False);

                              task1HasRun = true;

                              return 0;
                          }, taskScheduler)
                .Then(() =>
                      {
                          Assert.That(task1HasRun, Is.True);
                          Assert.That(task2HasRun, Is.False);

                          task2HasRun = true;

                          return CompletedTask<int>.Default;
                      }, currentThreadTaskScheduler)
                .Finally(() => autoResetEvent.Set(), taskScheduler);

            autoResetEvent.WaitOne();

            Assert.That(task1HasRun, Is.True);
            Assert.That(task2HasRun, Is.True);
        }

        [Test]
        public void when_SelectMany_and_two_schedulers_then_they_are_called_in_order_overload3()
        {
            var autoResetEvent = new AutoResetEvent(false);

            var task1HasRun = false;
            var task2HasRun = false;

            var taskScheduler = TaskScheduler.Default;
            var currentThreadTaskScheduler = new CurrentThreadTaskScheduler();

            new TaskFactory(taskScheduler)
                .StartNew(() =>
                          {
                              Assert.That(task1HasRun, Is.False);

                              task1HasRun = true;

                              return 0;
                          }, taskScheduler)
                .Then(_ =>
                      {
                          Assert.That(task1HasRun, Is.True);
                          Assert.That(task2HasRun, Is.False);

                          task2HasRun = true;

                          return CompletedTask.Default;
                      }, currentThreadTaskScheduler)
                .Finally(() => autoResetEvent.Set(), taskScheduler);

            autoResetEvent.WaitOne();

            Assert.That(task1HasRun, Is.True);
            Assert.That(task2HasRun, Is.True);
        }

        [Test]
        public void when_SelectMany_and_two_schedulers_then_they_are_called_in_order_overload4()
        {
            var autoResetEvent = new AutoResetEvent(false);

            var task1HasRun = false;
            var task2HasRun = false;

            var taskScheduler = TaskScheduler.Default;
            var currentThreadTaskScheduler = new CurrentThreadTaskScheduler();

            new TaskFactory(taskScheduler)
                .StartNew(() =>
                          {
                              Assert.That(task1HasRun, Is.False);

                              task1HasRun = true;

                              return 0;
                          }, taskScheduler)
                .Do(_ =>
                    {
                        Assert.That(task1HasRun, Is.True);
                        Assert.That(task2HasRun, Is.False);

                        task2HasRun = true;
                    }, currentThreadTaskScheduler)
                .Finally(() => autoResetEvent.Set(), taskScheduler);

            autoResetEvent.WaitOne();

            Assert.That(task1HasRun, Is.True);
            Assert.That(task2HasRun, Is.True);
        }

        [Test]
        public void when_SelectMany_and_two_schedulers_then_they_are_called_in_order_overload5()
        {
            var autoResetEvent = new AutoResetEvent(false);

            var task1HasRun = false;
            var task2HasRun = false;

            var taskScheduler = TaskScheduler.Default;
            var currentThreadTaskScheduler = new CurrentThreadTaskScheduler();

            new TaskFactory(taskScheduler)
                .StartNew(() =>
                          {
                              Assert.That(task1HasRun, Is.False);

                              task1HasRun = true;
                          }, taskScheduler)
                .Do(() =>
                    {
                        Assert.That(task1HasRun, Is.True);
                        Assert.That(task2HasRun, Is.False);

                        task2HasRun = true;
                    }, currentThreadTaskScheduler)
                .Finally(() => autoResetEvent.Set(), taskScheduler);

            autoResetEvent.WaitOne();

            Assert.That(task1HasRun, Is.True);
            Assert.That(task2HasRun, Is.True);
        }

        [Test]
        public void when_SelectMany_and_two_schedulers_then_they_are_called_in_order_overload6()
        {
            var autoResetEvent = new AutoResetEvent(false);

            var task1HasRun = false;
            var task2HasRun = false;

            var taskScheduler = TaskScheduler.Default;
            var currentThreadTaskScheduler = new CurrentThreadTaskScheduler();

            new TaskFactory(taskScheduler)
                .StartNew(() =>
                          {
                              Assert.That(task1HasRun, Is.False);

                              task1HasRun = true;
                          }, taskScheduler)
                .Then(() =>
                      {
                          Assert.That(task1HasRun, Is.True);
                          Assert.That(task2HasRun, Is.False);

                          task2HasRun = true;

                          return CompletedTask.Default;
                      }, currentThreadTaskScheduler)
                .Finally(() => autoResetEvent.Set(), taskScheduler);

            autoResetEvent.WaitOne();

            Assert.That(task1HasRun, Is.True);
            Assert.That(task2HasRun, Is.True);
        }

        [Test]
        public void when_SelectMany_and_exception_is_thrown_in_previous_task_then_select_many_is_not_called_overload1()
        {
            var autoResetEvent = new AutoResetEvent(false);

            var task1HasRun = false;
            var task2HasRun = false;

            var taskScheduler = TaskScheduler.Default;

            new TaskFactory(taskScheduler)
                .StartNew<int>(() =>
                               {
                                   Assert.That(task1HasRun, Is.False);

                                   task1HasRun = true;

                                   throw new Exception();
                               })
                .Then(_ =>
                      {
                          Assert.That(task2HasRun, Is.False);

                          task2HasRun = true;

                          return CompletedTask<int>.Default;
                      }, taskScheduler)
                .Finally(() => autoResetEvent.Set(), taskScheduler);

            autoResetEvent.WaitOne();

            Assert.That(task1HasRun, Is.True);
            Assert.That(task2HasRun, Is.False);
        }

        [Test]
        public void when_SelectMany_and_exception_is_thrown_in_previous_task_then_select_many_is_not_called_overload2()
        {
            var autoResetEvent = new AutoResetEvent(false);

            var task1HasRun = false;
            var task2HasRun = false;

            var taskScheduler = TaskScheduler.Default;

            new TaskFactory(taskScheduler)
                .StartNew<int>(() =>
                               {
                                   Assert.That(task1HasRun, Is.False);

                                   task1HasRun = true;

                                   throw new Exception();
                               })
                .Then(() =>
                      {
                          Assert.That(task2HasRun, Is.False);

                          task2HasRun = true;

                          return CompletedTask<int>.Default;
                      }, taskScheduler)
                .Finally(() => autoResetEvent.Set(), taskScheduler);

            autoResetEvent.WaitOne();

            Assert.That(task1HasRun, Is.True);
            Assert.That(task2HasRun, Is.False);
        }

        [Test]
        public void when_SelectMany_and_exception_is_thrown_in_previous_task_then_select_many_is_not_called_overload3()
        {
            var autoResetEvent = new AutoResetEvent(false);

            var task1HasRun = false;
            var task2HasRun = false;

            var taskScheduler = TaskScheduler.Default;

            new TaskFactory(taskScheduler)
                .StartNew(() =>
                          {
                              Assert.That(task1HasRun, Is.False);

                              task1HasRun = true;

                              throw new Exception();
                          })
                .Then(() =>
                      {
                          Assert.That(task2HasRun, Is.False);

                          task2HasRun = true;

                          return CompletedTask<int>.Default;
                      }, taskScheduler)
                .Finally(() => autoResetEvent.Set(), taskScheduler);

            autoResetEvent.WaitOne();

            Assert.That(task1HasRun, Is.True);
            Assert.That(task2HasRun, Is.False);
        }

        [Test]
        public void when_SelectMany_and_exception_is_thrown_in_previous_task_then_select_many_is_not_called_overload4()
        {
            var autoResetEvent = new AutoResetEvent(false);

            var task1HasRun = false;
            var task2HasRun = false;

            var taskScheduler = TaskScheduler.Default;

            new TaskFactory(taskScheduler)
                .StartNew<int>(() =>
                               {
                                   Assert.That(task1HasRun, Is.False);

                                   task1HasRun = true;

                                   throw new Exception();
                               })
                .Then(_ =>
                      {
                          Assert.That(task2HasRun, Is.False);

                          task2HasRun = true;

                          return CompletedTask.Default;
                      }, taskScheduler)
                .Finally(() => autoResetEvent.Set(), taskScheduler);

            autoResetEvent.WaitOne();

            Assert.That(task1HasRun, Is.True);
            Assert.That(task2HasRun, Is.False);
        }

        [Test]
        public void when_SelectMany_and_exception_is_thrown_in_previous_task_then_select_many_is_not_called_overload5()
        {
            var autoResetEvent = new AutoResetEvent(false);

            var task1HasRun = false;
            var task2HasRun = false;

            var taskScheduler = TaskScheduler.Default;

            new TaskFactory(taskScheduler)
                .StartNew<int>(() =>
                               {
                                   Assert.That(task1HasRun, Is.False);

                                   task1HasRun = true;

                                   throw new Exception();
                               })
                .Do(_ =>
                    {
                        Assert.That(task2HasRun, Is.False);

                        task2HasRun = true;
                    }, taskScheduler)
                .Finally(() => autoResetEvent.Set(), taskScheduler);

            autoResetEvent.WaitOne();

            Assert.That(task1HasRun, Is.True);
            Assert.That(task2HasRun, Is.False);
        }

        [Test]
        public void when_SelectMany_and_exception_is_thrown_in_previous_task_then_select_many_is_not_called_overload6()
        {
            var autoResetEvent = new AutoResetEvent(false);

            var task1HasRun = false;
            var task2HasRun = false;

            var taskScheduler = TaskScheduler.Default;

            new TaskFactory(taskScheduler)
                .StartNew(() =>
                          {
                              Assert.That(task1HasRun, Is.False);

                              task1HasRun = true;

                              throw new Exception();
                          })
                .Then(() =>
                      {
                          Assert.That(task2HasRun, Is.False);

                          task2HasRun = true;

                          return CompletedTask.Default;
                      }, taskScheduler)
                .Finally(() => autoResetEvent.Set(), taskScheduler);

            autoResetEvent.WaitOne();

            Assert.That(task1HasRun, Is.True);
            Assert.That(task2HasRun, Is.False);
        }
    }
}