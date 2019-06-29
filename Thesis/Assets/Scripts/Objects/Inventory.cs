using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public sealed class Inventory : MonoBehaviour
{
    [SerializeField]
    private uint initialCapacity;

    public uint Capacity { get; private set; }

    private Dictionary<string, InventoryItem> inventoryObjects;

    private void Awake()
    {
        Capacity = initialCapacity;

        inventoryObjects = new Dictionary<string, InventoryItem>((int)Capacity);
    }

    public void AddItem(string identifier, IInventoriable collectable)
    {
        if(collectable == null)
        {
            throw new ArgumentNullException("collectable object was null");
        }

        inventoryObjects.Add(identifier, new InventoryItem() { inventoriable = collectable });
    }

    public IInventoriable Peek(string identifier)
    {
        inventoryObjects.TryGetValue(identifier, out InventoryItem value);

        return value.inventoriable;
    }

    public void LockItem(string identifier, bool @lock)
    {
        if(inventoryObjects.TryGetValue(identifier, out InventoryItem value))
        {
            value.locked = @lock;
        }
    }

    public IInventoriable RemoveItem(string identifier)
    {
        if(inventoryObjects.TryGetValue(identifier, out InventoryItem value) && value.locked == false)
        {
            inventoryObjects.Remove(identifier);

            return value.inventoriable;
        }

        return default;
    }

    

    private struct InventoryItem
    {
        public IInventoriable inventoriable;
        public bool locked;
    }
}

public interface IInventoriable : ICollectable
{

}
