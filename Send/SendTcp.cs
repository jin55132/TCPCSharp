using System;              // For String, Console, ArgumentException
using System.Net.Sockets;  // For TcpClient, NetworkStream

class SendTcp {

  static void Main(string[] args) {

    if (args.Length != 2) // Test for correct # of args
      throw new ArgumentException("Parameters: <Destination> <Port>");

    String server = args[0];             // Destination address
    int servPort = Int32.Parse(args[1]); // Destination port

    // Create socket that is connected to server on specified port
    TcpClient client = new TcpClient(server, servPort);
    NetworkStream netStream = client.GetStream();

    ItemQuote quote = new ItemQuote(1234567890987654L, "5mm Super Widgets",
                                    1000, 12999, true, false);

    // Send text-encoded quote
    ItemQuoteEncoderText coder = new ItemQuoteEncoderText();
    byte[] codedQuote = coder.encode(quote);
    Console.WriteLine("Sending Text-Encoded Quote (" + 
                      codedQuote.Length + " bytes): ");
    Console.WriteLine(quote);

    netStream.Write(codedQuote, 0, codedQuote.Length);

    // Receive binary-encoded quote
    ItemQuoteDecoder decoder = new ItemQuoteDecoderBin();
    ItemQuote receivedQuote = decoder.decode(client.GetStream());
    Console.WriteLine("Received Binary-Encode Quote:");
    Console.WriteLine(receivedQuote);

    netStream.Close();
    client.Close();
  }
}
