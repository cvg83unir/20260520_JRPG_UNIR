using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{

    public UnityEvent<InventoryInfo> onObjectUsed;

    //List<InventoryInfo> itemsInInventory = new List<InventoryInfo>();

    public void NotifyObjectUsed(InventoryInfo invInfo)
    {
        this.onObjectUsed.Invoke(invInfo);
    }/*
    public void addItem(InventoryInfo invinf)
    {
        itemsInInventory.Add(invinf);
    }
    public void removeItem(InventoryInfo invinf)
    {
        itemsInInventory.Remove(invinf);
    }
    public bool checkItem(InventoryInfo invinf)
    {
        return itemsInInventory.Contains(invinf);
    }*/

}
