using System;

public class Node
{
    public Node parent;
    public Node child;
    public Node sibling;
    private int key;
    private int degree;

    public Node(Node parent, Node child, Node sibling, int key, int degree)
    {
        this.parent = parent;
        this.child = child;
        this.sibling = sibling;
        this.key = key;
        this.degree = degree;
    }

    public Node Parent
    {
        get { return parent; }
        set { parent = value; }
    }
    public Node Child
    {
        get { return child; }
        set { child = value; }
    }
    public Node Sibling
    {
        get { return sibling; }
        set { sibling = value; }
    }

    public int Key
    {
        get { return key; }
        set { key = value; }
    }

    public int Degree
    {
        get { return degree; }
        set { degree = value; }
    }

    public void IncrementDegree()
    {
        degree++;
    }
}

public class Heap
{
    public Node head;
    public Node rootTail;

    public Heap()
    {
        head = null;
        rootTail = null;
    }

    public Heap(int k)
    {
        head = new Node(null, null, null, k, 0);
        rootTail = new Node(null, null, null, k, 0);
    }

    public Heap(Node p)
    {
        head = p;
        rootTail = p;
    }

    public static void BinomialLink(Node y, Node z)
    {
        y.parent = z;
        y.sibling = z.child;
        z.child = y;
        z.IncrementDegree();
    }

    public static Heap MergeBinomialHeaps(Heap x, Heap y)
    {
        Node ptr;
        Heap heap = new Heap();

        Node ptrx = x.head;
        Node ptry = y.head;
        Node pom1, pom2;
        while (ptrx != null && ptry != null)
        {
            if (ptrx.Degree == ptry.Degree)
            {
                if (ptrx.Key <= ptry.Key)
                {
                    pom1 = ptrx;
                    ptrx = ptrx.sibling;
                    pom2 = ptry;
                    ptry = ptry.sibling;
                    heap.InsertFromBehind(pom1);
                    heap.InsertFromBehind(pom2);
                }
                else
                {
                    pom1 = ptrx;
                    ptrx = ptrx.sibling;
                    pom2 = ptry;
                    ptry = ptry.sibling;
                    heap.InsertFromBehind(pom2);
                    heap.InsertFromBehind(pom1);
                }
            }
            else if (ptrx.Degree < ptry.Degree)
            {
                pom1 = ptrx;
                ptrx = ptrx.sibling;
                heap.InsertFromBehind(pom1);
            }
            else
            {
                pom1 = ptry;
                ptry = ptry.sibling;
                heap.InsertFromBehind(pom1);
            }
        }

        if (ptrx == null)
        {
            heap.InsertFromBehind(ptry);
            x.head = null;
        }
        else if (ptry == null)
        {
            heap.InsertFromBehind(ptrx);
            y.head = null;
        }

        return heap;
    }

    public Node FindMinimumKey()
    {
        if (this.head == null)
        {
            return null;
        }

        Node ptr = this.head;
        Node ret = this.head;
        int min = ptr.Key;
        while (ptr != null)
        {
            if (ptr.Key < min)
            {
                ret = ptr;
                min = ptr.Key;
            }

            ptr = ptr.sibling;
        }

        return ret;
    }

    public static Heap BinomialHeapUnion(Heap h1, Heap h2)
    {
        Heap heap = new Heap();

        heap = MergeBinomialHeaps(h1, h2);

        if (heap.head == null)
        {
            return null;
        }

        Node prevx = null;
        Node x = heap.head;
        Node nextx = x.sibling;
        while (nextx != null)
        {
            if ((x.Degree != nextx.Degree) || (nextx.sibling != null && nextx.sibling.Degree == x.Degree))
            {
                prevx = x;
                x = nextx;
            }
            else
            {
                if (x.Key <= nextx.Key)
                {
                    x.sibling = nextx.sibling;
                    BinomialLink(nextx, x);
                }
                else
                {
                    if (prevx == null)
                    {
                        heap.head = nextx;
                    }
                    else
                    {
                        prevx.sibling = nextx;
                    }

                    BinomialLink(x, nextx);
                    x = nextx;
                }
            }
            nextx = x.sibling;
        }

        return heap;
    }

    public Heap BinomialHeapInsert(Node p)
    {
        if (p.Key < 0)
        {
            Console.WriteLine("Negativan je broj elementa");
            return this;
        }

        Heap heapX = new Heap(p);
        Heap heapPrim;
        heapPrim = BinomialHeapUnion(this, heapX);

        return heapPrim;
    }

    public void RemoveRootWithoutDelete(Node p)
    {
        if (head == null)
        {
            return;
        }
        if (head.sibling == null && p == head)
        {
            head = null;
            rootTail = null;
            return;
        }
        if (p == head)
        {
            head = p.sibling;
            return;
        }

        Node ptr = head;
        while (ptr.sibling != p)
        {
            ptr = ptr.sibling;
        }
        if (p.sibling == null)
        {
            rootTail = ptr;
        }

        ptr.sibling = p.sibling;
    }

    public void InsertFromHead(Node p)
    {
        if (head == null)
        {
            p.sibling = head;
            head = p;
            rootTail = p;
            return;
        }

        p.sibling = head;
        head = p;
    }

    public void InsertFromBehind(Node p)
    {
        // prazno
        if (rootTail == null)
        {
            head = p;
            rootTail = p;
            return;
        }

        rootTail.Sibling = p;
        rootTail = rootTail.Sibling;
    }

    public Heap ExtractMin()
    {
        if (head == null)
        {
            return null;
        }

        Node min = FindMinimumKey();
        RemoveRootWithoutDelete(min);

        Heap heap = new Heap();
        Node ptr = min.child;
        Node ptrAdv;
        while (ptr != null)
        {
            min.child = ptr.sibling;
            ptr.parent = null;
            heap.InsertFromHead(ptr);

            ptr = min.child;
        }

        return BinomialHeapUnion(this, heap);
    }

    public void ExchangeKeys(Node p1, Node p2)
    {
        int pom = p2.Key;
        p2.Key = p1.Key;
        p1.Key = pom;
    }

    public void DecreaseKey(Node x, int key)
    {
        if (key > x.Key)
        {
            Console.WriteLine("key je veci od key-a datog cvora");
            return;
        }

        x.Key = key;
        Node y = x;
        Node p = y.parent;
        while (p != null && y.Key < p.Key)
        {
            ExchangeKeys(p, y);

            y = p;
            p = p.parent;
        }
    }

    public Heap DeleteKey(Node p)
    {
        this.DecreaseKey(p, -1);
        return this.ExtractMin();
    }

    public void Dispose()
    {
        while (this.head != null)
        {
            Node next = this.head.sibling;
            this.head.sibling = null; // Disconnect the current head
            this.head = next;
        }
        this.rootTail = null;
    }

    public void PrintNumbersGreaterThanP(int p)
    {
        PrintNumbersGreaterThanPHelper(head, p);
    }

    private void PrintNumbersGreaterThanPHelper(Node node, int p)
    {
        if (node == null)
        {
            return;
        }

        if (node.Key > p)
        {
            Console.WriteLine(node.Key);
        }

        PrintNumbersGreaterThanPHelper(node.Child, p);
        PrintNumbersGreaterThanPHelper(node.Sibling, p);
    }
}

class Program
{
    static void Main()
    {
        int a = 1;
        int b = 10;
        int N = 20;
        int k = 5;
        int p = (int)(0.75 * (a + b));
        int i = 0;
        int[] niz = { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 };
        Random random = new Random();
        Heap heap = new Heap();

        while (i < N)
        {
            //int randomValue = random.Next(a, b + 1);
            int randomValue = niz[i];
            Node randomNode = new Node(null, null, null, randomValue, 1);
            heap = heap.BinomialHeapInsert(randomNode);
            i++;

            if (i % k == 0)
            {
                heap.Dispose();
            }
        }

        Console.WriteLine("Brojevi veci od {0}:", p);
        heap.PrintNumbersGreaterThanP(p);

        // Dispose of resources
        heap = null;
    }
}
