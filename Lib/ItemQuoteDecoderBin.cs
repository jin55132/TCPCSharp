using System;       // For String, Activator
using System.IO;    // For Stream
using System.Text;  // For Encoding
using System.Net;   // For IPAddress

public class ItemQuoteDecoderBin : ItemQuoteDecoder {

  public Encoding encoding; // Character encoding

  public ItemQuoteDecoderBin() : this (ItemQuoteTextConst.DEFAULT_CHAR_ENC) {
  }

  public ItemQuoteDecoderBin(String encodingDesc) {
    encoding = Encoding.GetEncoding(encodingDesc);
  }

  public ItemQuote decode(Stream wire) {
    BinaryReader src = new BinaryReader(new BufferedStream(wire));

    long itemNumber = IPAddress.NetworkToHostOrder(src.ReadInt64());
    int quantity = IPAddress.NetworkToHostOrder(src.ReadInt32());
    int unitPrice = IPAddress.NetworkToHostOrder(src.ReadInt32());
    byte flags = src.ReadByte();

    int stringLength = src.Read(); // Returns an unsigned byte as an int
    if (stringLength == -1)
      throw new EndOfStreamException();
    byte[] stringBuf = new byte[stringLength];
    src.Read(stringBuf, 0, stringLength);
    String itemDesc = encoding.GetString(stringBuf);

    return new ItemQuote(itemNumber,itemDesc, quantity, unitPrice,
      ((flags & ItemQuoteBinConst.DISCOUNT_FLAG) == ItemQuoteBinConst.DISCOUNT_FLAG),
      ((flags & ItemQuoteBinConst.IN_STOCK_FLAG) == ItemQuoteBinConst.IN_STOCK_FLAG));
  }

  public ItemQuote decode(byte[] packet) {
    Stream payload = new MemoryStream(packet, 0, packet.Length, false);
    return decode(payload);
  }
}
