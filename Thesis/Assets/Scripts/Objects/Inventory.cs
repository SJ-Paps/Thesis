using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public sealed class Inventory : SJMonoBehaviour
{
    public int Capacity
    {
        get
        {
            return inventoryObjects.Count;
        }
    }

    private Dictionary<string, InventoryItem> inventoryObjects;

    protected override void Awake()
    {
        base.Awake();

        inventoryObjects = new Dictionary<string, InventoryItem>();
    }

    public void AddItem(in string identifier, IInventoriable collectable)
    {
        if (collectable == null)
        {
            throw new ArgumentNullException("collectable object was null");
        }

        inventoryObjects.Add(identifier, new InventoryItem() { inventoriable = collectable });
    }

    public IInventoriable Peek(in string identifier)
    {
        inventoryObjects.TryGetValue(identifier, out InventoryItem value);

        return value.inventoriable;
    }

    public void LockItem(in string identifier, bool @lock)
    {
        if (inventoryObjects.TryGetValue(identifier, out InventoryItem value))
        {
            value.locked = @lock;
        }
    }

    public IInventoriable RemoveItem(in string identifier)
    {
        if (inventoryObjects.TryGetValue(identifier, out InventoryItem value) && value.locked == false)
        {
            inventoryObjects.Remove(identifier);

            return value.inventoriable;
        }

        return default;
    }

    public bool Contains(IInventoriable inventoriable)
    {
        foreach (KeyValuePair<string, InventoryItem> item in inventoryObjects)
        {
            if (item.Value.inventoriable == inventoriable)
            {
                return true;
            }
        }

        return false;
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
