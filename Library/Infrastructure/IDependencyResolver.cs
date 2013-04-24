using System;
using System.Collections.Generic;

namespace Nfield.Infrastructure
{
    /// <summary>
    /// A simple interface that can be implemented to expose a Inversion of Control container to the SDK. Internally the
    /// SDK uses this interface, exposed through <see cref="Nfield.Infrastructure.DependencyResolver"/> to resolve
    /// all dependencies.
    /// </summary>
    public interface IDependencyResolver
    {

        /// <summary>
        /// Resolves a single registered type.
        /// </summary>
        /// <param name="typeToResolve"></param>
        /// <returns></returns>
        object Resolve(Type typeToResolve);

        /// <summary>
        /// Resolves multiple registered instances.
        /// </summary>
        /// <param name="typeToResolve">The type to resolve</param>
        /// <returns></returns>
        IEnumerable<object> ResolveAll(Type typeToResolve);

    }
}
