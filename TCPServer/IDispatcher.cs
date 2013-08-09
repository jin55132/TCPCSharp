using System.Net.Sockets;  // For TcpListener

public interface IDispatcher {
  void startDispatching(TcpListener listener, ILogger logger,
                        IProtocolFactory protoFactory);
}
