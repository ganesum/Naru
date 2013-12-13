using System;
using System.Threading.Tasks;

namespace Naru.TPL
{
    // Based on: http://social.msdn.microsoft.com/Forums/sk/async/thread/30f3339c-5e04-4aa8-9a09-9be72d9d9a1b
    public static class EventAsync
    {
        /// <summary>
        /// Creates a <see cref="System.Threading.Tasks.Task"/>
        /// that waits for an event to occur.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// await EventAsync.FromEvent(
        ///     eh => storyboard.Completed += eh,
        ///     eh => storyboard.Completed -= eh);
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
        public static Task FromEvent(Action<EventHandler> addEventHandler, Action<EventHandler> removeEventHandler)
        {
            return new EventHandlerTaskSource(addEventHandler, removeEventHandler).Task;
        }

        private sealed class EventHandlerTaskSource
        {
            private readonly TaskCompletionSource<object> _tcs;
            private readonly Action<EventHandler> _removeEventHandler;

            public EventHandlerTaskSource(Action<EventHandler> addEventHandler, Action<EventHandler> removeEventHandler)
            {
                if (addEventHandler == null)
                {
                    throw new ArgumentNullException("addEventHandler");
                }

                if (removeEventHandler == null)
                {
                    throw new ArgumentNullException("removeEventHandler");
                }

                _tcs = new TaskCompletionSource<object>();
                _removeEventHandler = removeEventHandler;
                addEventHandler(EventCompleted);
            }

            /// <summary>
            /// Returns a task that waits for the event to occur.
            /// </summary>
            public Task<object> Task
            {
                get { return _tcs.Task; }
            }

            private void EventCompleted(object sender, EventArgs args)
            {
                _removeEventHandler.Invoke(EventCompleted);
                _tcs.SetResult(null);
            }
        }

        /// <summary>
        /// Creates a <see cref="System.Threading.Tasks.Task"/>
        /// that waits for an event to occur.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// await EventAsync.FromEvent(
        ///     eh => storyboard.Completed += eh,
        ///     eh => storyboard.Completed -= eh);
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
        public static Task<T> FromEvent<T>(Action<EventHandler<T>> addEventHandler, Action<EventHandler<T>> removeEventHandler)
            where T : EventArgs
        {
            return new EventHandlerTaskSource<T>(addEventHandler, removeEventHandler).Task;
        }

        private sealed class EventHandlerTaskSource<TEventArgs>
            where TEventArgs : EventArgs
        {
            private readonly TaskCompletionSource<TEventArgs> _tcs;
            private readonly Action<EventHandler<TEventArgs>> _removeEventHandler;

            public EventHandlerTaskSource(Action<EventHandler<TEventArgs>> addEventHandler, Action<EventHandler<TEventArgs>> removeEventHandler)
            {
                if (addEventHandler == null)
                {
                    throw new ArgumentNullException("addEventHandler");
                }

                if (removeEventHandler == null)
                {
                    throw new ArgumentNullException("removeEventHandler");
                }

                _tcs = new TaskCompletionSource<TEventArgs>();
                _removeEventHandler = removeEventHandler;
                addEventHandler(EventCompleted);
            }

            /// <summary>
            /// Returns a task that waits for the event to occur.
            /// </summary>
            public Task<TEventArgs> Task
            {
                get { return _tcs.Task; }
            }

            private void EventCompleted(object sender, TEventArgs args)
            {
                _removeEventHandler.Invoke(EventCompleted);
                _tcs.SetResult(args);
            }
        }
    }
}