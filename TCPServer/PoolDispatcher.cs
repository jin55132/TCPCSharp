using System.Threading;    // For Thread
using System.Net.Sockets;  // For TcpListener

class PoolDispatcher : IDispatcher {

  private const int NUMTHREADS = 8;  // Default thread pool size

  private int numThreads;            // Number of threads in pool

  public PoolDispatcher() {
    this.numThreads = NUMTHREADS;
  }

  public PoolDispatcher(int numThreads) {
    this.numThreads = numThreads;
  }

  public void startDispatching(TcpListener listener, ILogger logger,
                               IProtocolFactory protoFactory) {
    // Create N threads, each running an iterative server
    for (int i = 0; i < numThreads; i++) { 
      DispatchLoop dl = new DispatchLoop(listener, logger, protoFactory);
      Thread thread = new Thread(new ThreadStart(dl.rundispatcher));
      thread.Start();
      logger.writeEntry("Created and started Thread = " + thread.GetHashCode());
    }
  }
}

class DispatchLoop {

  TcpListener listener;
  ILogger logger;
  IProtocolFactory protoFactory;

  public DispatchLoop(TcpListener listener, ILogger logger, 
                      IProtocolFactory protoFactory) {
    this.listener     = listener;
    this.logger       = logger;
    this.protoFactory = protoFactory;
  }

  public void rundispatcher() {
    // Run forever, accepting and handling each connection
    for (;;) {
      try {
        Socket clntSock = listener.AcceptSocket();  // Block waiting for connection
        IProtocol protocol = protoFactory.createProtocol(clntSock, logger);
        protocol.handleclient();
      } catch (SocketException se) {
        logger.writeEntry("Exception = " +  se.Message);
      }
    }
  }
}
