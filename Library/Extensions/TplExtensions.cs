using System.Threading.Tasks;

namespace Nfield.Extensions
{
    internal static class TplExtensions
    {
        /// <summary>
        /// Flattens exception structure of AggregateExceptions without the need of catching/rethrowing
        /// </summary>
        public static Task<T> FlattenExceptions<T>(this Task<T> task)
        {
            return task.ContinueWith((errorTask) =>
                {
                    var tcs = new TaskCompletionSource<T>();

                    if (errorTask.Exception != null) // Exceptions occured
                    {
                        tcs.SetException(errorTask.Exception.Flatten().InnerExceptions);
                    }

                    return tcs.Task;
                }, TaskContinuationOptions.OnlyOnFaulted).Unwrap();
        }
    }
}