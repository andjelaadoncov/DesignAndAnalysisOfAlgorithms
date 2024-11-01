using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace lab1_18627
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random(); //za generisanje random brojeva

            int[] array1 = new int[100];
            for (int i = 0; i < 100; i++) // 100
            {
                array1[i] = random.Next(10000);
            }

            int[] array2 = new int[1000];
            for (int i = 0; i < 1000; i++) //1000
            {
                array2[i] = random.Next(10000);
            }

            int[] array3 = new int[10000];
            for (int i = 0; i < 10000; i++) // 10k
            {
                array3[i] = random.Next(10000);
            }

            int[] array4 = new int[100000];
            for (int i = 0; i < 100000; i++) // 100k
            {
                array4[i] = random.Next(10000);
            }

            int[] array5 = new int[1000000];
            for (int i = 0; i < 1000000; i++) // 1M
            {
                array5[i] = random.Next(10000);
            }

            int[] array6 = new int[10000000];
            for (int i = 0; i < 10000000; i++) // 10M
            {
                array6[i] = random.Next(10000);
            }

            //PrintFunction(array1);
            //Console.WriteLine("...............................................................");
            //SelectionSort(array1);
            //PrintFunction(array1);

            //PrintFunction(array1);
            //Console.WriteLine(".......................");
            //BucketSort(array1);
            //PrintFunction(array1);


            //PrintFunction(array1);
            //Console.WriteLine("/////////////////////////////////////////////");
            //HeapSort(array1);
            //PrintFunction(array1);

        }

        public static void PrintFunction(int[] niz)
        {
            for (int i = 0; i < niz.Length; i++)
            {
                Console.WriteLine(niz[i]);
            }
        }

        //Selection Sort
        public static void SelectionSort(int[] niz)
        {
            long memoryBefore = GC.GetTotalMemory(true); //get memory usage before function
            DateTime start = DateTime.Now;
            //prva petlja je za pomeranje elemenata niza jedan po jedan
            for (int i = 0; i < niz.Length - 1; i++)
            {
                int minElement = i; //pomocni index za nalazenje min elementa u nesortiranom delu niza
                for (int j = i + 1; j < niz.Length; j++)
                {
                    if (niz[j] < niz[minElement])
                    {
                        minElement = j;
                    }
                }

                //zamena mesta
                int swapHelp = niz[minElement];
                niz[minElement] = niz[i];
                niz[i] = swapHelp;
            }

            DateTime end = DateTime.Now;
            long memoryAfter = GC.GetTotalMemory(true);  // Get memory usage after function

            // Vreme izvrsenja:
            long memoryUsed = memoryAfter - memoryBefore;
            TimeSpan ts = (end - start);
            Console.WriteLine("The execution time of the program is {0} ms", ts.TotalMilliseconds);
            Console.WriteLine("The memory used for this function is: {0} bytes", memoryUsed.ToString());
        }

        //Heap Sort
        public static void HeapSort(int[] niz)
        {
            long memoryBefore = GC.GetTotalMemory(true); //get memory usage before function
            DateTime start = DateTime.Now;
            BuildHeap(niz);
            int n = niz.Length;
            for (int i = n - 1; i >= 1; i--)
            {
                int pom = niz[0];
                niz[0] = niz[i];
                niz[i] = pom;

                n--;
                Heapify(0, n, niz);
            }
            DateTime end = DateTime.Now;
            long memoryAfter = GC.GetTotalMemory(true);  // Get memory usage after function
            // Vreme izvrsenja:
            long memoryUsed = memoryAfter - memoryBefore;
            TimeSpan ts = (end - start);
            Console.WriteLine("The execution time of the program is {0} ms", ts.TotalMilliseconds);
            Console.WriteLine("The memory used for this function is: {0} bytes", memoryUsed.ToString());
        }


        //pomocne funkcije za HeapSort
        public static void BuildHeap(int[] niz) //fja za kreiranje Heap-a
        {
            int heap_size = niz.Length;
            for (int i = heap_size / 2 - 1; i >= 0; i--)
            {
                Heapify(i, heap_size, niz);
            }
        }

        public static void Heapify(int i, int heap_size, int[] niz) // za stabilisanje stabla nakon dodavanja novog elementa
        {
            int l = 2 * i + 1; //levi el
            int r = 2 * i + 2; //desni el
            int largest;

            if (l < heap_size && niz[l] > niz[i])
            {
                largest = l;
            }
            else
                largest = i;

            if (r < heap_size && niz[r] > niz[largest])
            {
                largest = r;
            }

            if (largest != i)
            {
                int pom = niz[i];
                niz[i] = niz[largest];
                niz[largest] = pom;

                Heapify(largest, heap_size, niz);
            }
        }

        //Bucket Sort
        public static void BucketSort(int[] array)
        {
            long memoryBefore = GC.GetTotalMemory(true); //get memory usage before function
            DateTime start = DateTime.Now;

            if (array == null || array.Length <= 1)
            {
                return; //nema smisla sortirati ako nema elemenata ili ukoliko postoji samo 1 element u nizu
            }

            int max = array[0]; //za trazenje najveceg elementa niza
            int min = array[0]; //za trazenje najmanjeg elementa niza

            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > max)
                {
                    max = array[i];
                }
                if (array[i] < min)
                {
                    min = array[i];
                }
            }

            int brojBucketa = max - min + 1;

            //kreiranje liste bucket-a
            List<int>[] buckets = new List<int>[brojBucketa];
            for (int i = 0; i < brojBucketa; i++)
            {
                buckets[i] = new List<int>();
            }

            //dodavanje elementa niza u odgovarajući Bucket
            for (int i = 0; i < array.Length; i++)
            {
                int chosenBucket = (array[i] - min);
                buckets[chosenBucket].Add(array[i]);
            }

            //vracanje elemenata u niz
            int k = 0;
            for (int i = 0; i < buckets.Length; i++)
            {
                for (int j = 0; j < buckets[i].Count; j++)
                {
                    array[k] = buckets[i][j];
                    k++;
                }
            }

            DateTime end = DateTime.Now;
            long memoryAfter = GC.GetTotalMemory(true);  // Get memory usage after function
            // Vreme izvrsenja:  
            long memoryUsed = memoryAfter - memoryBefore;
            TimeSpan ts = (end - start);
            Console.WriteLine("The execution time of the program is {0} ms", ts.TotalMilliseconds);
            Console.WriteLine("The memory used for this function is: {0} bytes", memoryUsed.ToString());
        }
    }
}
