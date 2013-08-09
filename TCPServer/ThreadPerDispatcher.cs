using System.Net.Sockets; // For TcpListener, Socket
using System.Threading;   // For Thread

class ThreadPerDispatcher : IDispatcher {

  public void startDispatching(TcpListener listener, ILogger logger,
                               IProtocolFactory protoFactory) {

    // Run forever, accepting and spawning threads to service each connection

    for (;;) {
      try {
        listener.Start();
        Socket clntSock = listener.AcceptSocket();  // Block waiting for connection
        IProtocol protocol = protoFactory.createProtocol(clntSock, logger);
        Thread thread = new Thread(new ThreadStart(protocol.handleclient));
        thread.Start();
        logger.writeEntry("Created and started Thread = " + thread.GetHashCode());
      } catch (System.IO.IOException e) {
        logger.writeEntry("Exception = " + e.Message);
      }
    }
    /* NOT REACHED */
  }
}
