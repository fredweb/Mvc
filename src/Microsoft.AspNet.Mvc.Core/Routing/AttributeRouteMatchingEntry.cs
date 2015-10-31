// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNet.Routing;
using Microsoft.AspNet.Routing.Template;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNet.Mvc.Routing
{
    /// <summary>
    /// Used to build an <see cref="TreeRouter"/>. Represents an individual URL-matching route that will be
    /// aggregated into the <see cref="TreeRouter"/>.
    /// </summary>
    public class AttributeRouteMatchingEntry
    {
        /// <summary>
        /// The order of the template.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// The precedence of the template.
        /// </summary>
        public decimal Precedence { get; set; }

        public IRouter Target { get; set; }

        public string RouteName { get; set; }

        public RouteTemplate RouteTemplate { get; set; }

        public TemplateMatcher TemplateMatcher { get; set; }

        public IReadOnlyDictionary<string, IRouteConstraint> Constraints { get; set; }
    }
}
