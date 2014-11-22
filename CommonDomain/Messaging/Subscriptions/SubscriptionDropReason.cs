using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Messaging.Subscriptions
{
    // Summary:
    //     Represents the reason subscription drop happened
    public enum SubscriptionDropReason
    {
        // Summary:
        //     Subscription dropped because the client called Close.
        UserInitiated = 0,
        //
        // Summary:
        //     Subscription dropped because the client is not authenticated.
        NotAuthenticated = 1,
        //
        // Summary:
        //     Subscription dropped because access to the stream was denied.
        AccessDenied = 2,
        //
        // Summary:
        //     Subscription dropped because of an error in the subscription phase.
        SubscribingError = 3,
        //
        // Summary:
        //     Subscription dropped because of a server error.
        ServerError = 4,
        //
        // Summary:
        //     Subscription dropped because the connection was closed.
        ConnectionClosed = 5,
        //
        // Summary:
        //     Subscription dropped because of an error during the catch-up phase.
        CatchUpError = 6,
        //
        // Summary:
        //     Subscription dropped because it's queue overflowed.
        ProcessingQueueOverflow = 7,
        //
        // Summary:
        //     Subscription dropped because an exception was thrown by a handler.
        EventHandlerException = 8,
        //
        // Summary:
        //     Subscription was dropped for an unknown reason.
        Unknown = 100,
    }
}
