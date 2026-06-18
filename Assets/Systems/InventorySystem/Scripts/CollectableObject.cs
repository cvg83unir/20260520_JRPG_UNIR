using System;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{

    [SerializeField]InventoryInfo inventoryInfo;

    /// <summary>
    /// Propiedad de sólo lectura para que otros objetos obtengan el inventoryinfo
    /// </summary>
    public InventoryInfo PropInventoryInfo
    {
        get => this.inventoryInfo;
    }

    internal void NotifyCollected()
    {
        Destroy(gameObject);
    }
}
