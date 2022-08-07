﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parser_Lesegais_ru.Parser.Starter
{
    class StarterParse
    {
        public void BigParseInfo()
        {
            DataBase.MethodsWithMSSQL mssql = new DataBase.MethodsWithMSSQL();
            mssql.TableCreatorIfTableNotExists("WoodDeal");
            DataBase.MethodsWithSQLite sqlite = new DataBase.MethodsWithSQLite();
            sqlite.TableCreatorIfTableNotExists("WoodDeal");

            int sizeOneRequest = 1000;
            int? maxPage = 10;

            try
            {
                for (int page = 1; page < maxPage; page++)
                {
                    try
                    {
                        //MakeQuery.WebClient.SendDataViaWebClient webClient = new MakeQuery.WebClient.SendDataViaWebClient();
                        //TheSearchReportWoodDeal.Content[] contentByWebClient = webClient.getWoodDealContentsOrNull(page, sizeOneRequest);
                        //_ = 1;

                        //MakeQuery.GraphQl.Request request = new MakeQuery.GraphQl.Request();
                        //TheSearchReportWoodDeal.Content[] contentByGraphQl = request.GetContent(page, sizeOneRequest);
                        //_ = 1;

                        MakeQuery.HttpRequests.SendRequestWithHttpWebRequest httpWebRequest = new MakeQuery.HttpRequests.SendRequestWithHttpWebRequest();
                        TheSearchReportWoodDeal.Content[] contentByHttpWebRequest = httpWebRequest.getWoodDealContentsOrNull(page, sizeOneRequest);
                        _ = 1;

                        sqlite.OpenConnection();
                        mssql.OpenConnection();

                        foreach (var reportWoodDeal in contentByHttpWebRequest) //contentByWebClient //contentByGraphQl
                        {
                            try
                            {
                                bool? existenceValueInSqliteTable = sqlite.ExistenceInTableOrNull(reportWoodDeal);
                                if (existenceValueInSqliteTable == false)
                                    sqlite.AddWoodDealToTable(reportWoodDeal);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error check existence or adding to sqlite:" + ex.Message);
                            }
                            try
                            {
                                bool? existenceValueInSqlTable = mssql.ExistenceInTableOrNull(reportWoodDeal);
                                if (existenceValueInSqlTable == false)
                                    mssql.AddWoodDealToTable(reportWoodDeal);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error check existence or adding to sql server:" + ex.Message);
                            }
                            _ = 1;
                        }

                        sqlite.GetDataFromDBToConsole();
                        mssql.GetDataFromDBToConsole();

                        var cachOfMaxPage = httpWebRequest.GetMaxPageOrNull(sizeOneRequest);
                        if (cachOfMaxPage != null)
                            maxPage = cachOfMaxPage;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        sqlite.CloseConnection();
                        mssql.CloseConnection();
                        Thread.Sleep(5000);
                        _ = 1;
                    }
                }
                _ = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlite.GetDataFromDBToConsole();
                sqlite.CloseConnection();
            }
        }
    }
}
