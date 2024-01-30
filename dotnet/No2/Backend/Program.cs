using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("Masukkan kata-kata pertama (pisahkan dengan koma):");
        string[] firstWords = Console.ReadLine().Split(',');

        Console.WriteLine("Masukkan kata-kata kedua (pisahkan dengan koma):");
        string[] secondWords = Console.ReadLine().Split(',');

        string result = CheckAnagrams(firstWords, secondWords);
        Console.WriteLine(result);
    }

    static string CheckAnagrams(string[] firstWords, string[] secondWords)
    {
        if (firstWords.Length != secondWords.Length)
        {
            throw new ArgumentException("Jumlah kata dalam kedua array harus sama.");
        }

        List<int> results = new List<int>();

        for (int i = 0; i < firstWords.Length; i++)
        {
            int isAnagram = AreAnagrams(firstWords[i], secondWords[i]) ? 1 : 0;
            results.Add(isAnagram);
        }

        return string.Join("", results);
    }

    static bool AreAnagrams(string word1, string word2)
    {
        // Mengubah kedua kata menjadi array karakter
        char[] charArray1 = word1.ToCharArray();
        char[] charArray2 = word2.ToCharArray();

        // Mengurutkan array karakter
        Array.Sort(charArray1);
        Array.Sort(charArray2);

        // Membandingkan apakah array karakter setelah diurutkan sama
        return new string(charArray1) == new string(charArray2);
    }
}
