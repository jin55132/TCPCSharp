using System;              // For String
using System.Collections;  // For ArrayList

public interface ILogger {
  void writeEntry(ArrayList entry); // Write list of lines
  void writeEntry(String entry);    // Write single line
}
