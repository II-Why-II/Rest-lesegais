using System;
using System.Data.SqlClient;

namespace Parser_Lesegais_ru.DataBase
{
    class MethodsWithMSSQL
    {
        private SqlConnection sqlConnection;
        public void TableCreatorIfTableNotExists(string name)
        {
            try
            {
                OpenConnection();
                CreateTableIfNotExists(name);
                CloseConnection();
            }
            catch(Exception ex)
            {
                Console.WriteLine("MS SQL error: " + ex.Message);
            }
        }
    
        public void OpenConnection()
        {
            try
            {
                sqlConnection = DBUtils.GetMSSQLDBConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void CloseConnection()
        {
            try
            {
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CreateTableIfNotExists(string tableName)
        {
            var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText =
            $@"
                IF EXISTS(
                SELECT * FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_NAME = '{tableName}')
                SELECT 'found' AS search_result ELSE SELECT 'not found' AS search_result;";
            try
            {
                var resultReader = sqlCommand.ExecuteReader();
                while (resultReader.Read())
                {
                    var r = resultReader.GetString(0);
                    if (r == "not found")
                    {
                        sqlCommand.CommandText =
                        $@"
                            CREATE TABLE {tableName}(
                            BuyerInn INT, 
                            BuyerName VARCHAR,
                            DealDate INT,
                            DealNumber INT PRIMARY KEY,
                            SellerInn INT,
                            SellerName VARCHAR,
                            WoodVolumeBuyer FLOAT,
                            WoodVolumeSeller FLOAT)";
                        sqlCommand.ExecuteNonQuery();
                    }
                    resultReader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void AddWoodDealToTable(TheSearchReportWoodDeal.Content woodDealModel)
        {
            SqlCommand insertCommand = sqlConnection.CreateCommand();
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
                var queryCommand = sqlConnection.CreateCommand();
                queryCommand.CommandText =
                @"
                    SELECT SellerInn
                    FROM WoodDeal
                ";

                SqlDataReader valueReader = queryCommand.ExecuteReader();
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
                var queryCommand = sqlConnection.CreateCommand();
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
