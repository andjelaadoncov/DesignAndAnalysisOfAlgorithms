using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pia5_18627
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph g1 = new Graph();
            //int N = 5;
            //int k = 1000;
            //int N = 100;
            //int N = 1000;
            //int N = 10000;
            //int N = 100000;
            //int k = 2 * N;
            //int k = 5 * N;
            //int k = 10 * N;
            //int k = 20 * N;
            //int k = 33 * N;
            //int k = 50 * N;
            g1.Build_Slucaj1(N, k);
            g1.PrintGraph();

            //Graph g2 = new Graph();
            //g2.Build_Slucaj2(N, k);

            //g2.PrintGraph();
            //g1.PrintGraph();

            //ova ispod dva odkomentarisati za proveru rada Primovog algoritma na nekom manjem primeru gde je lakse proveriti
            //g1.ProveraZaPrimov();
            //g1.PrintGraph();

            Console.WriteLine("MIN SPAN TREE _________ PRIMOV_________________________________________________\n");

            g1.PrimovMST();
            g1.PrintGraph();

            //g2.PrimovMST();
            //g2.PrintGraph();
        }
    }

    class GraphNode //klasa za cvor grafa
    {
        private int value;
        List<GraphNode> susedi;

        public GraphNode(int value)
        {
            this.value = value;
            susedi = new List<GraphNode>();
        }

        public int Value { get { return value; } }
        public List<GraphNode> Susedi { get { return susedi; } set { susedi = value; } }

        public List<int> Weights { get; set; } = new List<int>();

        public bool AddNeighbour(GraphNode sused)
        {
            if (Susedi.Contains(sused))
            {
                return false;
            }
            else
            {
                Susedi.Add(sused);
                return true;
            }
        }

        public bool RemoveNeighbour(GraphNode sused)
        {
            if (Susedi.Contains(sused))
            {
                Susedi.Remove(sused);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveAllNeighbors() //na kraju nisam koristila ovu fju u PrimovMST fji, vec ugradjenu fju Clear jer je brza
        {
            for (int i = Susedi.Count - 1; i >= 0; i--)
            {
                Susedi.RemoveAt(i);
            }
            return true;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[Cvor : " + Value + " sa susedima");
            foreach (var item in susedi)
            {
                stringBuilder.Append(" -> " + item.Value);
            }

            stringBuilder.AppendLine("]");

            return stringBuilder.ToString();
        }
    }

    class GraphEdge //klasa za grane grafa
    {
        public GraphNode Pocetni { get; set; }
        public GraphNode Krajnji { get; set; }

        public int Weight { get; set; }

        //public int CompareTo(GraphEdge edge)
        //{
        //    return this.Weight - edge.Weight;
        //}

        public override string ToString()
        {
            return $"Grana: {Pocetni.Value} - {Krajnji.Value}, tezina: {Weight}";
        }
    }

    class Graph
    {

        List<GraphNode> nodes = new List<GraphNode>();

        public Graph()
        {

        }

        public int Count { get { return nodes.Count; } }

        public List<GraphNode> Nodes
        {
            get { return nodes; }

            set { nodes = value; }
        }

        public bool AddNode(int value)
        {

            //if (Find(value) != null)
            //{
            //    return false;
            //}
            //else
            //{
                nodes.Add(new GraphNode(value));
                return true;
            //}
        }

        GraphNode Find(int node)
        {
            foreach (GraphNode item in nodes)
            {
                if (item.Value.Equals(node))
                {
                    return item;
                }
            }

            return null;
        }
        public bool AddEdge(GraphNode n1, GraphNode n2, int weight)
        {

            if (n1 == null || n2 == null || n1 == n2)
            {
                return false;
            }
            else if (n1.Susedi.Contains(n2))
            {
                return false;
            }
            else
            {
                n1.AddNeighbour(n2);
                n1.Weights.Add(weight);

                n2.AddNeighbour(n1);
                n2.Weights.Add(weight);

                return true;
            }
        }

        public void PrintGraph()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < Count; i++)
            {
                stringBuilder.Append(nodes[i].ToString());

                foreach (var sused in nodes[i].Susedi)
                {
                    int weightIndex = nodes[i].Susedi.IndexOf(sused);
                    int weight = nodes[i].Weights[weightIndex];
                    stringBuilder.AppendLine($"Grana izmedju cvora {nodes[i].Value} i {sused.Value} sa tezinom {weight}.");
                }

                if (i < Count - 1)
                {
                    stringBuilder.Append("\n");
                }

            }

            Console.WriteLine(stringBuilder.ToString());


        }

        public void GenerateNNodes(int N) //generise N cvora
        {
            for (int i = 0; i < N; i++)
            {
                AddNode(i);
            }
        }

        public void GenerateKEdges(int k) //generise k grana
        {
            Random random = new Random();
            for (int i = 0; i < k; i++)
            {
                int i1 = random.Next(Count);
                int i2 = random.Next(Count);

                GraphNode firstOne = Find(i1);
                GraphNode secondOne = Find(i2);

                if (!AddEdge(firstOne, secondOne, random.Next(20)))
                {
                    i--;
                }
            }
        }

        public void Build_Slucaj1(int N, int k) //za sluc1
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            GenerateNNodes(N);
            Random random = new Random();
            int randomIndex = random.Next(Count);

            GraphNode randomGeneratedNode = Find(randomIndex);
            for (int i = 0; i < Count; i++)
            {
                AddEdge(randomGeneratedNode, Nodes[i], random.Next(20));
            }
            // Stop the stopwatch
           
            GenerateKEdges(k);
            stopwatch.Stop();
            // Get the elapsed time
            TimeSpan elapsedTime = stopwatch.Elapsed;

            // Display the elapsed time
            Console.WriteLine($"Elapsed Time: {elapsedTime.TotalMilliseconds} milliseconds");

        }

        public void Build_Slucaj2(int N, int k) //za sluc2
        {

            GenerateNNodes(N);

            Random random = new Random();
            for (int i = 0; i < Count - 1; i++)
            {
                AddEdge(Nodes[i], Nodes[i + 1], random.Next(20));
            }

            AddEdge(Nodes[Count - 1], Nodes[0], random.Next(20));

            GenerateKEdges(k);
        }

        public GraphEdge this[int Od, int Do] //pravim indekser koji mi pomaze da nadjem granu
        {
            get
            {
                GraphNode nodeOd = Nodes[Od];
                GraphNode nodeDo = Nodes[Do];

                int i = nodeOd.Susedi.IndexOf(nodeDo);

                if (i >= 0)
                {
                    GraphEdge edge = new GraphEdge();
                    edge.Pocetni = nodeOd;
                    edge.Krajnji = nodeDo;
                    if (i <= nodeDo.Weights.Count)
                    {
                        edge.Weight = nodeOd.Weights[i];
                    }
                    else
                    {
                        edge.Weight = 0;
                    }

                    return edge;
                }

                return null;
            }
        }

        public void PrimovMST()
        {
            int[] previous = new int[Count]; //cuva se prethodni cvor za svaki cvor u MST
            previous[0] = -1;

            int[] minWeight = new int[Count]; //cuva se minimalna tezina za svaki cvor
            Fill(minWeight, int.MaxValue); //inicijalno su sve vrednosti stavljene na MaxValue (zamenjuje beskonacno)
            minWeight[0] = 0;

            bool[] deoMST = new bool[Count]; //sluzi mi da oznacim koji se cvorovi vec nalaze u MST
            for (int i = 0; i < deoMST.Length; i++)
            {
                deoMST[i] = false;
            }

            //deoMST[0] = true;

            for (int i = 0; i < Nodes.Count - 1; i++)
            {
                int minWeightIndex = ReturnIndex(minWeight, deoMST); //trazim ovde cvor sa min tezinom koji nije vec u MST
                deoMST[minWeightIndex] = true; //setujem da je sad dodat taj cvor


                for (int j = 0; j < Nodes.Count; j++)
                {
                    GraphEdge edge = this[minWeightIndex, j];
                    int weight;
                    if (edge != null)
                    {
                        weight = edge.Weight;
                    }
                    else
                        weight = -1;

                    if (edge != null && !deoMST[j] && weight < minWeight[j]) //ako postoji grana sa manjom tezinom do nekog cvora i nije deo MST, azuriram prethodnika i minWeight za taj cvor
                    {
                        previous[j] = minWeightIndex;
                        minWeight[j] = weight;
                    }
                }
            }

            //pravim listu grana koje cine MST
            List<GraphEdge> rezultat = new List<GraphEdge>();
            for (int i = 1; i < Nodes.Count; i++)
            {
                GraphEdge edge = this[previous[i], i];
                rezultat.Add(edge);
            }

            //uklananje starih grana iz stabla
            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].Susedi.Clear();
            }

            //dodavanje novih grana za napravljen PrimovMST
            for (int i = 0; i < rezultat.Count; i++)
            {
                this.AddEdge(rezultat[i].Pocetni, rezultat[i].Krajnji, rezultat[i].Weight);
            }

        }

        public int ReturnIndex(int[] weights, bool[] deoMST)
        {
            int minValue = int.MaxValue; //zbog poredjenja

            int minIndex = 0;

            for (int i = 0; i < Nodes.Count; i++)
            {
                if (!deoMST[i] && weights[i] < minValue) //ispituje se dal je vec cvor u MST i da li je tezina manja od minimalne
                {
                    minValue = weights[i];
                    minIndex = i;
                }
            }

            return minIndex;
        }
        public void Fill(int[] niz, int value)
        {
            for (int i = 0; i < niz.Length; i++)
            {
                niz[i] = value;
            }
        }

        public void ProveraZaPrimov() //fja koja pravi probni graf..sluzio mi je da proverim da li radi Primov algoritam
        {
            this.AddNode(1);
            this.AddNode(2);
            this.AddNode(3);
            this.AddNode(4);
            this.AddNode(5);
            this.AddNode(6);
            this.AddNode(7);
            this.AddNode(8);

            this.AddEdge(Find(1), Find(2), 3);
            this.AddEdge(Find(1), Find(3), 5);
            this.AddEdge(Find(2), Find(4), 4);
            this.AddEdge(Find(3), Find(4), 12);
            this.AddEdge(Find(4), Find(5), 9);
            this.AddEdge(Find(4), Find(8), 8);
            this.AddEdge(Find(5), Find(6), 4);
            this.AddEdge(Find(5), Find(8), 1);
            this.AddEdge(Find(5), Find(7), 5);
            this.AddEdge(Find(6), Find(7), 6);
            this.AddEdge(Find(7), Find(8), 20);
        }

        public List<GraphEdge> VratiGrane()
        {
            List<GraphEdge> edges = new List<GraphEdge>();
            for (int i = 0; i < Nodes.Count; i++)
            {
                GraphEdge edge = this[i, i + 1];
                edges.Add(edge);
            }

            return edges;
        }
    }
}
