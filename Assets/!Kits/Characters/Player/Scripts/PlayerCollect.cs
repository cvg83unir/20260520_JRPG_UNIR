using UnityEngine;
using UnityEngine.Events;

public class PlayerCollect : MonoBehaviour
{
    [SerializeField] GameObject inventoryItemUIPrefab;
    [SerializeField] Transform itemsParent;

    [SerializeField] InventoryInfo[] startingObjects;

    public UnityEvent <CollectableObject>onCollectedObjectDirectUsage;

    Inventory inventory;

    private void Awake()
    {
        this.inventory = GetComponent<Inventory>();

        foreach(InventoryInfo startingObjectInvInfo in this.startingObjects)
        {
            AddObjectToInventory(startingObjectInvInfo);
        }
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
                        AddObjectToInventory(collectable.PropInventoryInfo);
                    }

                    break;
            }
            
            collectable.NotifyCollected();
        }

    }

    private void AddObjectToInventory(InventoryInfo invInfo)
    {
        

        foreach (InventoryItem go in itemsParent.GetComponentsInChildren<InventoryItem>())
        {
            if (go.compareItem(invInfo))
            {
                go.addUsage(invInfo.remainingUseCount);
                return;
            }
        }

        GameObject newItem = Instantiate(inventoryItemUIPrefab, itemsParent);
        newItem.GetComponent<InventoryItem>().Initialize(this.inventory, invInfo);
    }

}
