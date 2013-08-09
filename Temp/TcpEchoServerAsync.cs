using System;              // For Console, IAsyncResult, ArgumentException
using System.Net;          // For IPEndPoint
using System.Net.Sockets;  // For Socket
using System.Threading;    // For ManualResetEvent

class ClientState {
  // Object to contain client state, including the client socket
  // and the receive buffer

  private const int BUFSIZE = 32; // Size of receive buffer
  private byte[] rcvBuffer;
  private Socket clntSock;

  public ClientState(Socket clntSock) {
    this.clntSock = clntSock;
    rcvBuffer = new byte[BUFSIZE]; // Receive buffer
  }

  public byte[] RcvBuffer { 
    get {
      return rcvBuffer; 
    }
  }

  public Socket ClntSock { 
    get {
      return clntSock; 
    }
  }
}

class TcpEchoServerAsync {

  private const int BACKLOG = 5;  // Outstanding connection queue max size

  static void Main(string[] args) {

    if (args.Length != 1) // Test for correct # of args
      throw new ArgumentException("Parameters: <Port>");
  
    int servPort = Int32.Parse(args[0]);
 
    // Create a Socket to accept client connections
    Socket servSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                                 ProtocolType.Tcp);

    servSock.Bind(new IPEndPoint(IPAddress.Any, servPort));
    servSock.Listen(BACKLOG);

    for (;;) { // Run forever, accepting and servicing connections
      Console.WriteLine("Thread {0} ({1}) - Main(): calling BeginAccept()",
                        Thread.CurrentThread.GetHashCode(), 
                        Thread.CurrentThread.ThreadState);
                        
      IAsyncResult result = servSock.BeginAccept(new AsyncCallback(AcceptCallback), 
                                                 servSock); 
      doOtherStuff();

      // Wait for the EndAccept before issuing a new BeginAccept 
      result.AsyncWaitHandle.WaitOne();
    }
  }
 
  public static void doOtherStuff() {
    for (int x=1; x<=5; x++) {
      Console.WriteLine("Thread {0} ({1}) - doOtherStuff(): {2}...", 
                        Thread.CurrentThread.GetHashCode(), 
                        Thread.CurrentThread.ThreadState, x);
      Thread.Sleep(1000);
    }
  }
 
  public static void AcceptCallback(IAsyncResult asyncResult) {

    Socket servSock = (Socket) asyncResult.AsyncState;
    Socket clntSock = null;
 
    try {

      clntSock = servSock.EndAccept(asyncResult);

      Console.WriteLine("Thread {0} ({1}) - AcceptCallback(): handling client at {2}",
                        Thread.CurrentThread.GetHashCode(), 
                        Thread.CurrentThread.ThreadState,
                        clntSock.RemoteEndPoint);

      ClientState cs = new ClientState(clntSock);

      clntSock.BeginReceive(cs.RcvBuffer, 0, cs.RcvBuffer.Length, SocketFlags.None, 
                            new AsyncCallback(ReceiveCallback), cs);
     } catch (SocketException se) {
      Console.WriteLine(se.ErrorCode + ": " + se.Message);
      clntSock.Close();
     }
  }

  public static void ReceiveCallback(IAsyncResult asyncResult) {

    ClientState cs = (ClientState) asyncResult.AsyncState;

    try {

      int recvMsgSize = cs.ClntSock.EndReceive(asyncResult);

      if (recvMsgSize > 0) {
        Console.WriteLine("Thread {0} ({1}) - ReceiveCallback(): received {2} bytes",
                          Thread.CurrentThread.GetHashCode(), 
                          Thread.CurrentThread.ThreadState,
                          recvMsgSize);

        cs.ClntSock.BeginSend(cs.RcvBuffer, 0, recvMsgSize, SocketFlags.None,
                              new AsyncCallback(SendCallback), cs);
      } else {
        cs.ClntSock.Close();
      }
    } catch (SocketException se) {
      Console.WriteLine(se.ErrorCode + ": " + se.Message);
      cs.ClntSock.Close();
    }
  }

  public static void SendCallback(IAsyncResult asyncResult) {
    ClientState cs = (ClientState) asyncResult.AsyncState;

    try {

      int bytesSent = cs.ClntSock.EndSend(asyncResult);

      Console.WriteLine("Thread {0} ({1}) - SendCallback(): sent {2} bytes",
                        Thread.CurrentThread.GetHashCode(), 
                        Thread.CurrentThread.ThreadState,
                        bytesSent);

      cs.ClntSock.BeginReceive(cs.RcvBuffer, 0, cs.RcvBuffer.Length, SocketFlags.None, 
                               new AsyncCallback(ReceiveCallback), cs);
    } catch (SocketException se) {
      Console.WriteLine(se.ErrorCode + ": " + se.Message);
      cs.ClntSock.Close();
    }
  }
}
