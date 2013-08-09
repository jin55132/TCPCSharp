using System;  // For String and Boolean

public class ItemQuote {

  public long itemNumber;         // Item identification number
  public String itemDescription;  // String description of item
  public int quantity;            // Number of items in quote (always >= 1)
  public int unitPrice;           // Price (in cents) per item
  public Boolean discounted;      // Price reflect a discount?
  public Boolean inStock;         // Item(s) ready to ship?

  public ItemQuote() {}

  public ItemQuote(long itemNumber, String itemDescription,
                   int quantity, int unitPrice, Boolean discounted, Boolean inStock) {
    this.itemNumber      = itemNumber;
    this.itemDescription = itemDescription;
    this.quantity        = quantity;
    this.unitPrice       = unitPrice;
    this.discounted      = discounted;
    this.inStock         = inStock;
  }
 
  public override String ToString() {
    String EOLN = "\n";
    String value = "Item# = " + itemNumber + EOLN +
                   "Description = " + itemDescription + EOLN +
                   "Quantity = " + quantity + EOLN +
                   "Price (each) = " + unitPrice + EOLN +
                   "Total Price = " + (quantity * unitPrice);

    if (discounted) 
      value += " (discounted)";
    if (inStock)
      value += EOLN + "In Stock" + EOLN;
    else 
      value += EOLN + "Out of Stock" + EOLN;

    return value;
  }
}
