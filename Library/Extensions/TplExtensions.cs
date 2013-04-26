//    This file is part of Nfield.SDK.
//
//    Nfield.SDK is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Nfield.SDK is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public License
//    along with Nfield.SDK.  If not, see <http://www.gnu.org/licenses/>.
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
            return task.ContinueWith(errorTask =>
                {
                    var tcs = new TaskCompletionSource<T>();

                    if (errorTask.Exception != null) // Exceptions occured
                    {
                        tcs.SetException(errorTask.Exception.Flatten().InnerExceptions);
                    }

                    return tcs.Task;
                    //According to this link http://msdn.microsoft.com/en-us/library/vstudio/system.threading.tasks.taskcontinuationoptions(v=vs.100).aspx
                    //this option doesn't work for multi task continuations.
                }, TaskContinuationOptions.OnlyOnFaulted).Unwrap();
        }
    }
}