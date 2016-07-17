using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc.Identity.Common
{/// <summary>
    /// Possible results from a sign in attempt
    /// </summary>
    public enum AppSignInStatus
    {
        /// <summary>
        /// Sign in was successful
        /// </summary>
        Success,

        /// <summary>
        /// User is locked out
        /// </summary>
        LockedOut,

        /// <summary>
        /// Sign in requires addition verification (i.e. two factor)
        /// </summary>
        RequiresVerification,

        /// <summary>
        /// Sign in failed
        /// </summary>
        Failure,
        /// <summary>
        /// make sure email
        /// </summary>
        NotSureEmail
    }

}