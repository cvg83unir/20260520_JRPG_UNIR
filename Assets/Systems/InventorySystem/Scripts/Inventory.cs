using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public UnityEvent<InventoryInfo> onObjectUsed;

    public void NotifyObjectUsed(InventoryInfo invInfo)
    {
        this.onObjectUsed.Invoke(invInfo);
    }

}
