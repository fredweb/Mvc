﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Microsoft.AspNet.Mvc.Internal
{
    public class ResponseContentTypeHelperTest
    {
        public static TheoryData<MediaTypeHeaderValue, string, string> ResponseContentTypeData
        {
            get
            {
                // contentType, responseContentType, expectedContentType
                return new TheoryData<MediaTypeHeaderValue, string, string>
                {
                    {
                        null,
                        null,
                        "text/default; p1=p1-value; charset=utf-8"
                    },
                    {
                        new MediaTypeHeaderValue("text/foo"),
                        null,
                        "text/foo"
                    },
                    {
                        MediaTypeHeaderValue.Parse("text/foo; charset=us-ascii"),
                        null,
                        "text/foo; charset=us-ascii"
                    },
                    {
                        MediaTypeHeaderValue.Parse("text/foo; p1=p1-value"),
                        null,
                        "text/foo; p1=p1-value"
                    },
                    {
                        MediaTypeHeaderValue.Parse("text/foo; p1=p1-value; charset=us-ascii"),
                        null,
                        "text/foo; p1=p1-value; charset=us-ascii"
                    },
                    {
                        null,
                        "text/bar",
                        "text/bar"
                    },
                    {
                        null,
                        "text/bar; p1=p1-value",
                        "text/bar; p1=p1-value"
                    },
                                        {
                        null,
                        "text/bar; p1=p1-value; charset=us-ascii",
                        "text/bar; p1=p1-value; charset=us-ascii"
                    },
                    {
                        MediaTypeHeaderValue.Parse("text/foo; charset=us-ascii"),
                        "text/bar",
                        "text/foo; charset=us-ascii"
                    },
                    {
                        MediaTypeHeaderValue.Parse("text/foo; charset=us-ascii"),
                        "text/bar; charset=utf-8",
                        "text/foo; charset=us-ascii"
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(ResponseContentTypeData))]
        public void GetsExpectedContentTypeAndEncoding(
            MediaTypeHeaderValue contentType,
            string responseContentType,
            string expectedContentType)
        {
            // Arrange
            var defaultContentType = MediaTypeHeaderValue.Parse("text/default; p1=p1-value; charset=utf-8");

            // Act
            string resolvedContentType = null;
            Encoding resolvedContentTypeEncoding = null;
            ResponseContentTypeHelper.ResolveContentTypeAndEncoding(
                contentType,
                responseContentType,
                defaultContentType,
                out resolvedContentType,
                out resolvedContentTypeEncoding);

            // Assert
            Assert.Equal(expectedContentType, resolvedContentType);
        }

        [Fact]
        public void DoesNotThrowException_OnInvalidResponseContentType()
        {
            // Arrange
            var expectedContentType = "invalid-content-type";
            var defaultContentType = MediaTypeHeaderValue.Parse("text/plain; charset=utf-8");

            // Act
            string resolvedContentType = null;
            Encoding resolvedContentTypeEncoding = null;
            ResponseContentTypeHelper.ResolveContentTypeAndEncoding(
                null,
                expectedContentType,
                defaultContentType,
                out resolvedContentType,
                out resolvedContentTypeEncoding);

            // Assert
            Assert.Equal(expectedContentType, resolvedContentType);
            Assert.Equal(Encoding.UTF8, resolvedContentTypeEncoding);
        }

        public static TheoryData<MediaTypeHeaderValue, string> ThrowsExceptionOnNullDefaultContentTypeData
        {
            get
            {
                return new TheoryData<MediaTypeHeaderValue, string>
                {
                    {
                        null,
                        null
                    },
                    {
                        null,
                        "text/default"
                    },
                    {
                        new MediaTypeHeaderValue("text/default"),
                        null
                    },
                    {
                        new MediaTypeHeaderValue("text/default"),
                        "text/default"
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(ThrowsExceptionOnNullDefaultContentTypeData))]
        public void ThrowsExceptionOn_NullDefaultContentType(
            MediaTypeHeaderValue actionResultContentType,
            string httpResponseContentType)
        {
            // Arrange, Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                string resolvedContentType = null;
                Encoding resolvedContentTypeEncoding = null;
                ResponseContentTypeHelper.ResolveContentTypeAndEncoding(
                    actionResultContentType,
                    httpResponseContentType,
                    null, // default content type
                    out resolvedContentType,
                    out resolvedContentTypeEncoding);
            });
        }

        public static TheoryData<MediaTypeHeaderValue, string> ThrowsExceptionOnNullDefaultContentTypeEncodingData
        {
            get
            {
                return new TheoryData<MediaTypeHeaderValue, string>
                {
                    {
                        null,
                        null
                    },
                    {
                        null,
                        "text/default"
                    },
                    {
                        new MediaTypeHeaderValue("text/default"),
                        null
                    },
                    {
                        new MediaTypeHeaderValue("text/default"),
                        "text/default"
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(ThrowsExceptionOnNullDefaultContentTypeEncodingData))]
        public void ThrowsExceptionOn_NullDefaultContentTypeEncoding(
            MediaTypeHeaderValue actionResultContentType,
            string httpResponseContentType)
        {
            // Arrange
            var defaultContentType = MediaTypeHeaderValue.Parse("text/bar; p1=p1-value");

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                string resolvedContentType = null;
                Encoding resolvedContentTypeEncoding = null;
                ResponseContentTypeHelper.ResolveContentTypeAndEncoding(
                    actionResultContentType,
                    httpResponseContentType,
                    defaultContentType,
                    out resolvedContentType,
                    out resolvedContentTypeEncoding);
            });
            Assert.Equal(
                $"The default content type '{defaultContentType.ToString()}' must have an encoding.",
                exception.Message);
        }
    }
}
