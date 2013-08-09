using System;              // For String
using System.Collections;  // For ArrayList
using System.Threading;    // For Mutex

class ConsoleLogger : ILogger {
  private static Mutex mutex = new Mutex();

  public void writeEntry(ArrayList entry) {
    mutex.WaitOne();

    IEnumerator line = entry.GetEnumerator();
    while (line.MoveNext())
       Console.WriteLine(line.Current);

    Console.WriteLine();

    mutex.ReleaseMutex();
  }

  public void writeEntry(String entry) {
    mutex.WaitOne();

    Console.WriteLine(entry);
    Console.WriteLine();

    mutex.ReleaseMutex();
  }
}
