using System;
using System.Collections.Generic;

public class BPlusTree
{
    private class Node
    {
        public List<int> Keys { get; set; }
        public List<Node> Children { get; set; }
        public Node Parent { get; set; }

        public Node()
        {
            Keys = new List<int>();
            Children = new List<Node>();
            Parent = null;
        }
    }

    private int order;
    private Node root;

    public BPlusTree(int order)
    {
        this.order = order;
        this.root = null;
    }

    public void Insert(int key)
    {
        if (root == null)
        {
            // Case 1: Tree is empty
            root = new Node();
            root.Keys.Add(key);
        }
        else
        {
            InsertKey(root, key);
        }
    }

    private void InsertKey(Node node, int key)
    {
        if (node.Children.Count == 0)
        {
            // Leaf Node
            InsertIntoLeaf(node, key);
        }
        else
        {
            // Internal Node
            int index = FindChildIndex(node, key);
            InsertKey(node.Children[index], key);
        }
    }

    private void InsertIntoLeaf(Node leaf, int key)
    {
        if (leaf.Keys.Count < order - 1)
        {
            // Case 1: Leaf is not full
            InsertInIncreasingOrder(leaf.Keys, key);
        }
        else
        {
            // Case 2: Leaf is full
            InsertInIncreasingOrder(leaf.Keys, key);

            Node newLeaf = SplitLeaf(leaf);

            if (leaf.Parent == null)
            {
                // Create a new root
                Node newRoot = new Node();
                newRoot.Keys.Add(newLeaf.Keys[0]);
                newRoot.Children.Add(leaf);
                newRoot.Children.Add(newLeaf);
                leaf.Parent = newRoot;
                newLeaf.Parent = newRoot;
                root = newRoot;
            }
            else
            {
                // Insert key into the parent node
                InsertKeyIntoParent(leaf.Parent, newLeaf.Keys[0], leaf, newLeaf);
            }
        }
    }

    private void InsertKeyIntoParent(Node parent, int key, Node leftChild, Node rightChild)
    {
        if (parent.Keys.Count < order - 1)
        {
            // Case 1: Parent is not full
            int index = FindChildIndex(parent, key);
            parent.Keys.Insert(index, key);
            parent.Children.Insert(index + 1, rightChild);
            rightChild.Parent = parent;
        }
        else
        {
            // Case 2: Parent is full
            int index = FindChildIndex(parent, key);
            parent.Keys.Insert(index, key);
            parent.Children.Insert(index + 1, rightChild);

            Node newParent = SplitInternalNode(parent);

            if (parent.Parent == null)
            {
                // Create a new root
                Node newRoot = new Node();
                newRoot.Keys.Add(newParent.Keys[0]);
                newRoot.Children.Add(parent);
                newRoot.Children.Add(newParent);
                parent.Parent = newRoot;
                newParent.Parent = newRoot;
                root = newRoot;
            }
            else
            {
                // Insert key into the parent node recursively
                InsertKeyIntoParent(parent.Parent, newParent.Keys[0], parent, newParent);
            }
        }
    }

    private void InsertInIncreasingOrder(List<int> list, int key)
    {
        int index = 0;
        while (index < list.Count && list[index] < key)
        {
            index++;
        }
        list.Insert(index, key);
    }

    private Node SplitLeaf(Node node)
    {
        int midIndex = node.Keys.Count / 2;
        Node newLeaf = new Node();

        for (int i = midIndex; i < node.Keys.Count; i++)
        {
            newLeaf.Keys.Add(node.Keys[i]);
        }

        node.Keys.RemoveRange(midIndex, node.Keys.Count - midIndex);

        if (node.Parent != null)
        {
            int index = node.Parent.Children.IndexOf(node);
            node.Parent.Children.Insert(index + 1, newLeaf);
            newLeaf.Parent = node.Parent;
        }

        return newLeaf;
    }

    private Node SplitInternalNode(Node node)
    {
        int midIndex = node.Keys.Count / 2;
        Node newInternal = new Node();

        for (int i = midIndex + 1; i < node.Keys.Count; i++)
        {
            newInternal.Keys.Add(node.Keys[i]);
            newInternal.Children.Add(node.Children[i]);
            node.Children[i].Parent = newInternal;
        }

        newInternal.Children.Add(node.Children[node.Children.Count - 1]);
        node.Children[node.Children.Count - 1].Parent = newInternal;

        node.Keys.RemoveRange(midIndex, node.Keys.Count - midIndex);
        node.Children.RemoveRange(midIndex + 1, node.Children.Count - midIndex - 1);

        if (node.Parent != null)
        {
            int index = node.Parent.Children.IndexOf(node);
            node.Parent.Children.Insert(index + 1, newInternal);
            newInternal.Parent = node.Parent;
        }

        return newInternal;
    }

    private int FindChildIndex(Node node, int key)
    {
        int index = 0;
        while (index < node.Keys.Count && key > node.Keys[index])
        {
            index++;
        }
        return index;
    }

    public void PrintTree()
    {
        if (root != null)
        {
            PrintNode(root, 0);
        }
    }

    private void PrintNode(Node node, int level)
    {
        Console.Write("Level " + level + ": ");
        foreach (int key in node.Keys)
        {
            Console.Write(key + " ");
        }
        Console.WriteLine();

        foreach (Node child in node.Children)
        {
            PrintNode(child, level + 1);
        }
    }
}

class Program
{
    static void Main()
    {
        BPlusTree bPlusTree = new BPlusTree(4);

        // Insert keys into the B+ tree
        int[] keys = { 1, 4, 7, 10, 17, 21, 31 };
        foreach (int key in keys)
        {
            bPlusTree.Insert(key);
        }

        // Print the B+ tree
        bPlusTree.PrintTree();
    }
}
