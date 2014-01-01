//--------------------------------------------------------------------------
// 
//  Copyright (c) Microsoft Corporation.  All rights reserved. 
//
//--------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Common.Logging;

namespace Naru.TPL
{
    public static class TaskEx
    {
        /// <summary>
        /// Catch exceptions ONLY of type TException and allow an Action to be performed on it.
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="task"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static Task Catch<TException>(this Task task, Action<TException> exceptionHandler)
            where TException : Exception
        {
            if (task == null) throw new ArgumentNullException("task");
            if (exceptionHandler == null) throw new ArgumentNullException("exceptionHandler");

            var tcs = new TaskCompletionSource<object>();
            task.ContinueWith(t =>
            {
                if (!task.IsFaulted && task.Exception == null)
                {
                    try
                    {
                        tcs.TrySetResult(null);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
                else if (task.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else if (task.IsFaulted)
                {
                    var exception = t.Exception.Flatten().InnerExceptions.FirstOrDefault() ?? t.Exception;

                    if (exception is TException)
                    {
                        exceptionHandler((TException)exception);
                    }

                    tcs.TrySetException(exception);
                }
            }, TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }

        /// <summary>
        /// Catch exceptions ONLY of type TException and allow an Action to be performed on it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <param name="task"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static Task<T> Catch<T, TException>(this Task<T> task, Action<TException> exceptionHandler)
            where TException : Exception
        {
            if (task == null) throw new ArgumentNullException("task");
            if (exceptionHandler == null) throw new ArgumentNullException("exceptionHandler");

            var tcs = new TaskCompletionSource<T>();
            task.ContinueWith(t =>
            {
                if (!task.IsFaulted && task.Exception == null)
                {
                    try
                    {
                        tcs.TrySetResult(t.Result);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
                else if (task.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else if (task.IsFaulted)
                {
                    var exception = t.Exception.Flatten().InnerExceptions.FirstOrDefault() ?? t.Exception;

                    if (exception is TException)
                    {
                        exceptionHandler((TException)exception);
                    }

                    tcs.TrySetException(exception);
                }
            }, TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }

        /// <summary>
        /// Catch ANY exception and allow an Action to be performed on it.
        /// Handle all exceptions raised.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="exceptionHandler"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public static Task CatchAndHandle(this Task task, Action<Exception> exceptionHandler, TaskScheduler scheduler)
        {
            return CatchAndHandle<Exception>(task, exceptionHandler, scheduler);
        }

        /// <summary>
        /// Catch exceptions ONLY of type TException and allow an Action to be performed on it.
        /// Handle all exceptions raised.
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="task"></param>
        /// <param name="exceptionHandler"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public static Task CatchAndHandle<TException>(this Task task, Action<TException> exceptionHandler, TaskScheduler scheduler)
            where TException : Exception
        {
            if (task == null) throw new ArgumentNullException("task");
            if (exceptionHandler == null) throw new ArgumentNullException("exceptionHandler");

            var tcs = new TaskCompletionSource<object>();
            task.ContinueWith(t =>
            {
                if (!task.IsFaulted && task.Exception == null)
                {
                    try
                    {
                        tcs.TrySetResult(null);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
                else if (task.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else if (task.IsFaulted)
                {
                    var exception = t.Exception.Flatten().InnerExceptions.FirstOrDefault() ?? t.Exception;

                    if (exception is TException)
                    {
                        exceptionHandler((TException)exception);
                    }

                    tcs.TrySetResult(null);
                }
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, scheduler);

            return tcs.Task;
        }
        
        public static Task Do(this Task first, Action next, TaskScheduler scheduler)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (next == null) throw new ArgumentNullException("next");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            var tcs = new TaskCompletionSource<object>();
            first.ContinueWith(_ =>
            {
                if (first.IsFaulted)
                {
                    tcs.TrySetException(first.Exception.InnerExceptions);
                }
                else if (first.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    try
                    {
                        next();
                        tcs.TrySetResult(null);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
            }, CancellationToken.None, TaskContinuationOptions.None, scheduler);

            return tcs.Task;
        }

        public static Task<T1> Do<T1>(this Task<T1> first, Action next, TaskScheduler scheduler)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (next == null) throw new ArgumentNullException("next");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            var tcs = new TaskCompletionSource<T1>();
            first.ContinueWith(_ =>
            {
                if (first.IsFaulted)
                {
                    tcs.TrySetException(first.Exception.InnerExceptions);
                }
                else if (first.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    try
                    {
                        next();
                        tcs.TrySetResult(first.Result);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
            }, CancellationToken.None, TaskContinuationOptions.None, scheduler);

            return tcs.Task;
        }

        public static Task<T1> Do<T1>(this Task<T1> first, Action<T1> next, TaskScheduler scheduler)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (next == null) throw new ArgumentNullException("next");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            var tcs = new TaskCompletionSource<T1>();
            first.ContinueWith(_ =>
            {
                if (first.IsFaulted)
                {
                    tcs.TrySetException(first.Exception.InnerExceptions);
                }
                else if (first.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    try
                    {
                        next(first.Result);

                        tcs.TrySetResult(first.Result);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
            }, CancellationToken.None, TaskContinuationOptions.None, scheduler);

            return tcs.Task;
        }

        public static Task<T1> Do<T1>(this Task<T1> first, Func<Task> next, TaskScheduler scheduler)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (next == null) throw new ArgumentNullException("next");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            var tcs = new TaskCompletionSource<T1>();
            first.ContinueWith(_ =>
            {
                if (first.IsFaulted)
                {
                    tcs.TrySetException(first.Exception.InnerExceptions);
                }
                else if (first.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    try
                    {
                        next().Do(() => tcs.TrySetResult(first.Result), scheduler);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
            }, CancellationToken.None, TaskContinuationOptions.None, scheduler);

            return tcs.Task;
        }

        public static Task<T1> Do<T1>(this Task<T1> first, Func<T1, Task> next, TaskScheduler scheduler)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (next == null) throw new ArgumentNullException("next");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            var tcs = new TaskCompletionSource<T1>();
            first.ContinueWith(_ =>
            {
                if (first.IsFaulted)
                {
                    tcs.TrySetException(first.Exception.InnerExceptions);
                }
                else if (first.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    try
                    {
                        next(first.Result).Do(() => tcs.TrySetResult(first.Result), scheduler);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
            }, CancellationToken.None, TaskContinuationOptions.None, scheduler);

            return tcs.Task;
        }

        /// <summary>
        /// ALWAYS execute the action.
        /// This should not be composed off of.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="action"></param>
        /// <param name="scheduler"></param>
        public static Task Finally(this Task task, Action action, TaskScheduler scheduler)
        {
            if (action == null) throw new ArgumentNullException("action");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            var tcs = new TaskCompletionSource<object>();
            task.ContinueWith(t =>
            {
                action();
                tcs.TrySetResult(null);
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, scheduler);

            return tcs.Task;
        }

        public static Task<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
            this Task<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            // Validate arguments
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");
            if (elementSelector == null) throw new ArgumentNullException("elementSelector");

            // When the source completes, return a grouping of just the one element
            return source.ContinueWith(t =>
            {
                var result = t.Result;
                var key = keySelector(result);
                var element = elementSelector(result);
                return (IGrouping<TKey, TElement>)new OneElementGrouping<TKey, TElement> { Key = key, Element = element };
            }, TaskContinuationOptions.NotOnCanceled);
        }

        public static Task<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
            this Task<TOuter> outer, Task<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, Task<TInner>, TResult> resultSelector,
            TaskScheduler scheduler)
        {
            // Argument validation handled by delegated method call
            return GroupJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default, scheduler);
        }

        public static Task<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
            this Task<TOuter> outer, Task<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, Task<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer,
            TaskScheduler scheduler)
        {
            // Validate arguments
            if (outer == null) throw new ArgumentNullException("outer");
            if (inner == null) throw new ArgumentNullException("inner");
            if (outerKeySelector == null) throw new ArgumentNullException("outerKeySelector");
            if (innerKeySelector == null) throw new ArgumentNullException("innerKeySelector");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");
            if (comparer == null) throw new ArgumentNullException("comparer");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            // First continue off of the outer and then off of the inner.  Two separate
            // continuations are used so that each may be canceled easily using the NotOnCanceled option.
            return outer.ContinueWith(delegate
            {
                var cts = new CancellationTokenSource();
                return inner.ContinueWith(delegate
                {
                    // Propagate all exceptions
                    Task.WaitAll(outer, inner);

                    // Both completed successfully, so if their keys are equal, return the result
                    if (comparer.Equals(outerKeySelector(outer.Result), innerKeySelector(inner.Result)))
                    {
                        return resultSelector(outer.Result, inner);
                    }
                        // Otherwise, cancel this task.
                    else
                    {
                        cts.CancelAndThrow();
                        return default(TResult); // won't be reached
                    }
                }, cts.Token, TaskContinuationOptions.NotOnCanceled, scheduler);
            }, TaskContinuationOptions.NotOnCanceled).Unwrap();
        }

        /// <summary>Represents a grouping of one element.</summary>
        /// <typeparam name="TKey">The type of the key for the element.</typeparam>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        private class OneElementGrouping<TKey, TElement> : IGrouping<TKey, TElement>
        {
            public TKey Key { get; internal set; }
            internal TElement Element { get; set; }
            public IEnumerator<TElement> GetEnumerator() { yield return Element; }
            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        }

        /// <summary>
        /// Handle all exceptions raised.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static Task HandleExceptions(this Task task)
        {
            if (task == null) throw new ArgumentNullException("task");

            var tcs = new TaskCompletionSource<object>();
            task.ContinueWith(t =>
            {
                if (!task.IsFaulted && task.Exception == null)
                {
                    try
                    {
                        t.Wait();
                        tcs.TrySetResult(null);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetResult(null);
                    }
                }
                else if (task.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else if (task.IsFaulted)
                {
                    // Do this so the exception is handled.
                    var exception = t.Exception.Flatten();

                    tcs.TrySetResult(null);
                }
            }, TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }

        public static Task<TResult> Join<TOuter, TInner, TKey, TResult>(
            this Task<TOuter> outer, Task<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            TaskScheduler scheduler)
        {
            // Argument validation handled by delegated method call
            return Join(outer, inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default, scheduler);
        }

        public static Task<TResult> Join<TOuter, TInner, TKey, TResult>(
            this Task<TOuter> outer, Task<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer,
            TaskScheduler scheduler)
        {
            // Validate arguments
            if (outer == null) throw new ArgumentNullException("outer");
            if (inner == null) throw new ArgumentNullException("inner");
            if (outerKeySelector == null) throw new ArgumentNullException("outerKeySelector");
            if (innerKeySelector == null) throw new ArgumentNullException("innerKeySelector");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");
            if (comparer == null) throw new ArgumentNullException("comparer");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            // First continue off of the outer and then off of the inner.  Two separate
            // continuations are used so that each may be canceled easily using the NotOnCanceled option.
            return outer.ContinueWith(delegate
            {
                var cts = new CancellationTokenSource();
                return inner.ContinueWith(delegate
                {
                    // Propagate all exceptions
                    Task.WaitAll(outer, inner);

                    // Both completed successfully, so if their keys are equal, return the result
                    if (comparer.Equals(outerKeySelector(outer.Result), innerKeySelector(inner.Result)))
                    {
                        return resultSelector(outer.Result, inner.Result);
                    }
                        // Otherwise, cancel this task.  
                    else
                    {
                        cts.CancelAndThrow();
                        return default(TResult); // won't be reached
                    }
                }, cts.Token, TaskContinuationOptions.NotOnCanceled, scheduler);
            }, TaskContinuationOptions.NotOnCanceled).Unwrap();
        }

        /// <summary>
        /// Log exceptions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static Task<T> LogException<T>(this Task<T> task, ILog logger)
        {
            task.ContinueWith(t =>
            {
                var exception = t.Exception.Flatten();

                logger.Error(exception);
            }, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnFaulted);

            return task;
        }

        /// <summary>
        /// Log exceptions.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static Task LogException(this Task task, ILog logger)
        {
            task.ContinueWith(t =>
            {
                var exception = t.Exception.Flatten();

                logger.Error(exception);
            }, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnFaulted);

            return task;
        }


        /// <summary>
        /// Returns a task which retries the task returned by the specified task provider.
        /// http://gorodinski.com/blog/2012/03/22/retry-with-the-net-task-parallel-library-tpl/
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="taskProvider"></param>
        /// <param name="maxAttemps"></param>
        /// <param name="shouldRetry"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public static Task<TResult> Retry<TResult>(this Task<TResult> task, Func<Task<TResult>> taskProvider,
                                                   int maxAttemps, Func<Exception, bool> shouldRetry,
                                                   TaskScheduler scheduler)
        {
            if (taskProvider == null) throw new ArgumentNullException("taskProvider");
            if (scheduler == null) throw new ArgumentNullException("scheduler");
            if (maxAttemps < 0) throw new ArgumentNullException("maxAttemps", "maxAttemps is a negative number.");

            if (shouldRetry == null)
            {
                shouldRetry = _ => true;
            }

            if (!task.IsFaulted)
            {
                return task;
            }

            if (maxAttemps > 0 && shouldRetry(task.Exception.InnerException))
            {
                return taskProvider()
                    .ContinueWith(retryTask =>
                                  Retry(retryTask, taskProvider, --maxAttemps, shouldRetry, scheduler),
                                  CancellationToken.None, TaskContinuationOptions.None, scheduler)
                    .Result;
            }

            // will throw AggregateException
            return task;
        }

        /// <summary>
        /// Task that immediately return the value passed in.
        /// </summary>
        /// <returns></returns>
        public static Task<T> Return<T>(T value)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.TrySetResult(value);
            return tcs.Task;
        }

        public static Task<TResult> Select<TSource, TResult>(this Task<TSource> source, Func<TSource, TResult> selector, TaskScheduler scheduler)
        {
            // Validate arguments
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            // Use a continuation to run the selector function
            return source.ContinueWith(t => selector(t.Result), CancellationToken.None, TaskContinuationOptions.NotOnCanceled, scheduler);
        }

        public static Task<TResult> Select<TResult>(this Task source, Func<TResult> selector, TaskScheduler scheduler)
        {
            // Validate arguments
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            // Use a continuation to run the selector function
            return source.ContinueWith(t => selector(), CancellationToken.None, TaskContinuationOptions.NotOnCanceled, scheduler);
        }

        #region Then

        /// <summary>
        /// When first completes, pass the results to next.
        /// next is only called if first completed successfully.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="first"></param>
        /// <param name="next"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public static Task<T2> Then<T1, T2>(this Task<T1> first, Func<T1, Task<T2>> next, TaskScheduler scheduler)
        {
            // http://blogs.msdn.com/b/pfxteam/archive/2010/11/21/10094564.aspx?Redirected=true

            if (first == null) throw new ArgumentNullException("first");
            if (next == null) throw new ArgumentNullException("next");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            var tcs = new TaskCompletionSource<T2>();
            first.ContinueWith(_ =>
            {
                if (first.IsFaulted)
                {
                    tcs.TrySetException(first.Exception.InnerExceptions);
                }
                else if (first.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    try
                    {
                        var t = next(first.Result);
                        if (t == null)
                        {
                            tcs.TrySetCanceled();
                        }
                        else
                        {
                            t.ContinueWith(__ =>
                            {
                                if (t.IsFaulted)
                                {
                                    tcs.TrySetException(t.Exception.InnerExceptions);
                                }
                                else if (t.IsCanceled)
                                {
                                    tcs.TrySetCanceled();
                                }
                                else
                                {
                                    tcs.TrySetResult(t.Result);
                                }
                            }, CancellationToken.None, TaskContinuationOptions.None, scheduler);
                        }
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
            }, CancellationToken.None, TaskContinuationOptions.None, scheduler);

            return tcs.Task;
        }

        /// <summary>
        /// When first completes.
        /// next is only called if first completed successfully.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="first"></param>
        /// <param name="next"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public static Task<T1> Then<T1>(this Task first, Func<Task<T1>> next, TaskScheduler scheduler)
        {
            // http://blogs.msdn.com/b/pfxteam/archive/2010/11/21/10094564.aspx?Redirected=true

            if (first == null) throw new ArgumentNullException("first");
            if (next == null) throw new ArgumentNullException("next");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            var tcs = new TaskCompletionSource<T1>();
            first.ContinueWith(_ =>
            {
                if (first.IsFaulted)
                {
                    tcs.TrySetException(first.Exception.InnerExceptions);
                }
                else if (first.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    try
                    {
                        var t = next();
                        if (t == null)
                        {
                            tcs.TrySetCanceled();
                        }
                        else
                        {
                            t.ContinueWith(__ =>
                            {
                                if (t.IsFaulted)
                                {
                                    tcs.TrySetException(t.Exception.InnerExceptions);
                                }
                                else if (t.IsCanceled)
                                {
                                    tcs.TrySetCanceled();
                                }
                                else
                                {
                                    tcs.TrySetResult(t.Result);
                                }
                            }, CancellationToken.None, TaskContinuationOptions.None, scheduler);
                        }
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
            }, CancellationToken.None, TaskContinuationOptions.None, scheduler);

            return tcs.Task;
        }

        /// <summary>
        /// When first completes, pass the results to next.
        /// next is only called if first completed successfully.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="first"></param>
        /// <param name="next"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public static Task Then<T1>(this Task<T1> first, Func<T1, Task> next, TaskScheduler scheduler)
        {
            // http://blogs.msdn.com/b/pfxteam/archive/2010/11/21/10094564.aspx?Redirected=true

            if (first == null) throw new ArgumentNullException("first");
            if (next == null) throw new ArgumentNullException("next");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            var tcs = new TaskCompletionSource<object>();
            first.ContinueWith(_ =>
            {
                if (first.IsFaulted)
                {
                    tcs.TrySetException(first.Exception.InnerExceptions);
                }
                else if (first.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    try
                    {
                        var t = next(first.Result);
                        if (t == null)
                        {
                            tcs.TrySetCanceled();
                        }
                        else
                        {
                            t.ContinueWith(__ =>
                            {
                                if (t.IsFaulted)
                                {
                                    tcs.TrySetException(t.Exception.InnerExceptions);
                                }
                                else if (t.IsCanceled)
                                {
                                    tcs.TrySetCanceled();
                                }
                                else
                                {
                                    tcs.TrySetResult(null);
                                }
                            }, CancellationToken.None, TaskContinuationOptions.None, scheduler);
                        }
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
            }, scheduler);

            return tcs.Task;
        }

        /// <summary>
        /// Execute an Action, Task completes when it is finished.
        /// next is only called if first completed successfully.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="first"></param>
        /// <param name="next"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public static Task<T2> Then<T1, T2>(this Task<T1> first, Func<T1, T2> next, TaskScheduler scheduler)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (next == null) throw new ArgumentNullException("next");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            var tcs = new TaskCompletionSource<T2>();
            first.ContinueWith(_ =>
            {
                if (first.IsFaulted)
                {
                    tcs.TrySetException(first.Exception.InnerExceptions);
                }
                else if (first.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    try
                    {
                        var result = next(first.Result);

                        tcs.TrySetResult(result);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
            }, scheduler);

            return tcs.Task;
        }

        /// <summary>
        /// When first completes, pass the results to next.
        /// next is only called if first completed successfully.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="next"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public static Task Then(this Task first, Func<Task> next, TaskScheduler scheduler)
        {
            // http://blogs.msdn.com/b/pfxteam/archive/2010/11/21/10094564.aspx?Redirected=true

            if (first == null) throw new ArgumentNullException("first");
            if (next == null) throw new ArgumentNullException("next");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            var tcs = new TaskCompletionSource<object>();
            first.ContinueWith(_ =>
            {
                if (first.IsFaulted) tcs.TrySetException(first.Exception.InnerExceptions);
                else if (first.IsCanceled) tcs.TrySetCanceled();
                else
                {
                    try
                    {
                        var t = next();
                        if (t == null) tcs.TrySetCanceled();
                        else
                            t.ContinueWith(__ =>
                            {
                                if (t.IsFaulted) tcs.TrySetException(t.Exception.InnerExceptions);
                                else if (t.IsCanceled) tcs.TrySetCanceled();
                                else tcs.TrySetResult(null);
                            }, CancellationToken.None, TaskContinuationOptions.None, scheduler);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
            }, scheduler);

            return tcs.Task;
        }

        #endregion

        public static Task StartNew(this TaskFactory taskFactory, Action action, TaskScheduler scheduler)
        {
            if (action == null) throw new ArgumentNullException("action");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            return taskFactory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, scheduler);
        }

        public static Task<T1> StartNew<T1>(this TaskFactory taskFactory, Func<T1> action, TaskScheduler scheduler)
        {
            if (action == null) throw new ArgumentNullException("action");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            return taskFactory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, scheduler);
        }

        public static Task<T1> StartNew<T1>(this TaskFactory taskFactory, Func<object, T1> action, T1 state, TaskScheduler scheduler)
        {
            if (action == null) throw new ArgumentNullException("action");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            return taskFactory.StartNew<T1>(action, state, CancellationToken.None, TaskCreationOptions.None, scheduler);
        }

        #region Timeouts

        /// <summary>Creates a new Task that mirrors the supplied task but that will be canceled after the specified timeout.</summary>
        /// <param name="task">The task.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The new Task that may time out.</returns>
        public static Task WithTimeout(this Task task, TimeSpan timeout)
        {
            var result = new TaskCompletionSource<object>(task.AsyncState);
            var timer = new Timer(state => ((TaskCompletionSource<object>)state).TrySetCanceled(), result, timeout, TimeSpan.FromMilliseconds(-1));
            task.ContinueWith(t =>
            {
                timer.Dispose();
                result.TrySetFromTask(t);
            }, TaskContinuationOptions.ExecuteSynchronously);
            return result.Task;
        }

        /// <summary>Creates a new Task that mirrors the supplied task but that will be canceled after the specified timeout.</summary>
        /// <typeparam name="TResult">Specifies the type of data contained in the task.</typeparam>
        /// <param name="task">The task.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The new Task that may time out.</returns>
        public static Task<TResult> WithTimeout<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            var result = new TaskCompletionSource<TResult>(task.AsyncState);
            var timer = new Timer(state => ((TaskCompletionSource<TResult>)state).TrySetCanceled(), result, timeout, TimeSpan.FromMilliseconds(-1));
            task.ContinueWith(t =>
            {
                timer.Dispose();
                result.TrySetFromTask(t);
            }, TaskContinuationOptions.ExecuteSynchronously);
            return result.Task;
        }

        #endregion
    }
}