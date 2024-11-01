using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6_pia
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] orderValues = { 3, 5, 10, 33, 101, 333, 1001 };
            int[] nValues = { 100, 1000, 10000, 100000, 1000000 };

            foreach (int order in orderValues)
            {
                foreach (int n in nValues)
                {
                    Console.WriteLine($"Testovi ={order}, N={n}");
                    TestBPlus(order, n);
                    Console.WriteLine();
                }
            }
            //testiranja da li radi
            //bPlusTree.Insert(21);
            //bPlusTree.Insert(30);
            //bPlusTree.Insert(20);
            //bPlusTree.Insert(10);
            //bPlusTree.Insert(7);
            //bPlusTree.Insert(25);
            //bPlusTree.Insert(42);

            //bPlusTree.Insert(1);
            //bPlusTree.Insert(4);
            //bPlusTree.Insert(7);
            //bPlusTree.Insert(10);
            //bPlusTree.Insert(17);
            //bPlusTree.Insert(21);
            //bPlusTree.Insert(31);

            //bPlusTree.PrintTree();

            //bPlusTree.Delete(21);
            //Console.WriteLine("Posle brisanja:");
            //bPlusTree.PrintTree();

        }
        public static void TestBPlus(int order, int n)
        {
            Random random = new Random();
            BPlusTree bPlusTree = new BPlusTree(order);

            DateTime startInsert = DateTime.Now;
            // Simulacija dodavanja prirodnih brojeva u stablo
            for (int i = 1; i <= n; i++)
            {
                bPlusTree.Insert(i);
            }

            DateTime endInsert = DateTime.Now;
            TimeSpan insertTime = endInsert - startInsert;


            //DateTime startDelete = DateTime.Now;
            //// brisanja 10% vrednosti
            //int toDelete = (int)(n * 0.1);
            //for (int i = 1; i <= toDelete; i++)
            //{
            //    int randomNumber = random.Next(1, n + 1);
            //    bPlusTree.Delete(randomNumber);
            //}

            //DateTime endDelete = DateTime.Now;
            //TimeSpan deleteTime = endInsert - startInsert;


            // Pretraga ključeva u intervalu od prvog do X-tog najmanjeg elementa strukture Y
            List<int> Y = new List<int>(); //struktura radjena kao List
            int X = 3 * n / 5;

            for (int i = 1; i <= n; i++)
            {
                int randomN = random.Next(1, n + 1);
                Y.Add(randomN);
            }

            //sortiranje strukture Y
            Y.Sort();

            //Console.WriteLine("All elements of the list Y:");

            //foreach (int element in Y)
            //{
            //    Console.Write(element + " ");
            //}

            //Console.WriteLine($"Rezultat pretrage između {Y[0]} i {Y[X]}:");

            DateTime startSearch = DateTime.Now;
            List<int> result = bPlusTree.SearchRange(Y[0], Y[X]);
            //Console.WriteLine(string.Join(", ", result));

            DateTime endSearch = DateTime.Now;
            TimeSpan searchTime = endSearch - startSearch;

            Console.WriteLine($"Insert Time: {insertTime.TotalMilliseconds} ms");
            //Console.WriteLine($"Delete Time: {deleteTime.TotalMilliseconds} ms");
            Console.WriteLine($"Search Time: {searchTime.TotalMilliseconds} ms");
        }
    }

    public class BPlusNode
    {
        //provera da li je list
        public bool IsLeaf { get; set; }
        public BPlusNode Parent { get; set; } 
        public BPlusNode Next { get; set; }    
        public List<int> Keys { get; set; }
        public List<BPlusNode> Children { get; set; }

        public BPlusNode(bool isLeaf = false)
        {
            IsLeaf = isLeaf;
            Keys = new List<int>();
            Children = new List<BPlusNode>();
            Parent = null;
            Next = null;
        }
    }

    public class BPlusTree
    {
        public BPlusNode Root { get; set; }
        public int Order { get; } //max vrednost po cvoru

        public BPlusTree(int order)
        {
            Root = new BPlusNode(isLeaf: true);
            Order = order;
        }

        public bool IsFull(BPlusNode node)
        {
            return node.Keys.Count == Order - 1; //provera za maksimalne vrednosti po cvoru
        }


        public void AddKey(BPlusNode node, int key)
        {
            int index = 0;
            while (index < node.Keys.Count && key >= node.Keys[index])
            {
                index++;
            }

            node.Keys.Insert(index, key);
        }

        public BPlusNode FindLeafNodeForInsertion(int key) //pronalazi list u B+ u kojem moze da se doda kljuc
        {
            BPlusNode current = Root;

            while (!current.IsLeaf) 
            {
                int i = 0; //poz kljuca

                while (i < current.Keys.Count && key >= current.Keys[i]) //pronalazi prvi kljuc koji je veci ili jednak key-u
                {
                    i++;
                }

                
                if (i < current.Children.Count)
                {
                    current = current.Children[i];
                }
                else
                {
                    //trenutni cvor se postavlja na poslednje dete
                    current = current.Children.Last();
                }
            }

            return current;
        }

        public BPlusNode SplitFullLeafNode(BPlusNode fullLeafNode)
        {
            int splitIndex = fullLeafNode.Keys.Count / 2; //index za deljenje list cvora

            BPlusNode newLeafNode = new BPlusNode(isLeaf: true);

            //slanje odredjenog dela u novi leaf cvor
            newLeafNode.Keys.AddRange(fullLeafNode.Keys.GetRange(splitIndex, fullLeafNode.Keys.Count - splitIndex));

            //uklanjanje iz originalnog
            fullLeafNode.Keys.RemoveRange(splitIndex, fullLeafNode.Keys.Count - splitIndex);

           
            newLeafNode.Next = fullLeafNode.Next; //update pointera
            fullLeafNode.Next = newLeafNode;

            int middleKey = newLeafNode.Keys[0];
            return newLeafNode;
        }



        //unutar unutrasnjeg cvora se dostigne maksimalni broj kljuceva (internal cvor)
        public BPlusNode SplitFullInternalNode(BPlusNode fullInternalNode)
        {
            int splitIndex = fullInternalNode.Keys.Count / 2;
            int initialKeyCount = fullInternalNode.Keys.Count; 

            BPlusNode newInternalNode = new BPlusNode();

            //transfer kljuceva u novi unustrasnji cvor 
            newInternalNode.Keys.AddRange(fullInternalNode.Keys.GetRange(splitIndex, initialKeyCount - splitIndex));
            //transfer dece
            newInternalNode.Children.AddRange(fullInternalNode.Children.GetRange(splitIndex + 1, initialKeyCount - splitIndex));

            //uklananje
            fullInternalNode.Keys.RemoveRange(splitIndex, initialKeyCount - splitIndex);
            fullInternalNode.Children.RemoveRange(splitIndex + 1, initialKeyCount - splitIndex);

            
            foreach (var child in newInternalNode.Children)
            {
                child.Parent = newInternalNode;
            }

            return newInternalNode;
        }



        public void InsertIntoParent(BPlusNode leftChild, int key, BPlusNode rightChild)
        {
            BPlusNode parent = leftChild.Parent;

            //roditeljski cvor null, levo dete je root B+ stabla
            if (parent == null)
            {
                BPlusNode newRoot = new BPlusNode();
                newRoot.Keys.Add(key);
                newRoot.Children.Add(leftChild);
                newRoot.Children.Add(rightChild);
                leftChild.Parent = newRoot;
                rightChild.Parent = newRoot;
                Root = newRoot;
            }
            else
            {
                int index = 0;
                while (index < parent.Keys.Count && key >= parent.Keys[index])
                {
                    index++;
                }

                if (IsFull(parent)) //da li je roditeljski pun
                {
                    BPlusNode newInternalNode = SplitFullInternalNode(parent); //podela roditeljskog

                    
                    index = 0;
                    while (index < parent.Keys.Count && key >= parent.Keys[index])
                    {
                        index++;
                    }

                    
                    parent.Keys.Insert(index, key);

                    
                    parent.Children.Insert(index + 1, newInternalNode);

                    //update
                    leftChild.Parent = parent;
                    rightChild.Parent = parent;

                    // ako je roditeljski idalje pun, rekurzija ponovo
                    InsertIntoParent(parent, newInternalNode.Keys[newInternalNode.Keys.Count / 2], rightChild);
                }
                else
                {
                   
                    parent.Keys.Insert(index, key);
                    parent.Children.Insert(index + 1, rightChild);
                    rightChild.Parent = parent; // update roditelja desnog deteta
                }
            }
        }




        public void Insert(int key)
        {
            // dodavanja kljuca u B+ stablo
            // pronalazi list u koji se moze dodati novi kljuc
            BPlusNode leafNode = FindLeafNodeForInsertion(key);

            // provera da li je list pun
            if (IsFull(leafNode))
            {
                // cepanje punog lista
                BPlusNode newLeafNode = SplitFullLeafNode(leafNode);

                
                InsertIntoParent(leafNode, newLeafNode.Keys[0], newLeafNode);

                //sa kojim listom se nastavlja
                if (key >= newLeafNode.Keys[0])
                {
                    leafNode = newLeafNode;
                }
            }

            // ubacivanje key-a u list
            AddKey(leafNode, key);
        }

        //koristim za uklananje kljuca iz liste kljuceva odredjenog cvora
        public void RemoveKey(BPlusNode node, int key)
        {
            node.Keys.Remove(key);

            if (node.Parent != null)
            {
                int indexInParent = node.Parent.Children.IndexOf(node);

                //update roditelja
                if (indexInParent > 0)
                {
                    node.Parent.Keys[indexInParent - 1] = node.Keys.FirstOrDefault();
                }

                
                if (node.Keys.Count < (Order - 1) / 2)
                {
                    AdjustUnderflow(node);
                }
            }
        }



        public BPlusNode FindLeafNodeContainingKey(int key)
        {
            BPlusNode current = Root;

            while (!current.IsLeaf)
            {
                int index = 0;
                while (index < current.Keys.Count && key >= current.Keys[index])
                {
                    index++;
                }

                if (index < current.Children.Count)
                {
                    current = current.Children[index];
                }
                else
                {
                    current = current.Children.Last();
                }
            }

            return current;
        }



        public bool IsUnderflow(BPlusNode node)
        {
            // ako je cvor polu prazan
            return node.Keys.Count < Order / 2;
        }

        public void AdjustUnderflow(BPlusNode node)
        {
            if (node.Parent == null)
            {
                if (node.Keys.Count == 0 && node.Children.Count == 1)
                {
                    Root = node.Children[0];
                    if (Root != null)
                    {
                        Root.Parent = null;
                    }
                }
            }
            else
            {
                BPlusNode leftSibling = GetLeftSibling(node);
                if (leftSibling != null && leftSibling.Keys.Count > Order / 2)
                {
                    BorrowFromLeftSibling(leftSibling, node);
                }
                else
                {
                    BPlusNode rightSibling = GetRightSibling(node);
                    if (rightSibling != null && rightSibling.Keys.Count > Order / 2)
                    {
                        BorrowFromRightSibling(rightSibling, node);
                    }
                    else
                    {
                        MergeWithSibling(node);
                    }
                }

                if (node.Parent != null && node.Parent.Keys.Count < Order / 2)
                {
                    AdjustUnderflow(node.Parent);
                }
            }
        }



        public BPlusNode GetLeftSibling(BPlusNode node)
        {
            if (node.Parent != null)
            {
                int index = node.Parent.Children.IndexOf(node);
                if (index > 0)
                {
                    return node.Parent.Children[index - 1];
                }
            }

            return null;
        }

        public BPlusNode GetRightSibling(BPlusNode node)
        {
            if (node.Parent != null)
            {
                int index = node.Parent.Children.IndexOf(node);
                if (index < node.Parent.Children.Count - 1)
                {
                    return node.Parent.Children[index + 1];
                }
            }

            return null;
        }

        public void BorrowFromLeftSibling(BPlusNode leftSibling, BPlusNode node)
        {
            int borrowIndex = leftSibling.Keys.Count - 1;

            int keyFromParent = node.Parent.Keys[node.Parent.Children.IndexOf(node) - 1];
            int keyFromLeftSibling = leftSibling.Keys[borrowIndex];

            node.Parent.Keys[node.Parent.Children.IndexOf(node) - 1] = keyFromLeftSibling;
            node.Keys.Insert(0, keyFromParent);

            if (!node.IsLeaf)
            {
                BPlusNode borrowedChild = leftSibling.Children[borrowIndex + 1];
                borrowedChild.Parent = node;
                node.Children.Insert(0, borrowedChild);
                leftSibling.Children.RemoveAt(borrowIndex + 1);
            }

            leftSibling.Keys.RemoveAt(borrowIndex);
        }


        public void BorrowFromRightSibling(BPlusNode rightSibling, BPlusNode node)
        {
            if (rightSibling.Keys.Count > 0 && node.Parent != null)
            {
                int borrowIndex = 0;

               
                int indexOfNode = node.Parent.Children.IndexOf(node);
                if (indexOfNode >= 0)
                {
                    int keyFromParent = node.Parent.Keys[indexOfNode];
                    int keyFromRightSibling = rightSibling.Keys[borrowIndex];

                    node.Parent.Keys[indexOfNode] = keyFromRightSibling;
                    node.Keys.Add(keyFromParent);

                    if (!node.IsLeaf)
                    {
                        BPlusNode borrowedChild = rightSibling.Children[borrowIndex];
                        borrowedChild.Parent = node;
                        node.Children.Add(borrowedChild);
                        rightSibling.Children.RemoveAt(borrowIndex);
                    }

                    rightSibling.Keys.RemoveAt(borrowIndex);
                }
            }
        }

        //ovde puca program...probala sam da resim ali nisam uspela :(
        public void MergeWithSibling(BPlusNode node)
        {
            BPlusNode parent = node.Parent;
            int index = parent.Children.IndexOf(node);

            if (index > 0)
            {
                BPlusNode leftSibling = parent.Children[index - 1];

                
                leftSibling.Keys.Add(parent.Keys[index - 1]);
                leftSibling.Keys.AddRange(node.Keys);
                leftSibling.Children.AddRange(node.Children);

                parent.Keys.RemoveAt(index - 1);
                parent.Children.RemoveAt(index);
            }
            else if (index + 1 < parent.Children.Count)
            {
                BPlusNode rightSibling = parent.Children[index + 1];

                
                node.Keys.Insert(0, parent.Keys[index]);
                node.Keys.AddRange(rightSibling.Keys);
                node.Children.AddRange(rightSibling.Children);

                parent.Keys.RemoveAt(index);
                parent.Children.RemoveAt(index + 1);
            }

           
            if (parent.Keys.Count == 0)
            {
                if (parent.Parent != null)
                {
                   
                    MergeWithSibling(parent);
                }
                else
                {
                   
                    Root = node;
                    node.Parent = null;
                }
            }
            else if (parent.Keys.Count < (Order - 1) / 2)
            {
                AdjustUnderflow(parent);
            }
        }



        // brisanje kljuca iz B+ stabla
        public void Delete(int key)
        {
            // pronalazenje lista koji sadrzi kljuc
            BPlusNode leafNode = FindLeafNodeContainingKey(key);
            RemoveKey(leafNode, key);

            // provera da li je kljuc u "internal" cvoru
            if (leafNode != Root && leafNode.Keys.Count < (Order - 1) / 2)
            {
                AdjustUnderflow(leafNode);
            }

            // provera podkapaciteta u korenu
            if (Root != null && Root.Keys.Count == 0)
            {
                Root = Root.Children.FirstOrDefault();
                if (Root != null)
                {
                    Root.Parent = null;
                }
            }

            // brisanje u "internal" cvoru
            BPlusNode parentNode = leafNode.Parent;
            while (parentNode != null)
            {
                RemoveKey(parentNode, key);

                // Ako nije prvi kljuc u "internal" cvoru, azuriraj roditeljski kljuc
                if (parentNode.Keys.Count > 0 && parentNode.Children.IndexOf(leafNode) > 0)
                {
                    parentNode.Keys[parentNode.Children.IndexOf(leafNode) - 1] = leafNode.Keys.First();
                }

                if (parentNode.Keys.Count < (Order - 1) / 2)
                {
                    AdjustUnderflow(parentNode);
                }

                parentNode = parentNode.Parent;
            }

            // ako je potrebno azuriraj koren
            if (Root != null && Root.Keys.Count == 0)
            {
                Root = Root.Children.FirstOrDefault();
                if (Root != null)
                {
                    Root.Parent = null;
                }
            }
        }


        public List<int> SearchRange(int startKey, int endKey)
        {
            // pretrage kljuceva u intervalu
            List<int> result = new List<int>();

            BPlusNode currentNode = FindLeafNodeContainingKey(startKey);

            while (currentNode != null)
            {
                for (int i = 0; i < currentNode.Keys.Count; i++)
                {
                    if (currentNode.Keys[i] >= startKey && currentNode.Keys[i] <= endKey)
                    {
                        result.Add(currentNode.Keys[i]);
                    }
                }

               
                if (currentNode.Next != null)
                {
                    currentNode = currentNode.Next;
                }
                else
                {
                    
                    break;
                }
            }

            return result;
        }

        //fje za stampanje
        public void PrintTree()
        {
            Console.WriteLine("Printing B+ Tree:");
            PrintTree(Root, 0);
        }

        public void PrintTree(BPlusNode node, int level)
        {
            if (node != null)
            {
                Console.Write($"{new string(' ', level * 4)}[");
                for (int i = 0; i < node.Keys.Count; i++)
                {
                    Console.Write($"{node.Keys[i]}");
                    if (i < node.Keys.Count - 1)
                    {
                        Console.Write(", ");
                    }
                }
                Console.WriteLine("]");

                if (!node.IsLeaf)
                {
                    for (int i = 0; i < node.Children.Count; i++)
                    {
                        
                        if (node.Children[i] != null)
                        {
                            PrintTree(node.Children[i], level + 1);
                        }
                    }
                }
            }
        }

    }

}