using System;
using Microsoft.Data.Sqlite;

namespace Parser_Lesegais_ru.DataBase
{
    class MethodsWithSQLite
    {
        private SqliteConnection sqliteConnection;
        public void TableCreatorIfTableNotExists(string name)
        {
            try
            {
                OpenConnection();
                CreateTableIfNotExists(name);
                CloseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("SQLite error: " + ex.Message);
            }
        }
        public void OpenConnection()
        {
            try
            {
                sqliteConnection = DataBase.DBUtils.GetSQLiteDBConnection();
                sqliteConnection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _ = 1;
            }
        }
        public void CloseConnection()
        {
            try
            {
                sqliteConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CreateTableIfNotExists(string tableName)
        {
            var sqliteCommand = sqliteConnection.CreateCommand();
            sqliteCommand.CommandText =
            $@"
                CREATE TABLE IF NOT EXISTS {tableName}(
                    BuyerInn INTEGER, 
                    BuyerName TEXT,
                    DealDate TEXT,
                    DealNumber INTEGER PRIMARY KEY,
                    SellerInn INTEGER,
                    SellerName TEXT,
                    WoodVolumeBuyer FLOAT,
                    WoodVolumeSeller FLOAT 
            )";
            try
            {
                sqliteCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void AddWoodDealToTable(TheSearchReportWoodDeal.Content woodDealModel)
        {
            SqliteCommand insertCommand = sqliteConnection.CreateCommand();
            insertCommand.CommandText =
            @"
                INSERT INTO WoodDeal (BuyerInn, BuyerName, DealDate, DealNumber, SellerInn, SellerName, WoodVolumeBuyer, WoodVolumeSeller) 
                VALUES ('" + woodDealModel.BuyerInn + "','" + woodDealModel.BuyerName + "','" + woodDealModel.DealDate + "','" + woodDealModel.DealNumber + "','" + woodDealModel.SellerInn + "','" + woodDealModel.SellerName + "','" + woodDealModel.WoodVolumeBuyer + "','" + woodDealModel.WoodVolumeSeller + "')";
            try
            {
                insertCommand.ExecuteNonQuery();
                Console.WriteLine("+1");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void GetDataFromDBToConsole()
        {
            try
            {
                var queryCommand = sqliteConnection.CreateCommand();
                queryCommand.CommandText =
                @"
                    SELECT SellerInn
                    FROM WoodDeal
                ";

                SqliteDataReader valueReader = queryCommand.ExecuteReader();
                while (valueReader.Read())
                {
                    if (valueReader.HasRows)
                    {
                        object obj = valueReader["SellerInn"];
                        Console.WriteLine(obj);
                    }
                }
                valueReader.Close();
                _ = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error get data from db:" + ex.Message);
            }
        }
        public bool? ExistenceInTableOrNull(TheSearchReportWoodDeal.Content woodDealModel)
        {
            try
            {
                var queryCommand = sqliteConnection.CreateCommand();
                queryCommand.CommandText =
                @"
                    SELECT count(*)
                    FROM WoodDeal
                    WHERE DealNumber='" + woodDealModel.DealNumber + "'";

                int count = Convert.ToInt32(queryCommand.ExecuteScalar());
                if (count == 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
