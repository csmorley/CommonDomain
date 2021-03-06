﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Bus
{
    public interface IHandle<T> where T : Message
    {
        void Handle(T message);
    }
}
