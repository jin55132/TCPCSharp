using System;       // For String, Activator
using System.IO;    // For IOException
using System.Text;  // For Encoding

public class ItemQuoteEncoderText : ItemQuoteEncoder {

  public Encoding encoding; // Character encoding

  public ItemQuoteEncoderText() : this(ItemQuoteTextConst.DEFAULT_CHAR_ENC) {
  }

  public ItemQuoteEncoderText(string encodingDesc) {
    encoding = Encoding.GetEncoding(encodingDesc);
  }

  public byte[] encode(ItemQuote item) {
    
    String EncodedString = item.itemNumber + " "; 
    if (item.itemDescription.IndexOf('\n') != -1) 
      throw new IOException("Invalid description (contains newline)");
    EncodedString = EncodedString + item.itemDescription + "\n"; 
    EncodedString = EncodedString + item.quantity + " "; 
    EncodedString = EncodedString + item.unitPrice + " "; 

    if (item.discounted) 
      EncodedString = EncodedString + "d"; // Only include 'd' if discounted 
    if (item.inStock) 
      EncodedString = EncodedString + "s"; // Only include 's' if in stock 
    EncodedString = EncodedString + "\n"; 

    if (EncodedString.Length > ItemQuoteTextConst.MAX_WIRE_LENGTH) 
      throw new IOException("Encoded length too long");
    
    byte[] buf = encoding.GetBytes(EncodedString);

    return buf;
     
  }
}
