using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            char[] niz1 = { 'P', 'T', 'T', 'P', 'T' };
            int k1 = 1;
            PrintArray(niz1, k1);
            Console.WriteLine(MaxLopovi(niz1, k1)); //treba rezultat da bude 2

            char[] niz2 = { 'T', 'T', 'P', 'P', 'T', 'P' };
            int k2 = 2;
            PrintArray(niz2, k2);
            Console.WriteLine(MaxLopovi(niz2, k2)); //treba rezultat da bude 3

            char[] niz3 = { 'P', 'T', 'P', 'T', 'T', 'P' }; 
            int k3 = 3;
            PrintArray(niz3, k3);
            Console.WriteLine(MaxLopovi(niz3, k3)); //treba rezultat da bude 3

            char[] niz4 = { 'P', 'T', 'T', 'P', 'T', 'P' };
            int k4 = 1;
            PrintArray(niz4, k4);
            Console.WriteLine(MaxLopovi(niz4, k4)); //treba rezultat da bude 3

            char[] niz5 = { 'P', 'T', 'T', 'P', 'T', 'P' };
            int k5 = 2;
            PrintArray(niz5, k5);
            Console.WriteLine(MaxLopovi(niz5, k5)); //treba rezultat da bude 3

            char[] niz6 = {'T', 'T', 'P', 'P', 'T'};
            int k6 = 2;
            PrintArray(niz6, k6);
            Console.WriteLine(MaxLopovi(niz6, k6)); //treba rezultat da bude 2

            char[] niz7 = {'T', 'T' ,'P' , 'P', 'P', 'P'};
            int k7 = 2;
            PrintArray(niz7, k7);
            Console.WriteLine(MaxLopovi(niz7, k7)); //treba rezultat da bude 2
        }

        public static void PrintArray(char[] niz, int k)
        {
            for(int i = 0; i < niz.Length; i++)
            {
                Console.Write(niz[i] + " ");
            }

            Console.Write(", k = " + k);
            Console.Write(" ---> ");
        }
        public static int MaxLopovi(char[] niz, int k)
        {
            int i = 0; //za pozicije u nizu
            int brUhvacenih = 0; //pamti broj lopova koji su uhvaceni
            bool pronadjen = false; //ovo mi sluzi za uspostavljanja uslova da svaki policajac moze da uhvati 1 lopova

            while (i < niz.Length)
            {
                if (niz[i] == 'P') //pronalazimo policajca
                {
                    int j = Math.Max(0, i - k); //pocetni indeks koji mi sluzi za proveru lopova u okolini policajca
                    while (j <= i + k && j < niz.Length && pronadjen == false) //j moze da ide do pozicije koja je jednaka indeksu policajca + k mesta
                    {
                        if (niz[j] == 'T') //provera da li je nadjen lopov
                        {
                            brUhvacenih++;
                            pronadjen = true;
                            niz[j] = 'X'; // Obeležavamo uhvaćenog lopova kako ne bismo uhvatili istog lopova više puta.
                        }
                        j++;
                    }

                    pronadjen = false;
                }
                i++;
            }

            return brUhvacenih;
        }
    }
}
