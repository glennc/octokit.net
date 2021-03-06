﻿using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.Serialization;

namespace Octokit
{
    /// <summary>
    /// 
    /// </summary>
#if !NETFX_CORE
    [Serializable]
#endif
    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors",
        Justification = "These exceptions are specific to the GitHub API and not general purpose exceptions")]
    public class TwoFactorRequiredException : AuthorizationException
    {
        /// <summary>
        /// Constructs an instance of TwoFactorRequiredException.
        /// </summary>
        public TwoFactorRequiredException() : this(TwoFactorType.None)
        {
        }

        /// <summary>
        /// Constructs an instance of TwoFactorRequiredException.
        /// </summary>
        /// <param name="twoFactorType">Expected 2FA response type</param>
        public TwoFactorRequiredException(TwoFactorType twoFactorType)
            : base(HttpStatusCode.Unauthorized, null)
        {
            TwoFactorType = twoFactorType;
        }

        /// <summary>
        /// Constructs an instance of TwoFactorRequiredException.
        /// </summary>
        /// <param name="response">The HTTP payload from the server</param>
        /// <param name="twoFactorType">Expected 2FA response type</param>
        public TwoFactorRequiredException(IResponse response, TwoFactorType twoFactorType) : base(response)
        {
            Debug.Assert(response != null && response.StatusCode == HttpStatusCode.Unauthorized,
                "TwoFactorRequiredException status code should be 401");

            TwoFactorType = twoFactorType;
        }

        public override string Message
        {
            get { return ApiErrorMessageSafe ?? "Two-factor authentication code is required"; }
        }

#if !NETFX_CORE
        /// <summary>
        /// Constructs an instance of TwoFactorRequiredException.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains
        /// contextual information about the source or destination.
        /// </param>
        protected TwoFactorRequiredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null) return;
            TwoFactorType = (TwoFactorType) (info.GetInt32("TwoFactorType"));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("TwoFactorType", TwoFactorType);
        }
#endif

        /// <summary>
        /// Expected 2FA response type
        /// </summary>
        public TwoFactorType TwoFactorType { get; private set; }
    }

    /// <summary>
    /// Methods for receiving 2FA authentication codes
    /// </summary>
    public enum TwoFactorType
    {
        /// <summary>
        /// No method configured
        /// </summary>
        None,
        /// <summary>
        /// Unknown method
        /// </summary>
        Unknown,
        /// <summary>
        /// Receive via SMS
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sms")]
        Sms,
        /// <summary>
        /// Receive via application
        /// </summary>
        AuthenticatorApp
    }
}
