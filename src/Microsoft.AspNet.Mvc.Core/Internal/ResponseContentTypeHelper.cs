﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNet.Mvc.Core;
using Microsoft.Net.Http.Headers;

namespace Microsoft.AspNet.Mvc.Internal
{
    public static class ResponseContentTypeHelper
    {
        public static MediaTypeHeaderValue GetContentType(
            MediaTypeHeaderValue actionResultContentType,
            string httpResponseContentType,
            MediaTypeHeaderValue defaultContentType)
        {
            if (defaultContentType == null)
            {
                throw new ArgumentNullException(nameof(defaultContentType));
            }

            if (defaultContentType.Encoding == null)
            {
                throw new InvalidOperationException(
                    Resources.FormatDefaultContentTypeMustHaveEncoding(defaultContentType.ToString()));
            }

            // 1. User sets the ContentType property on the action result
            if (actionResultContentType != null)
            {
                if (actionResultContentType.Encoding == null)
                {
                    // Do not modify the user supplied content type, so copy it instead
                    var contentType = actionResultContentType.Copy();
                    contentType.Encoding = defaultContentType.Encoding;
                    return contentType;
                }

                return actionResultContentType;
            }

            // 2. User sets the ContentType property on the http response directly
            if (!string.IsNullOrEmpty(httpResponseContentType))
            {
                var contentType = MediaTypeHeaderValue.Parse(httpResponseContentType);
                contentType.Encoding = contentType.Encoding ?? defaultContentType.Encoding;
                return contentType;
            }

            // 3. Fall-back to the default content type
            return defaultContentType;
        }
    }
}
