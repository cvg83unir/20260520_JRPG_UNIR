using UnityEngine;

[CreateAssetMenu]
public class InventoryInfo : ScriptableObject
{

    public enum InventoryObjectType
    {
        Health,
        Magic,
        Ammo,
    }

    public enum UsageType
    {
        Direct,
        InInventory
    }

    public InventoryObjectType type;
    public UsageType usage;
    public float recovery = 1f;
    public Sprite spriteImage;
    public int remainingUseCount = 3;
    public string infoText;

}
