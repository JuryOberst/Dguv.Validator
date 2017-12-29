// <copyright file="UvCheckStatus.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

namespace Dguv.Validator.Format
{
    public class UvCheckStatus : IStatus
    {
        private readonly string _message;

        public UvCheckStatus(int code, string message)
        {
            _message = message;
            Code = code;
        }

        public int Code { get; }

        public bool IsSuccessful => Code == 0;

        public string GetStatusText() => _message;
    }
}
