using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_18627
{
    internal class Program
    {
        static Random rand = new Random();

        static void Main(string[] args)
        {
            //100 reci
            //GenerateFile("hexadecimal100.txt", 100, GenerateRandomHexadecimalString); 
            //GenerateFile("ascii100.txt", 100, GenerateRandomASCIIString);

            //1000 reci
            //GenerateFile("hexadecimal1000.txt", 1000, GenerateRandomHexadecimalString); 
            //GenerateFile("ascii1000.txt", 1000, GenerateRandomASCIIString);

            //10 000 reci
            //GenerateFile("hexadecimal10000.txt", 10000, GenerateRandomHexadecimalString); 
            //GenerateFile("ascii10000.txt", 10000, GenerateRandomASCIIString);

            //100 000 reci
            GenerateFile("hexadecimal100000.txt", 100000, GenerateRandomHexadecimalString); 
            GenerateFile("ascii100000.txt", 100000, GenerateRandomASCIIString);

            //100
            //ReplaceWordsInFile("hexadecimal100.txt", "AACCK", 10); //5 duzina 
            //ReplaceWordsInFile("ascii100.txt", "!?a11", 10);

            //ReplaceWordsInFile("hexadecimal100.txt", "AACCKAACCK", 10); //10 duzina
            //ReplaceWordsInFile("ascii100.txt", "!?a11!?a11", 10);

            //ReplaceWordsInFile("hexadecimal100.txt", "AACCKAACCKAACCKAACCK", 10); //20 duzina
            //ReplaceWordsInFile("ascii100.txt", "!?a11!?a11!?a11!?a11", 10);

            //ReplaceWordsInFile("hexadecimal100.txt", "AACCKAACCKAACCKAACCKAACCKAACCKAACCKAACCKAACCKAACCK", 10); //50 duzina
            //ReplaceWordsInFile("ascii100.txt", "!?a11!?a11!?a11!?a11!?a11!?a11!?a11!?a11!?a11!?a11", 10);

            //1000
            //ReplaceWordsInFile("hexadecimal1000.txt", "AACCK", 10); //5 duzina 
            //ReplaceWordsInFile("ascii1000.txt", "!?a11", 10);

            //ReplaceWordsInFile("hexadecimal1000.txt", "AACCKAACCK", 10); //10 duzina
            //ReplaceWordsInFile("ascii1000.txt", "!?a11!?a11", 10);

            //ReplaceWordsInFile("hexadecimal1000.txt", "AACCKAACCKAACCKAACCK", 10); //20 duzina
            //ReplaceWordsInFile("ascii1000.txt", "!?a11!?a11!?a11!?a11", 10);

            //ReplaceWordsInFile("hexadecimal1000.txt", "AACCKAACCKAACCKAACCKAACCKAACCKAACCKAACCKAACCKAACCK", 10); //50 duzina
            //ReplaceWordsInFile("ascii1000.txt", "!?a11!?a11!?a11!?a11!?a11!?a11!?a11!?a11!?a11!?a11", 10);

            ////10000
            //ReplaceWordsInFile("hexadecimal10000.txt", "AACCK", 10); //5 duzina 
            //ReplaceWordsInFile("ascii10000.txt", "!?a11", 10);

             //ReplaceWordsInFile("hexadecimal10000.txt", "AACCKAACCK", 10); //10 duzina
            //ReplaceWordsInFile("ascii10000.txt", "!?a11!?a11", 10);

            //ReplaceWordsInFile("hexadecimal10000.txt", "AACCKAACCKAACCKAACCK", 10); //20 duzina
            //ReplaceWordsInFile("ascii10000.txt", "!?a11!?a11!?a11!?a11", 10);

            //ReplaceWordsInFile("hexadecimal10000.txt", "AACCKAACCKAACCKAACCKAACCKAACCKAACCKAACCKAACCKAACCK", 10); //50 duzina
            //ReplaceWordsInFile("ascii10000.txt", "!?a11!?a11!?a11!?a11!?a11!?a11!?a11!?a11!?a11!?a11", 10);

            ////100000
            //ReplaceWordsInFile("hexadecimal100000.txt", "AACCK", 10); //5 duzina 
            //ReplaceWordsInFile("ascii100000.txt", "!?a11", 10);

            //ReplaceWordsInFile("hexadecimal100000.txt", "AACCKAACCK", 10); //10 duzina
            //ReplaceWordsInFile("ascii100000.txt", "!?a11!?a11", 10);

            //ReplaceWordsInFile("hexadecimal100000.txt", "AACCKAACCKAACCKAACCK", 10); //20 duzina
            //ReplaceWordsInFile("ascii100000.txt", "!?a11!?a11!?a11!?a11", 10);

            ReplaceWordsInFile("hexadecimal100000.txt", "AACCKAACCKAACCKAACCKAACCKAACCKAACCKAACCKAACCKAACCK", 10); //50 duzina
            ReplaceWordsInFile("ascii100000.txt", "!?a11!?a11!?a11!?a11!?a11!?a11!?a11!?a11!?a11!?a11", 10);


            string tekst;

            using (StreamReader reader = new StreamReader("hexadecimal100000.txt")) //ubacite ime fajla u kojem trazimo string
            {
                tekst = reader.ReadToEnd();
            }

            DateTime start = DateTime.Now;
            RabinKarp(tekst, "AACCKAACCKAACCKAACCKAACCKAACCKAACCKAACCKAACCKAACCK"); //izmenite string koji trazite
            //FindWords(tekst, "!?a11!?a11!?a11!?a11!?a11!?a11!?a11!?a11!?a11!?a11"); //izmenite string koji trazite
            DateTime end = DateTime.Now;
            TimeSpan ts = (end - start);
            Console.WriteLine("The execution time of the program is {0} ms", ts.TotalMilliseconds);

        }

        public static string GenerateRandomHexadecimalString(int length)
        {
            const string alphabet = "0123456789ABCDEF";
            StringBuilder sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                sb.Append(alphabet[rand.Next(alphabet.Length)]);
            }

            return sb.ToString();
        }

        public static string GenerateRandomASCIIString(int length)
        {
            const string alphabet = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            StringBuilder sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                sb.Append(alphabet[rand.Next(alphabet.Length)]);
            }

            return sb.ToString();
        }

        public static void GenerateFile(string fileName, int wordCount, Func<int, string> stringGenerator)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                for (int i = 0; i < wordCount; i++)
                {
                    
                    string word = stringGenerator(rand.Next(20));
                    writer.Write(word + " ");
                }

                //writer.WriteLine();
            }
        }

        public static int LevenshteinDistance(string s1, string s2)
        {
            //napraviti prvo tabelu koja ce da kreira matricu gde je br. elem. s1 vrsta, a br. elem. s2 kolona
            int[][] matrica = new int[s1.Length + 1][];
            for(int i = 0; i < s1.Length + 1; i++)
            {
                matrica[i] = new int[s2.Length + 1];
            }

            for(int i = 0; i < s1.Length + 1; i++)
            {
                matrica[i][0] = i; //napuni prvu vrstu
            }

            for(int j = 0; j < s2.Length + 1; j++)
            {
                matrica[0][j] = j; //napuni prvu kolonu
            }

            int cost;
            for(int j=1; j < s2.Length + 1; j++)
            {
                for(int i=1; i < s1.Length + 1; i++)
                {
                    if (s1[i - 1] == s2[j - 1])
                    {
                        cost = 0;
                    }
                    else
                    {
                        cost = 1;
                    }

                   int pom = Math.Min(matrica[i - 1][j] + 1, matrica[i][j - 1] + 1);
                   matrica[i][j] = Math.Min(pom, matrica[i - 1][j - 1] + cost);
                }
            }

            return matrica[s1.Length][s2.Length];
        }

        //pomocna fja za implementaciju Levenshtein algoritma
        public static void FindWords(string tekst, string patern)
        {
            string[] words = tekst.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            List<string> closeWords = new List<string>();

            foreach (string word in words)
            {
                int distance = LevenshteinDistance(patern, word);

                if (distance <= 3)
                    closeWords.Add(word);
            }

            // Write close words to the output file
            using (StreamWriter writer = new StreamWriter("noviLevenshtein.txt"))
            {
                foreach (string closeWord in closeWords)
                {
                    writer.WriteLine(closeWord);
                }
            }
        }

        public static void RabinKarp(string tekst, string patern)
        {
            int n = tekst.Length;
            int m = patern.Length;

            const int d = 256;
            const int q = 191; //odabran prost broj tako da se vrednost 10q taman uklapa u jednu racunarsku rec

            //h <- d^(m-1) mod q
            int h = 1;
            for(int i = 0; i< m-1; i++)
            {
                h = (h * d) % q;
            }

            int p = 0; //odgovarajuca dekadna vrednost stringa koji trazimo
            int ts = 0; //odgovarajuca dekadna vrednost podstringa duzine m

            for(int i = 0; i< m; i++)
            {
                p = (p * d + patern[i]) % q;
                ts = (ts * d + tekst[i]) % q;
            }

            //trazene paterna u prosledenom tekstu
            using (StreamWriter sw = new StreamWriter("noviRabinKarp.txt"))
            {
                for (int s = 0; s <= n - m; s++)
                {
                    if (p == ts) //ako se dekadna vrednost karaktera poklapa sa podstringom teksta provera string po string
                    { // provera ide zbog mogucih spurious hits
                        int j = 0;
                        while (j < m && patern[j] == tekst[s + j])
                        {
                            j++;
                        }

                        if (j == m)
                        {
                            //Console.WriteLine("Pattern occurs with shift " + s);
                            string foundString = tekst.Substring(s, m);
                            sw.WriteLine(foundString);
                        }
                    }

                    if (s < n - m)
                    {
                        ts = (d * (ts - tekst[s] * h) + tekst[s + m]) % q;

                        if (ts < 0) //za slucaj da je dobijena negativna hash vrednost
                            ts += q;
                    }
                }
            }
        }
        public static void ReplaceWordsInFile(string filePath, string replacementWord, int numberOfReplacements)
        {
            // Citanje svih linija iz fajla
            string[] lines = File.ReadAllLines(filePath);

            // Nasumicno biranje indeksa reci koje će biti zamenjene
            Random random = new Random();
            for (int i = 0; i < numberOfReplacements; i++)
            {
                int randomLineIndex = random.Next(0, lines.Length);
                string[] words = lines[randomLineIndex].Split(' ');

                if (words.Length > 0)
                {
                    int randomWordIndex = random.Next(0, words.Length);
                    words[randomWordIndex] = replacementWord;
                    lines[randomLineIndex] = string.Join(" ", words);
                }
            }

            // Pisanje promenjenih linija nazad u fajl
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (string line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }
    }
}
