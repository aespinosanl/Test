using System;
using System.Collections.Generic;
using System.Linq;

namespace Nfield.Infrastructure
{
    /// <summary>
    /// Extends <see cref="IDependencyResolver"/> with generic implementations of the <see cref="IDependencyResolver.Resolve"/>
    /// and <see cref="IDependencyResolver.ResolveAll"/> methods.
    /// </summary>
    public static class DependencyResolverExtensions
    {

        /// <summary>
        /// Resolve classes of type T
        /// </summary>
        /// <typeparam name="T">type to resolve</typeparam>
        /// <param name="dependencyResolver">resolver</param>
        /// <returns>instance of type T</returns>
        public static T Resolve<T>(this IDependencyResolver dependencyResolver)
        {
            return (T) dependencyResolver.Resolve(typeof (T));
        }

        /// <summary>
        /// Resolve classes of tyep T
        /// </summary>
        /// <typeparam name="T">type to resolve</typeparam>
        /// <param name="dependencyResolver">resolver</param>
        /// <returns>Enumeration of all instances of type T</returns>
        public static IEnumerable<T> ResolveAll<T>(this IDependencyResolver dependencyResolver)
        {
            return dependencyResolver.ResolveAll(typeof (T))
                                     .Cast<T>();
        }

    }
}
