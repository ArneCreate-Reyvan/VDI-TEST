using System;

partial class Program
{
    static void Main()
    {
        Console.WriteLine("Masukkan karakter (Lebih Baik 6 Karakter):");
        string input = Console.ReadLine()!;

        string output = BalikDanSplit(input);
        Console.WriteLine($"Output: {output}");
    }

    static string BalikDanSplit(string input)
{
    if (input == null)
    {
        throw new ArgumentNullException(nameof(input), "Input tidak boleh null.");
    }

    char[] array = input.ToCharArray();
    int length = array.Length;
    int middle = length / 2;

    // Balik kiri
    for (int i = 0; i < middle / 2; i++)
    {
        char temp = array[i];
        array[i] = array[middle - i - 1];
        array[middle - i - 1] = temp;
    }

    // Balik kanan
    for (int i = middle + (length % 2), j = length - 1; i < j; i++, j--)
    {
        char temp = array[i];
        array[i] = array[j];
        array[j] = temp;
    }

    return new string(array);
}


}