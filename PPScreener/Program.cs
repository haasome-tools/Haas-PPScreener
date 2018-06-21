using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

using Newtonsoft.Json;
using Haasonline.Public.LocalApi.CSharp;
using Haasonline.Public.LocalApi.CSharp.Enums;

namespace PPScreener
{

    public class PPScrenerConfig
    {
        public string IPAddress { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 8096;
        public string Secret { get; set; } = "SomeSecretHere";
        public string PrimarySecondaryCurrency { get; set; } = "BTC";
        public string AccountGUID { get; set; } = "ReplaceMeWithGuid";

        
        public decimal Fee { get; set; } = 0.1m;
        public int DelayBTInMiliseconds = 1000;
        public int ExchangeSelection { get; set; } = 1;
        public int MinutesToBackTest { get; set; } = 1440;
        public bool CollectDataFirst { get; set; } = false;

    }

    public class Program
    {

        public static string configFileName = "PPScreener.json";

        public static PPScrenerConfig mainConfig;

        public static void ShowBanner()
        {
            Console.WriteLine("___  ___  ____ ____ ____ ____ ____ _  _ ____ ____ ");
            Console.WriteLine("|__] |__] [__  |    |__/ |___ |___ |\\ | |___ |__/ ");
            Console.WriteLine("|    |    ___] |___ |  \\ |___ |___ | \\| |___ |  \\ ");
            Console.WriteLine("By: R4stl1n");
            Console.WriteLine();
            Console.WriteLine();
        }

        public static bool CheckForConfig()
        {
            Program.mainConfig = new PPScrenerConfig();

            if (File.Exists(Program.configFileName))
            {
                Program.mainConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<PPScrenerConfig>(File.ReadAllText(Program.configFileName));
                return true;
            }

            // Create a config
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(Program.mainConfig);
            File.WriteAllText(Program.configFileName, json);
            Console.WriteLine("[*] PPScreener.json Created Succesfully");
            Console.WriteLine("[!] Config Not Found, config created please modify and then relaunch.");

            return false;
        }

        public static string GetWorkingAccountGUID()
        {

            if (mainConfig.AccountGUID.Equals("ReplaceMeWithGuid"))
            {
                HaasonlineClient haasonlineClient = new HaasonlineClient(mainConfig.IPAddress, mainConfig.Port, mainConfig.Secret);

                var accounts = haasonlineClient.AccountDataApi.GetEnabledAccounts();

                Console.WriteLine("[!] Please Take Note Of The Account GUID And Add To Your Config");
                // Quick hacky to get a key
                foreach (string x in accounts.Result.Result.Keys)
                {
                    Console.WriteLine("[-] {0} : {1}", accounts.Result.Result[x], x);
                }
            }
            else
            {
                return mainConfig.AccountGUID;
            }

            return "";
        }

        public static bool CheckConnection()
        {
            try
            {
                Console.WriteLine("[*] Verifying API Connection and Credentials");
                HaasonlineClient haasonlineClient = new HaasonlineClient(mainConfig.IPAddress, mainConfig.Port, mainConfig.Secret);

                var results = haasonlineClient.TestCreditials();

                Thread.Sleep(1000);

                return results.Result;
            }
            catch
            {
                return false;
            }

        }


        static void Main(string[] args)
        {

            Program.ShowBanner();

            Console.WriteLine("[*] Starting up PPScreener for Haasonline");
            Console.WriteLine("[*] Checking for config");

            if (Program.CheckForConfig())
            {

                if (CheckConnection())
                {

                    var workingAccountGUID = Program.GetWorkingAccountGUID();

                    if (!workingAccountGUID.Equals(""))
                    {

                        HaasonlineClient haasonlineClient = new HaasonlineClient(mainConfig.IPAddress, mainConfig.Port, mainConfig.Secret);

                        var markets = haasonlineClient.MarketDataApi.GetPriceMarkets((EnumPriceSource)mainConfig.ExchangeSelection);

                        List<string> marketsToList = new List<string>();

                        foreach(var market in markets.Result.Result)
                        {
                            if (market.SecondaryCurrency.Equals(mainConfig.PrimarySecondaryCurrency))
                            {
                                marketsToList.Add(market.PrimaryCurrency);
                            }
                        }

                        Console.WriteLine("[*] Loaded {0} Markets To Screen", marketsToList.Count);

                        Console.WriteLine();

                        int count = 0;

                        // YES YES I KNOW THIS IS BAD BUT I WAS LAZY
                        LoadChartsFirst:

                        foreach (string market in marketsToList)
                        {
                            string botName = "PP-" + market + ":" + mainConfig.PrimarySecondaryCurrency;

                            var newBot = haasonlineClient.CustomBotApi.NewBot(Haasonline.Public.LocalApi.CSharp.Enums.EnumCustomBotType.PingPongBot, botName, workingAccountGUID, market, mainConfig.PrimarySecondaryCurrency, "");
                            Thread.Sleep(1000);
                            var allBots = haasonlineClient.CustomBotApi.GetAllBots();

                            if (mainConfig.CollectDataFirst.Equals(false))
                            {
                                Console.WriteLine("[*] Currently Testing: {0}:{1}", market, mainConfig.PrimarySecondaryCurrency);
                            } else
                            {
                                Console.Write("\r[*] Currently Loading Market: {0} - {1}", count, marketsToList.Count);
                            }

                            // Find the Bot guid
                            foreach (var bot in allBots.Result.Result)
                            {
                                if (bot.Name.Equals(botName))
                                {
                                    count = count + 1;

                                    var setupScalpBot = haasonlineClient.CustomBotApi.SetupPingPongBot(bot.GUID, bot.Name, workingAccountGUID, market, mainConfig.PrimarySecondaryCurrency, "", 0, 1000, mainConfig.Fee, mainConfig.PrimarySecondaryCurrency);
                                    var backtestBot = haasonlineClient.CustomBotApi.BacktestBot<Haasonline.Public.LocalApi.CSharp.DataObjects.CustomBots.BaseCustomBot>(bot.GUID, mainConfig.MinutesToBackTest);
                                    Thread.Sleep(mainConfig.DelayBTInMiliseconds);
                                    var botResults = haasonlineClient.CustomBotApi.GetBot<Haasonline.Public.LocalApi.CSharp.DataObjects.CustomBots.BaseCustomBot>(bot.GUID);

                                    if (mainConfig.CollectDataFirst.Equals(false))
                                    {
                                        if (botResults.Result.Result.ROI > 0.0m)
                                        {
                                            Console.WriteLine("[*] {0} - ROI: {1:N4}%", botName, botResults.Result.Result.ROI);
                                        }
                                        else
                                        {
                                            Console.WriteLine("[*] {0} - ROI: {1:N4}% - Automatically Deleted", botName, botResults.Result.Result.ROI);
                                            haasonlineClient.CustomBotApi.RemoveBot(bot.GUID);
                                        }
                                    } else
                                    {
                                        haasonlineClient.CustomBotApi.RemoveBot(bot.GUID);
                                    }
                                }
                            }
                        }

                        count = 0;
                        if (mainConfig.CollectDataFirst)
                        {
                            Console.WriteLine();
                            mainConfig.CollectDataFirst = false;
                            goto LoadChartsFirst;
                        }
                    }
                    else
                    {
                        Console.WriteLine("[!] Please Update Your Config File And Try Again");
                    }
                }
                else
                {
                    Console.WriteLine("[!] Connection failed please verify IP, Port and Secret in config");
                }


            }
            Console.WriteLine("[***] Completed");
            Console.ReadLine();
        }
    }
}
