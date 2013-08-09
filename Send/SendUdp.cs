using System;              // For String, Int32, ArgumentException
using System.Net.Sockets;  // For UdpClient

class SendUdp {

  //static void Main(string[] args) {

  //  if (args.Length != 2 && args.Length != 3)  // Test for correct # of args
  //    throw new ArgumentException("Parameter(s): <Destination>" +
  //                                       " <Port> [<encoding]");

  //  String server = args[0];             // Server name or IP address
  //  int destPort = Int32.Parse(args[1]); // Destination port

  //  ItemQuote quote = new ItemQuote(1234567890987654L, "5mm Super Widgets",
  //                                  1000, 12999, true, false);

  //  UdpClient client = new UdpClient(); // UDP socket for sending

  //  ItemQuoteEncoder encoder = (args.Length == 3 ?
  //                              new ItemQuoteEncoderText(args[2]) :
  //                              new ItemQuoteEncoderText());

  //  byte[] codedQuote = encoder.encode(quote);

  //  client.Send(codedQuote, codedQuote.Length, server, destPort); 

  //  client.Close();
  //}
}
