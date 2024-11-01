using System;
using System.Collections.Generic;
using System.Linq;

class BinomialNode
{
    public int Key { get; set; }
    public int Degree { get; set; }
    public BinomialNode Parent { get; set; }
    public BinomialNode Child { get; set; }
    public BinomialNode Sibling { get; set; }

    public BinomialNode(int key)
    {
        Key = key;
        Degree = 0;
        Parent = null;
        Child = null;
        Sibling = null;
    }
}

class BinomialHeap
{
    private BinomialNode head;
    private int count;
    private BinomialNode minNode;

    public BinomialHeap()
    {
        head = null;
        count = 0;
        minNode = null;
    }

    public BinomialHeap MakeBinomialHeap()
    {
        return new BinomialHeap();
    }

    public BinomialNode BinomialHeapMinimum()
    {
        if (head == null) return null;
        BinomialNode y = null;
        BinomialNode x = head;
        int min = head.Key;

        while (x != null)
        {
            if (x.Key < min)
            {
                min = x.Key;
                y = x;
            }
            x = x.Sibling;
        }

        return y;
    }

    public void BinomialLink(BinomialNode y, BinomialNode z)
    {
        y.Parent = z;
        y.Sibling = z.Child;
        z.Child = y;
        z.Degree++;
    }

    public BinomialHeap BinomialHeapUnion(BinomialHeap h1, BinomialHeap h2)
    {
        BinomialHeap h = MakeBinomialHeap();
        h.head = BinomialHeapMerge(h1.head, h2.head);
        h.count = h1.count + h2.count;

        if (h.head == null)
        {
            return h;
        }

        BinomialNode prevX = null;
        BinomialNode x = h.head;
        BinomialNode nextX = x.Sibling;

        while (nextX != null)
        {
            if ((x.Degree != nextX.Degree) || (nextX.Sibling != null && nextX.Sibling.Degree == x.Degree))
            {
                prevX = x;
                x = nextX;
            }
            else if (x.Key <= nextX.Key)
            {
                x.Sibling = nextX.Sibling;
                BinomialLink(nextX, x);
            }
            else
            {
                if (prevX == null)
                {
                    h.head = nextX;
                }
                else
                {
                    prevX.Sibling = nextX;
                }
                BinomialLink(x, nextX);
                x = nextX;
            }
            nextX = x.Sibling;
        }

        // azurira polje minNode na cvor sa najmanjim kljucem u novom hipu
        if (h1.minNode == null || (h2.minNode != null && h2.minNode.Key < h1.minNode.Key))
        {
            h.minNode = h2.minNode;
        }
        else
        {
            h.minNode = h1.minNode;
        }

        return h;
    }

    public BinomialNode BinomialHeapMerge(BinomialNode h1, BinomialNode h2)
    {
        if (h1 == null)
        {
            return h2;
        }
        else if (h2 == null)
        {
            return h1;
        }
        else
        {
            BinomialNode head = null;
            BinomialNode tail = null;
            BinomialNode x = h1;
            BinomialNode y = h2;

            while (x != null && y != null)
            {
                if (x.Degree <= y.Degree)
                {
                    if (tail != null)
                    {
                        tail.Sibling = x;
                    }
                    else
                    {
                        head = x;
                    }
                    tail = x;
                    x = x.Sibling;
                }
                else
                {
                    if (tail != null)
                    {
                        tail.Sibling = y;
                    }
                    else
                    {
                        head = y;
                    }
                    tail = y;
                    y = y.Sibling;
                }
            }

            if (x != null)
            {
                tail.Sibling = x;
            }
            else
            {
                tail.Sibling = y;
            }

            return head;
        }
    }


    public void BinomialHeapInsert(int key)
    {
        BinomialNode node = new BinomialNode(key);
        BinomialHeap newHeap = MakeBinomialHeap();
        newHeap.head = node;
        this.head = BinomialHeapUnion(this, newHeap).head;
        Consolidate();
        count++;

        if (minNode == null || node.Key < minNode.Key)
        {
            minNode = node;
        }
    }

    public void DeleteMin()
    {
        // pronalazi cvor sa najmanjim kljucem
        minNode = BinomialHeapMinimum();

        if (minNode == null)
        {
            // prazan heap ne treba brisanje....zasto dolazi ovde
            return;
        }

        // podesava head tako da preskoci cvor sa najmanjim kljucem
        if (head == minNode)
        {
            head = minNode.Sibling;
        }
        else
        {
            //nalazi prethodni cvor kako bi se preskocio cvor sa najmanjim kljucem
            BinomialNode prevNode = null;
            BinomialNode currentNode = head;

            while (currentNode != null && currentNode != minNode)
            {
                prevNode = currentNode;
                currentNode = currentNode.Sibling;
            }

            // preskace cvor sa najmanjim kljucem
            if (prevNode != null)
            {
                prevNode.Sibling = minNode.Sibling;
            }
        }

        //cisti roditelja i siblinga za cvor sa najmanjim kljucem
        minNode.Parent = null;
        minNode.Sibling = null;

       
        Consolidate();
        // smanjujem veličinu hipa za 1
        count--;
    }

    public void Consolidate()
    {
        // kreira niz za cuvanje stabala po redovima
        int maxDegree = (int)Math.Floor(Math.Log(count + 1) / Math.Log(2));
        BinomialNode[] array = new BinomialNode[maxDegree + 1];

        // prolazi kroz listu stabala u hipu
        BinomialNode current = head;
        while (current != null)
        {
            // cuva referencu na sledece stablo u listi
            BinomialNode next = current.Sibling;

            // spaja stabla istog reda dok god postoji mesto u nizu
            int degree = current.Degree;
            while (array[degree] != null)
            {
                // uporedjuje kljuceve korena i bira manji kao novi koren
                BinomialNode other = array[degree];
                if (current.Key > other.Key)
                {
                    BinomialNode temp = current;
                    current = other;
                    other = temp;
                }

                // spaja drugo stablo kao dete prvog i povecava red
                Link(other, current);
                array[degree] = null;
                degree++;
            }

            // postavlja trenutno stablo na odgovarajuce mesto u nizu
            array[degree] = current;

            // prelazi na sledece stablo u listi
            current = next;
        }

        // nova lista stabala iz niza
        head = null;
        minNode = null;
        //BinomialNode minNode = null;
        for (int i = 0; i <= maxDegree; i++)
        {
            if (array[i] != null)
            {
                // dodaje stablo na kraj liste
                array[i].Parent = null;
                array[i].Sibling = head;
                head = array[i];

                // azuriranje minimuma
                if (minNode == null || array[i].Key < minNode.Key)
                {
                    minNode = array[i];
                }
            }
        }
    }

    // pomocna funkcija za Consolidate
    public void Link(BinomialNode child, BinomialNode parent)
    {
        // uklanja dete iz liste stabala
        child.Sibling = null;

        // dodaje dete na pocetak liste dece roditelja
        child.Parent = parent;
        child.Sibling = parent.Child;
        parent.Child = child;

        // povecava stepen roditelja
        parent.Degree++;
    }

    public void PrintNumbersGreaterThanP(int p, int N, int k)
    {
        Console.WriteLine($"N = {N}, k = {k}");
        Console.WriteLine($"Brojevi veci od {p}:");

        PrintNumbersGreaterThanPHelper(head, p);
    }

    private void PrintNumbersGreaterThanPHelper(BinomialNode node, int p)
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

    public int Size()
    {
        return count;
    }
}

class Program
{
    static void Main()
    {
        int a = 1;
        int b = 100;
        int N = 1000;
        int k = 10;
        int Nmin = 1000;
        int Nmax = 10000000;

        int kmin = 10;
        int kmax = 100;

        int p = (int)(0.75 * (a + b));


        BinomialHeap binomialHeap = new BinomialHeap();
        Random random = new Random();

        for (N = Nmin; N <= Nmax; N *= 10)
        {
            for (k = kmin; k <= kmax; k += 10)
            {
                for (int i = 0; i < N; i++)
                {
                    //nije najbolje printanje mozda najbolje da se proba sa samo unosenjem vrednosti
                    //za N i k za odredjenje slucajeve
                    //nisam stigla to lepo da odradim ali je potrebna samo ova for petlja zadnja onda u tom slucaju..
                    int randomValue = random.Next(a, b + 1);

                    binomialHeap.BinomialHeapInsert(randomValue);
                    //Console.WriteLine(binomialHeap.Size());

                    if ((i + 1) % k == 0)
                    {
                        binomialHeap.DeleteMin();
                    }
                }

                Console.WriteLine("Brojevi veci od {0}:", p);
                binomialHeap.PrintNumbersGreaterThanP(p, N, k);
                Console.WriteLine();
            }
        }
    }
}
