using System;              // For Console, Int32, ArgumentException
using System.Net;          // For IPAddress
using System.Net.Sockets;  // For TcpListener, TcpClient

class RecvTcp {

  static void Main(string[] args) {

    if (args.Length != 1) // Test for correct # of args
      throw new ArgumentException("Parameters: <Port>");
  
    int port = Int32.Parse(args[0]);

    // Create a TCPListener to accept client connections
    TcpListener listener = new TcpListener(IPAddress.Any, port);
    listener.Start();

    TcpClient client = listener.AcceptTcpClient();   // Get client connection

    // Receive text-encoded quote
    ItemQuoteDecoder decoder = new ItemQuoteDecoderText();
    ItemQuote quote = decoder.decode(client.GetStream());
    Console.WriteLine("Received Text-Encoded Quote:");
    Console.WriteLine(quote);

    // Repeat quote with binary-encoding, adding 10 cents to the price
    ItemQuoteEncoder encoder = new ItemQuoteEncoderBin();
    quote.unitPrice += 10;  // Add 10 cents to unit price
    Console.WriteLine("Sending (binary)...");
    byte[] bytesToSend = encoder.encode(quote);
    client.GetStream().Write(bytesToSend, 0 , bytesToSend.Length);

    client.Close();   
    listener.Stop();  
  }
}

