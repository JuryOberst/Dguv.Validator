// <copyright file="StatusExtensions.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

namespace Dguv.Validator
{
    public static class StatusExtensions
    {
        public static void EnsureSuccess(this IStatus status)
        {
            if (!status.IsSuccessful)
                throw new DguvValidationException(status.GetStatusText());
        }
    }
}
