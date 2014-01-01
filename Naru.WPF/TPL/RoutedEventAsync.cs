using System;
using System.Threading.Tasks;
using System.Windows;

namespace Naru.WPF.TPL
{
    // Based on: http://social.msdn.microsoft.com/Forums/sk/async/thread/30f3339c-5e04-4aa8-9a09-9be72d9d9a1b
    public static class RoutedEventAsync
    {
        /// <summary>
        /// Creates a <see cref="System.Threading.Tasks.Task"/>
        /// that waits for an event to occur.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// await RoutedEventAsync.FromRoutedEvent(
        ///     eh => button.Click += eh,
        ///     eh => button.Click -= eh);
        /// ]]>
        /// </example>
        /// <param name="addEventHandler">
        /// The action that subscribes to the event.
        /// </param>
        /// <param name="removeEventHandler">
        /// The action that unsubscribes from the event when it first occurs.
        /// </param>
        /// <returns>
        /// The <see cref="System.Threading.Tasks.Task"/> that
        /// completes when the event registered in
        /// <paramref name="addEventHandler"/> occurs.
        /// </returns>
        public static Task<RoutedEventArgs> FromRoutedEvent(Action<RoutedEventHandler> addEventHandler, Action<RoutedEventHandler> removeEventHandler)
        {
            return new RoutedEventHandlerTaskSource(addEventHandler, removeEventHandler).Task;
        }

        private sealed class RoutedEventHandlerTaskSource
        {
            private readonly TaskCompletionSource<RoutedEventArgs> _tcs;
            private readonly Action<RoutedEventHandler> _removeEventHandler;

            public RoutedEventHandlerTaskSource(Action<RoutedEventHandler> addEventHandler, Action<RoutedEventHandler> removeEventHandler)
            {
                if (addEventHandler == null)
                {
                    throw new ArgumentNullException("addEventHandler");
                }

                if (removeEventHandler == null)
                {
                    throw new ArgumentNullException("removeEventHandler");
                }

                _tcs = new TaskCompletionSource<RoutedEventArgs>();
                _removeEventHandler = removeEventHandler;
                addEventHandler(EventCompleted);
            }

            /// <summary>
            /// Returns a task that waits for the routed to occur.
            /// </summary>
            public Task<RoutedEventArgs> Task
            {
                get { return _tcs.Task; }
            }

            private void EventCompleted(object sender, RoutedEventArgs args)
            {
                _removeEventHandler(EventCompleted);
                _tcs.SetResult(args);
            }
        }
    }
}