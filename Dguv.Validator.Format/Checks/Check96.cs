﻿// <copyright file="Check96.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

using System.Linq;

namespace Dguv.Validator.Format.Checks
{
    public class Check96 : IDguvChecksumHandler
    {
        private readonly Check16 _check1 = new Check16();
        private readonly Check15 _check2 = new Check15();

        /// <inheritdoc />
        public int Id => 96;

        /// <inheritdoc />
        public string[] Calculate(string membershipNumber)
        {
            return _check1.Calculate(membershipNumber).Concat(_check2.Calculate(membershipNumber)).ToArray();
        }
    }
}
