using System;     // For Boolean
using System.IO;  // For Stream

public class Framer {

  public static byte[] nextToken(Stream input, byte[] delimiter) {
    int nextByte;

    // If the stream has already ended, return null
    if ((nextByte = input.ReadByte()) == -1)
      return null;

    MemoryStream tokenBuffer = new MemoryStream();
    do {
      tokenBuffer.WriteByte((byte)nextByte);
      byte[] currentToken = tokenBuffer.ToArray();
      if (endsWith(currentToken, delimiter)) {
        int tokenLength = currentToken.Length - delimiter.Length;
        byte[] token = new byte[tokenLength];
        Array.Copy(currentToken, 0, token, 0, tokenLength); 
        return token;
      }
    } while ((nextByte = input.ReadByte()) != -1);   // Stop on EOS
    return tokenBuffer.ToArray();         // Received at least one byte
  }

  // Returns true if value ends with the bytes in the suffix
  private static Boolean endsWith(byte[] value, byte[] suffix) {
    if (value.Length < suffix.Length)
      return false;

    for (int offset=1; offset <= suffix.Length; offset++) 
      if (value[value.Length - offset] != suffix[suffix.Length - offset])
        return false; 

    return true;
  }
}
