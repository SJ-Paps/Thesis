using System;
using System.Collections.Generic;

public sealed class Blackboard : IBlackboard
{
    private Dictionary<string, BlackboardNode> nodes;

    public Blackboard()
    {
        nodes = new Dictionary<string, BlackboardNode>();
    }

    public BlackboardNode<T> AddItem<T>(in string identifier, T value)
    {
        if(nodes.ContainsKey(identifier) == false)
        {
            BlackboardNode<T> node = new BlackboardNode<T>(value);

            nodes.Add(identifier, node);

            return node;
        }

        return null;
    }

    public BlackboardNode<T> GetItemNodeOf<T>(in string identifier)
    {
        if(nodes.TryGetValue(identifier, out BlackboardNode node))
        {
            if(node == null)
            {
                var newNode = new BlackboardNode<T>(default);
                nodes[identifier] = newNode;
                return newNode;
            }
            else if(node is BlackboardNode<T> castNode)
            {
                return castNode;
            }
        }

        return null;
    }

    public T GetItemOf<T>(in string identifier)
    {
        BlackboardNode<T> node = GetItemNodeOf<T>(in identifier);

        if(node != null)
        {
            return node.GetValue();
        }

        return default;
    }

    public object GetItemOf(in string identifier)
    {
        if (nodes.TryGetValue(identifier, out BlackboardNode node) && node != null)
        {
            return node.GetValueObject();
        }

        return null;
    }

    public void RemoveItem(in string identifier)
    {
        nodes.Remove(identifier);
    }

    public void AddListenerToValueChangedTo<T>(in string identifier, Action<T> listener)
    {
        if(nodes.TryGetValue(identifier, out BlackboardNode node) && node != null && node is BlackboardNode<T> castNode)
        {
            castNode.onValueChanged += listener;
        }
    }

    public void RemoveListener<T>(in string identifier, Action<T> listener)
    {
        if (nodes.TryGetValue(identifier, out BlackboardNode node) && node != null && node is BlackboardNode<T> castNode)
        {
            castNode.onValueChanged -= listener;
        }
    }

    public void CreateSlot(in string identifier)
    {
        nodes.Add(identifier, null);
    }

    public BlackboardNode<T> UpdateItem<T>(in string identifier, T newValue)
    {
        if(nodes.TryGetValue(identifier, out BlackboardNode node))
        {
            if(node == null)
            {
                var newNode = new BlackboardNode<T>(newValue);
                nodes[identifier] = newNode;
                return newNode;
            }
            else if(node is BlackboardNode<T> castNode)
            {
                castNode.SetValue(newValue);
                return castNode;
            }
        }

        return AddItem<T>(in identifier, newValue);
    }
}
