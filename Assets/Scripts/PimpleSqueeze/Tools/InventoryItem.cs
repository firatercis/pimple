[System.Serializable]
public class InventoryItem
{
    // Attributes
    public string itemName;
    public string prefsKey;
    public int cost;
    public int maxQuantity;
    public string description;
    // State variables
    public int quantity;
    public bool isBuyable = false;
    public bool isLocked = true;

    public void SetBuyable(int userMoney)
    {
        if (userMoney >= cost)
        {
            isBuyable = true;
        }
        else
        {
            isBuyable = false;
        }
    }

    public void SetUnlocked()
    {
        isLocked = false;
    }
}
