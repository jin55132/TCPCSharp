using System.Net.Sockets;  // For Socket

public interface IProtocolFactory {
  IProtocol createProtocol(Socket clntSock, ILogger logger);
}
