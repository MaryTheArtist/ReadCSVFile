using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace ReadCSVFile
{
    class Program
    {
        static void Main(string[] args)
        {
          
            string filePath = @"C:\Users\Valkovi\Desktop\New folder (3)\offers.csv";
            bool isFirstLine = true;


            StreamReader reader = null;
            if (File.Exists(filePath))
            {
                using (SQLiteConnection con = new SQLiteConnection("Data Source=MyDatabase.sqlite;"))
                using (SQLiteCommand command = con.CreateCommand())
                {
                    con.Open();

                    command.CommandText = "CREATE TABLE IF NOT EXISTS offers (offerId INT PRIMARY KEY, monthlyFee REAL, newContractsForMonth INT, sameContractsForMonth INT, CancelledContractsForMonth INT)";
                    command.ExecuteNonQuery();

                    reader = new StreamReader(File.OpenRead(filePath));

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        var values = line.Split(';').ToList();

                        command.CommandText = "INSERT OR IGNORE INTO offers (offerId, monthlyFee, newContractsForMonth, sameContractsForMonth, CancelledContractsForMonth) VALUES (@offerId, @monthlyFee, @newContractsForMonth, @sameContractsForMonth, @CancelledContractsForMonth)";
                        command.Parameters.AddWithValue("@offerId", int.Parse(values[0]));
                        command.Parameters.AddWithValue("@monthlyFee", Math.Round(float.Parse(values[1].Replace(',','.')), 2));
                        command.Parameters.AddWithValue("@newContractsForMonth", int.Parse(values[2]));
                        command.Parameters.AddWithValue("@sameContractsForMonth", int.Parse(values[3]));
                        command.Parameters.AddWithValue("@CancelledContractsForMonth", int.Parse(values[4]));
                        command.ExecuteNonQuery();

                    }
                }
            }
            else
            {
                Console.WriteLine("File doesn't exist");
                return;
            }

            Console.WriteLine("Done.");
        }    
    }
}
