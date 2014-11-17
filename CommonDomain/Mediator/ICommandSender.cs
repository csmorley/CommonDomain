using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Mediator
{
    public interface ICommandSender
    {
        Result SendCommand<TCommand>(TCommand commandToSend) where TCommand : class;
    }
}
