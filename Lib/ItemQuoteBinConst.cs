using System;   // For String

public class ItemQuoteBinConst {
  public static readonly String DEFAULT_CHAR_ENC = "ascii";

  public static readonly byte DISCOUNT_FLAG   = 1 << 7;
  public static readonly byte IN_STOCK_FLAG   = 1 << 0;
  public static readonly int  MAX_DESC_LEN    = 255;
  public static readonly int  MAX_WIRE_LENGTH = 1024;
}
