using System;              // For String, IAsyncResult, ArgumentException
using System.Text;         // For Encoding
using System.Net.Sockets;  // For TcpClient, NetworkStream
using System.Threading;    // For ManualResetEvent

class ClientState  {
  // Object to contain client state, including the network stream
  // and the send/recv buffer

  private byte[] byteBuffer;
  private NetworkStream netStream;
  private StringBuilder echoResponse;
  private int totalBytesRcvd = 0; // Total bytes received so far

  public ClientState(NetworkStream netStream, byte[] byteBuffer) {
    this.netStream = netStream;
    this.byteBuffer = byteBuffer;
    echoResponse = new StringBuilder();
  }

  public NetworkStream NetStream {
    get { 
       return netStream;
    }
  }

  public byte[] ByteBuffer {
    set {
      byteBuffer = value;
    }
    get {
      return byteBuffer;
    }
  }
  
  public void AppendResponse(String response) {
    echoResponse.Append(response);
  }
  public String EchoResponse {
    get {
      return echoResponse.ToString();
    }
  }

  public void AddToTotalBytes(int count) {
    totalBytesRcvd += count;
  }
  public int TotalBytes {
    get {
      return totalBytesRcvd;
    }
  }
}

class TcpEchoClientAsync {

  // A manual event signal we will trigger when all reads are complete:
  public static ManualResetEvent ReadDone = new ManualResetEvent(false);

  static void Main(string[] args) {

    if ((args.Length < 2) || (args.Length > 3)) { // Test for correct # of args
      throw new ArgumentException("Parameters: <Server> <Word> [<Port>]");
    }

    String server = args[0]; // Server name or IP address

    // Use port argument if supplied, otherwise default to 7
    int servPort = (args.Length == 3) ? Int32.Parse(args[2]) : 7;

    Console.WriteLine("Thread {0} ({1}) - Main()", Thread.CurrentThread.GetHashCode(), 
                      Thread.CurrentThread.ThreadState);

    // Create TcpClient that is connected to server on specified port
    TcpClient client = new TcpClient();

    client.Connect(server, servPort);
    Console.WriteLine("Thread {0} ({1}) - Main(): connected to server", 
                      Thread.CurrentThread.GetHashCode(), 
                      Thread.CurrentThread.ThreadState);

    NetworkStream netStream = client.GetStream();
    ClientState cs = new ClientState(netStream, Encoding.ASCII.GetBytes(args[1]));

    // Send the encoded string to the server
    IAsyncResult result = netStream.BeginWrite(cs.ByteBuffer, 0, 
                                               cs.ByteBuffer.Length, 
                                               new AsyncCallback(WriteCallback), 
                                               cs);

    doOtherStuff();

    result.AsyncWaitHandle.WaitOne(); // block until EndWrite is called

    // Receive the same string back from the server
    result = netStream.BeginRead(cs.ByteBuffer, cs.TotalBytes, 
                                 cs.ByteBuffer.Length - cs.TotalBytes,
                                 new AsyncCallback(ReadCallback), cs);

    doOtherStuff();

    ReadDone.WaitOne(); // Block until ReadDone is manually set

    netStream.Close();  // Close the stream
    client.Close();     // Close the socket
  }
  
  public static void doOtherStuff() {
    for (int x=1; x<=5; x++) {
      Console.WriteLine("Thread {0} ({1}) - doOtherStuff(): {2}...", 
                        Thread.CurrentThread.GetHashCode(), 
                        Thread.CurrentThread.ThreadState, x);
      Thread.Sleep(1000);
    }
  }
 
  public static void WriteCallback(IAsyncResult asyncResult) {

    ClientState cs = (ClientState) asyncResult.AsyncState;

    cs.NetStream.EndWrite(asyncResult);
    Console.WriteLine("Thread {0} ({1}) - WriteCallback(): Sent {2} bytes...", 
                      Thread.CurrentThread.GetHashCode(),
                      Thread.CurrentThread.ThreadState, cs.ByteBuffer.Length);
  }

  public static void ReadCallback(IAsyncResult asyncResult) {

    ClientState cs = (ClientState) asyncResult.AsyncState;
    
    int bytesRcvd = cs.NetStream.EndRead(asyncResult);

    cs.AddToTotalBytes(bytesRcvd);
    cs.AppendResponse(Encoding.ASCII.GetString(cs.ByteBuffer, 0, bytesRcvd));

    if (cs.TotalBytes < cs.ByteBuffer.Length) {
      Console.WriteLine("Thread {0} ({1}) - ReadCallback(): Received {2} bytes...", 
                        Thread.CurrentThread.GetHashCode(),
                        Thread.CurrentThread.ThreadState, bytesRcvd);
      cs.NetStream.BeginRead(cs.ByteBuffer, cs.TotalBytes, 
                             cs.ByteBuffer.Length - cs.TotalBytes,
                             new AsyncCallback(ReadCallback), cs.NetStream);
    } else {
      Console.WriteLine("Thread {0} ({1}) - ReadCallback(): Received {2} total " + 
                        "bytes: {3}", 
                        Thread.CurrentThread.GetHashCode(),
                        Thread.CurrentThread.ThreadState, cs.TotalBytes, 
                        cs.EchoResponse);
      ReadDone.Set(); // Signal read complete event 
    }
  }
}
