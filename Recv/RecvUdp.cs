using System;              // For Int32, ArgumentException
using System.Net;          // For IPEndPoint
using System.Net.Sockets;  // For UdpClient

class RecvUdp {

  //static void Main(string[] args) {

  //  if (args.Length != 1 && args.Length != 2)  // Test for correct # of args
  //     throw new ArgumentException("Parameter(s): <Port> [<encoding>]");

  //  int port = Int32.Parse(args[0]);   // Receiving Port

  //  UdpClient client = new UdpClient(port); // UDP socket for receiving

  //  byte[] packet = new byte[ItemQuoteTextConst.MAX_WIRE_LENGTH];
  //  IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, port);

  //  packet = client.Receive(ref remoteIPEndPoint);

  //  ItemQuoteDecoderText decoder = (args.Length == 2 ?   // Which encoding
  //                                  new ItemQuoteDecoderText(args[1]) :
  //                                  new ItemQuoteDecoderText() );

  //  ItemQuote quote = decoder.decode(packet);
  //  Console.WriteLine(quote);

  //  client.Close();
  //}
}
