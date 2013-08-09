using System.Net.Sockets; // For Socket

public class EchoProtocolFactory : IProtocolFactory {
  public EchoProtocolFactory() {}

  public IProtocol createProtocol(Socket clntSock, ILogger logger) {
    return new EchoProtocol(clntSock, logger);
  }
}
