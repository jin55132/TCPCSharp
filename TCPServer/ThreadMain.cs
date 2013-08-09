using System;              // For String, Int32, Activator
using System.Net;          // For IPAddress
using System.Net.Sockets;  // For TcpListener

class ThreadMain {

  static void Main(string[] args) {

    if (args.Length != 3)  // Test for correct # of args
      throw new ArgumentException("Parameter(s): [<Optional properties>]"
                                  + " <Port> <Protocol> <Dispatcher>");

    int servPort = Int32.Parse(args[0]);  // Server Port 
    String protocolName = args[1];        // Protocol name
    String dispatcherName = args[2];      // Dispatcher name

    TcpListener listener = new TcpListener(IPAddress.Any, servPort);
    listener.Start();

    ILogger logger = new ConsoleLogger();   // Log messages to console

    System.Runtime.Remoting.ObjectHandle objHandle = 
               Activator.CreateInstance(null, protocolName + "ProtocolFactory");
    IProtocolFactory protoFactory = (IProtocolFactory)objHandle.Unwrap(); 

    objHandle = Activator.CreateInstance(null, dispatcherName + "Dispatcher");
    IDispatcher dispatcher = (IDispatcher)objHandle.Unwrap(); 

    dispatcher.startDispatching(listener, logger, protoFactory);
    /* NOT REACHED */
  }
}
