using UnityEngine;
using UnityEngine.Events;

public class PlayerCollect : MonoBehaviour
{
    [SerializeField] GameObject inventoryItemUIPrefab;
    [SerializeField] Transform itemsParent;
    
    public UnityEvent <CollectableObject>onCollectedObjectDirectUsage;

    Inventory inventory;

    private void Awake()
    {
        this.inventory = GetComponent<Inventory>();
    }


    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        CollectableObject collectable = elOtro.GetComponent<CollectableObject>();

        if (collectable != null)
        {
            switch (collectable.PropInventoryInfo.usage)
            {
                case InventoryInfo.UsageType.Direct:
                    this.onCollectedObjectDirectUsage.Invoke(collectable);
                    break;
                case InventoryInfo.UsageType.InInventory:
                    {
                        GameObject newItem = Instantiate(inventoryItemUIPrefab, itemsParent);
                        newItem.GetComponent<InventoryItem>().Initialize(this.inventory, collectable.PropInventoryInfo);
                    }

                    break;
            }
            
            collectable.NotifyCollected();
        }

    }


}
