﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Aggregates
{
    public interface IIdentity
    {
        string Value { get; }
    }
}
