using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace TestingMe
{
    class Program
    {

        static List<int> GetKontrahents(int Nr)
        {
            List<int> kontrahents = new List<int>();
            using (var connection = new DBConnection("Server=localhost;Port=3306;Database=testingme;User Id=root;Password="))
            {
                MySqlCommand GetCity = new MySqlCommand($"SELECT p2.Miasto from kontrahent p1 LEFT JOIN kodpocztowy p2 on p1.KodPocztowy = p2.Kod Where p1.Nr = '{Nr}' ", connection.GetConnection());
                DataTable tableCity = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(GetCity);

                adapter.Fill(tableCity);

                var city = tableCity.Rows[0][0].ToString();

                MySqlCommand GetKontrahent = new MySqlCommand($"SELECT Nr from kontrahent p1 LEFT JOIN kodpocztowy p2 on p1.KodPocztowy = p2.Kod Where p2.Miasto = '{city}'", connection.GetConnection());
                MySqlDataReader readerNr = GetKontrahent.ExecuteReader();

                while (readerNr.Read())
                {
                    kontrahents.Add(readerNr.GetInt32("Nr"));
                    Console.WriteLine(readerNr.GetInt32("Nr"));
                }
            }

            return kontrahents;
        }

        static void Main(string[] args)
        {
            List<int> kontrahents = GetKontrahents(3);
        }

    }
    class Kontrahent
    {
        public int Nr { get; set; }
        public string Nazwa { get; set; }
        public string KodPocztowy { get; set; }

    }
    class DBConnection : IDisposable
    {

        MySqlConnection connection;
        public DBConnection(string connectionString)
        {
            connection = new MySqlConnection(connectionString);
        }

        public MySqlConnection GetConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }
        public void Dispose()
        {
            connection.Close();
        }
    }
}
