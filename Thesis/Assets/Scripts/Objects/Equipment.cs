using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public sealed class Equipment : SJMonoBehaviour
{
    public int SlotsCount
    {
        get
        {
            return equipmentSlots.Count;
        }
    }

    private Dictionary<string, EquipmentSlot> equipmentSlots;
    
    protected override void Awake()
    {
        base.Awake();

        equipmentSlots = new Dictionary<string, EquipmentSlot>();
    }

    public void AddEquipmentSlot(in string identifier)
    {
        equipmentSlots.Add(identifier, default);
    }

    public void AddEquipmentSlot(in string identifier, IEquipable equipable)
    {
        AddEquipmentSlot(in identifier);
        SetObjectAtSlot(in identifier, equipable);
    }

    public void RemoveEquipmentSlot(in string identifier)
    {
        if(equipmentSlots.TryGetValue(identifier, out EquipmentSlot slot) && slot.InUse == false)
        {
            equipmentSlots.Remove(identifier);
        }
    }

    public void SetObjectAtSlot(in string identifier, IEquipable equipable)
    {
        if(equipable == null)
        {
            throw new ArgumentNullException("equipable object was null");
        }

        if(equipmentSlots.ContainsKey(identifier))
        {
            equipmentSlots[identifier] = new EquipmentSlot() { equipable = equipable};
        }
    }

    public void ReleaseSlot(in string identifier)
    {
        if (equipmentSlots.ContainsKey(identifier))
        {
            equipmentSlots[identifier] = default;
        }
    }

    public IEquipable GetEquipable(in string identifier)
    {
        if(equipmentSlots.TryGetValue(identifier, out EquipmentSlot slot) && slot.InUse)
        {
            return slot.equipable;
        }

        return default;
    }

    public bool IsSlotInUse(in string identifier)
    {
        if(equipmentSlots.TryGetValue(identifier, out EquipmentSlot slot))
        {
            return slot.InUse;
        }

        return false;
    }

    public bool IsObjectEquipped(IEquipable equipable)
    {
        foreach(KeyValuePair<string, EquipmentSlot> slot in equipmentSlots)
        {
            if(slot.Value.equipable == equipable)
            {
                return true;
            }
        }

        return false;
    }

    public bool HasEquippedObjectOfType<T>()
    {
        foreach (KeyValuePair<string, EquipmentSlot> slot in equipmentSlots)
        {
            if (slot.Value.equipable is T)
            {
                return true;
            }
        }

        return false;
    }

    public bool HasEquippedObjectOfType<T>(out T obj)
    {
        foreach (KeyValuePair<string, EquipmentSlot> slot in equipmentSlots)
        {
            if (slot.Value.equipable is T tValue)
            {
                obj = tValue;

                return true;
            }
        }

        obj = default;

        return false;
    }

    public bool HasSomethingEquipped()
    {
        foreach (KeyValuePair<string, EquipmentSlot> slot in equipmentSlots)
        {
            if (slot.Value.InUse)
            {
                return true;
            }
        }

        return false;
    }

    private struct EquipmentSlot
    {
        public IEquipable equipable;
        public bool InUse
        {
            get
            {
                return equipable != null;
            }
        }
    }
}
