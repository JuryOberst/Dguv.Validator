// <copyright file="Check99.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

using System.Linq;

namespace Dguv.Validator.Format.Checks
{
    public class Check99 : IDguvChecksumHandler
    {
        private readonly Check01 _check1 = new Check01();
        private readonly Check15 _check2 = new Check15();

        /// <inheritdoc />
        public int Id => 99;

        /// <inheritdoc />
        public string[] Calculate(string membershipNumber)
        {
            return _check1.Calculate(membershipNumber).Concat(_check2.Calculate(membershipNumber)).ToArray();
        }
    }
}
