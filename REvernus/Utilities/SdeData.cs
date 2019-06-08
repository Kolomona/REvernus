﻿using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows;

namespace REvernus.Utilities
{
    public static class SdeData
    {
        public static string SdeDataBasePath => Path.Combine(Environment.CurrentDirectory, "Data", "eve.db");
        private static readonly SQLiteConnection SdeDatabaseConnection = new SQLiteConnection($"Data Source={SdeDataBasePath};Version=3;New=True;Compress=true;Read Only=True;FailIfMissing=True");

        public static Task<DataTable> GetInventoryTypesAsync()
        {
            try
            {
                SdeDatabaseConnection.Open();
                var command = SdeDatabaseConnection.CreateCommand();
                command.CommandText = "SELECT * FROM 'invTypes' WHERE marketGroupID IS NOT null";

                var reader = command.ExecuteReader();
                var dataTable = new DataTable();
                dataTable.Load(reader);

                SdeDatabaseConnection.Close();

                return Task.FromResult(dataTable);
            }
            catch (Exception e)
            {
                if (e is SQLiteException)
                {
                    SystemSounds.Asterisk.Play();
                    MessageBox.Show(
                        "There was an error accessing the Static Data Export local database.\nHave you downloaded the Static Data Export under File?", "Error", MessageBoxButton.OK);
                }
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
