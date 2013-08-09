using System.IO; // For Stream

public interface ItemQuoteDecoder {
  ItemQuote decode(Stream source);
  ItemQuote decode(byte[] packet);
}
