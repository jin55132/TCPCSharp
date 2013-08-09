using System;              // For String
using System.IO;           // For StreamWriter
using System.Threading;    // For Mutex
using System.Collections;  // For ArrayList

class FileLogger : ILogger {
  private static Mutex mutex = new Mutex();

  private StreamWriter output;  // Log file

  public FileLogger(String filename) {
    // Create log file
    output = new StreamWriter(filename, true); 
  }

  public void writeEntry(ArrayList entry) {
    mutex.WaitOne();

    IEnumerator line = entry.GetEnumerator(); 
    while (line.MoveNext())
      output.WriteLine(line.Current);
    output.WriteLine();
    output.Flush();

    mutex.ReleaseMutex();
  }

  public void writeEntry(String entry) {
    mutex.WaitOne();

    output.WriteLine(entry);
    output.WriteLine();
    output.Flush();

    mutex.ReleaseMutex();
  }
}
