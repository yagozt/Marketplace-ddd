using Marketplace.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marketplace.Domain
{
    public class UserId : Value<UserId>
    {
        private readonly Guid _value;
        public UserId(Guid value)
        {
            _value = value;
        }
    }
}
