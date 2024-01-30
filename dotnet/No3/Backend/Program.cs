using System;
using MySql.Data.MySqlClient;

class Program
{
    static void Main()
    {
        // Informasi koneksi database
        string connectionString = "Server=localhost;Database=vdi_test;User ID=root;Password=;";

        // Input data transaksi dari pengguna
        Console.WriteLine("Masukkan informasi transaksi:");

        Console.Write("Tipe Customer (platinum/gold/silver): ");
        string? tipeCustomer = Console.ReadLine();

        Console.Write("Point Reward: ");
        int pointReward;
        while (!int.TryParse(Console.ReadLine(), out pointReward))
        {
            Console.WriteLine("Masukkan angka yang valid untuk Point Reward.");
            Console.Write("Point Reward: ");
        }

        Console.Write("Total Belanja: ");
        decimal totalBelanja;
        while (!decimal.TryParse(Console.ReadLine(), out totalBelanja))
        {
            Console.WriteLine("Masukkan angka yang valid untuk Total Belanja.");
            Console.Write("Total Belanja: ");
        }

        // Data transaksi
        string transaksiId = GenerateTransaksiId();
        decimal diskon = HitungDiskon(tipeCustomer, pointReward, totalBelanja);
        decimal totalBayar = totalBelanja - diskon;

        // Menyimpan data transaksi ke database
        SimpanDataTransaksi(connectionString, transaksiId, tipeCustomer, pointReward, totalBelanja, diskon);

        // Menampilkan informasi
        Console.WriteLine("Data transaksi berhasil disimpan ke database.");
        Console.WriteLine($"Diskon: {diskon:C}");
        Console.WriteLine($"Total Bayar: {totalBayar:C}");
    }


    static void SimpanDataTransaksi(string connectionString, string transaksiId, string? tipeCustomer, int pointReward, decimal totalBelanja, decimal diskon)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "INSERT INTO Transaksi (TransaksiID, TipeCustomer, PointReward, TotalBelanja, Diskon, TotalBayar) " +
                        "VALUES (@TransaksiID, @TipeCustomer, @PointReward, @TotalBelanja, @Diskon, (@TotalBelanja - @Diskon))";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@TransaksiID", transaksiId);
                cmd.Parameters.AddWithValue("@TipeCustomer", tipeCustomer ?? "");
                cmd.Parameters.AddWithValue("@PointReward", pointReward);
                cmd.Parameters.AddWithValue("@TotalBelanja", totalBelanja);
                cmd.Parameters.AddWithValue("@Diskon", diskon);

                cmd.ExecuteNonQuery();
            }
        }
    }


    static string GenerateTransaksiId()
    {
        string tanggal = DateTime.Now.ToString("yyyyMMdd");
        int runningNumber = AmbilRunningNumberDariDatabase() ?? 1;

        while (CekTransaksiIdDalamDatabase($"{tanggal}_{runningNumber:D5}"))
        {
            runningNumber++;
        }

        SimpanRunningNumberKeDatabase(runningNumber + 1);
        return $"{tanggal}_{runningNumber:D5}";
    }

    static bool CekTransaksiIdDalamDatabase(string transaksiId)
    {
        using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=perhitungan_diskon;User ID=root;Password=;"))
        {
            connection.Open();

            string query = "SELECT COUNT(*) FROM Transaksi WHERE TransaksiID = @TransaksiID";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@TransaksiID", transaksiId);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
    }


    static int? AmbilRunningNumberDariDatabase()
    {
        using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=perhitungan_diskon;User ID=root;Password=;"))
        {
            connection.Open();

            string query = "SELECT Number FROM RunningNumber";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return Convert.ToInt32(reader["Number"]);
                    }
                }
            }
        }

        return null;
    }

    static void SimpanRunningNumberKeDatabase(int runningNumber)
    {
        using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=perhitungan_diskon;User ID=root;Password=;"))
        {
            connection.Open();

            string query = "INSERT INTO RunningNumber (Number) VALUES (@RunningNumber) " +
                        "ON DUPLICATE KEY UPDATE Number = @RunningNumber";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@RunningNumber", runningNumber);

                cmd.ExecuteNonQuery();
            }
        }
    }

        static decimal HitungDiskon(string? tipeCustomer, int pointReward, decimal totalBelanja)
    {
        decimal diskon = 0;

        if (tipeCustomer?.ToLower() == "platinum")
        {
            if (pointReward >= 100 && pointReward <= 300)
                diskon = totalBelanja * (50 / 100m) + 35;
            else if (pointReward >= 301 && pointReward <= 500)
                diskon = totalBelanja * (50 / 100m) + 50;
            else if (pointReward > 500)
                diskon = totalBelanja * (50 / 100m) + 68;
        }
        else if (tipeCustomer?.ToLower() == "gold")
        {
            if (pointReward >= 100 && pointReward <= 300)
                diskon = totalBelanja * (25 / 100m) + 25;
            else if (pointReward >= 301 && pointReward <= 500)
                diskon = totalBelanja * (25 / 100m) + 34;
            else if (pointReward > 500)
                diskon = totalBelanja * (25 / 100m) + 52;
        }
        else if (tipeCustomer?.ToLower() == "silver")
        {
            if (pointReward >= 100 && pointReward <= 300)
                diskon = totalBelanja * (10 / 100m) + 12;
            else if (pointReward >= 301 && pointReward <= 500)
                diskon = totalBelanja * (10 / 100m) + 27;
            else if (pointReward > 500)
                diskon = totalBelanja * (10 / 100m) + 39;
        }

        return diskon;
    }
}