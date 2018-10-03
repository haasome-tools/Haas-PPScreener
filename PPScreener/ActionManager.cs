using Haasonline.Public.LocalApi.CSharp;
using Haasonline.Public.LocalApi.CSharp.DataObjects.AccountData;
using Haasonline.Public.LocalApi.CSharp.DataObjects.CustomBots;
using Haasonline.Public.LocalApi.CSharp.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PPScreener
{
    // Meat of the application
    // Performs all real actions
    public static class ActionManager
    {
        public static HaasConfig mainConfig;

        public static string DefaultConfigName = "HaasTool.config";

        public static string BaseBotTemplateGuid = "";

        public static bool LoadConfig()
        {

            if (File.Exists(DefaultConfigName))
            {
                ActionManager.mainConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<HaasConfig>(File.ReadAllText(DefaultConfigName));
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool LoadConfig(string fileName)
        {

            if (File.Exists(fileName))
            {
                ActionManager.mainConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<HaasConfig>(File.ReadAllText(fileName));
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool SaveConfig()
        {
            try
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(ActionManager.mainConfig);

                File.WriteAllText(DefaultConfigName, json);

                return true;
            }
            catch
            {

                return false;
            }

        }
 
        public static bool SaveConfig(string fileName)
        {
            try
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(ActionManager.mainConfig);

                File.WriteAllText(fileName, json);

                return true;
            }
            catch
            {
                return false;
            }

        }

        public static void ShowConfig()
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ActionManager.mainConfig));
        }

        public static void SetConfigIpAddress(string ipAddress)
        {
            ActionManager.mainConfig.IPAddress = ipAddress;
        }

        public static void SetConfigPort(int port)
        {
            ActionManager.mainConfig.Port = port;
        }

        public static void SetConfigSecret(string secret)
        {
            ActionManager.mainConfig.Secret = secret;
        }

        public static void SetConfigAccountGuid(string accountGUID)
        {
            ActionManager.mainConfig.AccountGUID = accountGUID;
        }

        public static void SetConfigKeepThreshold(decimal threshold)
        {
            ActionManager.mainConfig.KeepThreshold = threshold;
        }

        public static void SetConfigBacktestDelay(int delay)
        {
            ActionManager.mainConfig.BackTestDelayInMiliseconds = delay;
        }

        public static void SetConfigFee(decimal fee)
        {
            ActionManager.mainConfig.Fee = fee;
        }

        public static void SetConfigBackTestLength(int length)
        {
            ActionManager.mainConfig.BackTestLength = length;
        }

        public static void SetConfigPrimaryCurrency(string currency)
        {
            ActionManager.mainConfig.PrimaryCurrency = currency;
        }

        public static void SetConfigWriteResultsToFile(bool writeToFile)
        {
            ActionManager.mainConfig.WriteResultsToFile = writeToFile;
        }

        public static void SetConfigPersistBots(bool persist)
        {
            ActionManager.mainConfig.PersistBots = persist;
        }

        public static void SetConfigRetryCount(int count)
        {
            ActionManager.mainConfig.RetryCount = count;   
        }

        public static bool CheckHaasConnection()
        {
            HaasonlineClient haasonlineClient = new HaasonlineClient(ActionManager.mainConfig.IPAddress, ActionManager.mainConfig.Port, ActionManager.mainConfig.Secret);

            var accounts = haasonlineClient.AccountDataApi.GetEnabledAccounts();

            if (accounts.Result.ErrorCode == EnumErrorCode.Success)
            {
                if (ActionManager.GetAccountGUIDS().Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static List<Tuple<string,string>> GetAccountGUIDS()
        {

            List<Tuple<string, string>> results = new List<Tuple<string, string>>();


            HaasonlineClient haasonlineClient = new HaasonlineClient(ActionManager.mainConfig.IPAddress, ActionManager.mainConfig.Port, ActionManager.mainConfig.Secret);

            var accounts = haasonlineClient.AccountDataApi.GetEnabledAccounts();

            // Quick hacky to get a key
            foreach (string x in accounts.Result.Result.Keys)
            {
                results.Add(new Tuple<string, string>(accounts.Result.Result[x], x));
            }
            
            return results;

        }

        public static List<string> GetMarkets()
        {

            List<string> results = new List<string>();

            if (ActionManager.CheckHaasConnection())
            {
                try
                {
                    HaasonlineClient haasonlineClient = new HaasonlineClient(ActionManager.mainConfig.IPAddress, ActionManager.mainConfig.Port, ActionManager.mainConfig.Secret);

                    AccountInformation accountInformation = haasonlineClient.AccountDataApi.GetAccountDetails(mainConfig.AccountGUID).Result.Result;

                    var markets = haasonlineClient.MarketDataApi.GetPriceMarkets(accountInformation.ConnectedPriceSource);

                    foreach (var market in markets.Result.Result)
                    {
                        if (market.SecondaryCurrency.Equals(mainConfig.PrimaryCurrency))
                        {
                            results.Add(market.PrimaryCurrency);
                        }
                    }
                }
                catch
                {
                    return results;
                }
            }

            return results;
        }

        public static string CreateTemplateBot()
        {

            List<string> markets = ActionManager.GetMarkets();

            if (ActionManager.CheckHaasConnection())
            {
                if (markets.Count > 0)
                {
                    HaasonlineClient haasonlineClient = new HaasonlineClient(ActionManager.mainConfig.IPAddress, ActionManager.mainConfig.Port, ActionManager.mainConfig.Secret);
                    var newBot = haasonlineClient.CustomBotApi.NewBot<BaseCustomBot>(Haasonline.Public.LocalApi.CSharp.Enums.EnumCustomBotType.PingPongBot,
                        "PPScreener-Template", mainConfig.AccountGUID, markets[0], mainConfig.PrimaryCurrency, "");

                    ActionManager.BaseBotTemplateGuid = newBot.Result.Result.GUID;
                }
            }

            return ActionManager.BaseBotTemplateGuid;
        }

        public static void DeleteTemplateBot()
        {
            if (ActionManager.CheckHaasConnection())
            {
                HaasonlineClient haasonlineClient = new HaasonlineClient(ActionManager.mainConfig.IPAddress, ActionManager.mainConfig.Port, ActionManager.mainConfig.Secret);
                haasonlineClient.CustomBotApi.RemoveBot(ActionManager.BaseBotTemplateGuid);
            }
        }

        public static void GrabMarketData(string market)
        {
            HaasonlineClient haasonlineClient = new HaasonlineClient(ActionManager.mainConfig.IPAddress, ActionManager.mainConfig.Port, ActionManager.mainConfig.Secret);

            AccountInformation accountInformation = haasonlineClient.AccountDataApi.GetAccountDetails(mainConfig.AccountGUID).Result.Result;

            var task = Task.Run(async () => await haasonlineClient.MarketDataApi.GetHistory(accountInformation.ConnectedPriceSource, market, mainConfig.PrimaryCurrency, "", 1, mainConfig.BackTestLength * 2));

            task.Wait();

        }

        public static BaseCustomBot PerformBackTest(string market)
        {

            HaasonlineClient haasonlineClient = new HaasonlineClient(ActionManager.mainConfig.IPAddress, ActionManager.mainConfig.Port, ActionManager.mainConfig.Secret);

            var task = Task.Run(async () => await haasonlineClient.CustomBotApi.BacktestBot<BaseCustomBot>(BaseBotTemplateGuid, ActionManager.mainConfig.BackTestLength, ActionManager.mainConfig.AccountGUID, market, ActionManager.mainConfig.PrimaryCurrency, ""));
            
            task.Wait();

            return task.Result.Result;

        }

        public static List<BaseCustomBot> GetAllCustomBots()
        {
            if (ActionManager.CheckHaasConnection())
            {
                HaasonlineClient haasonlineClient = new HaasonlineClient(ActionManager.mainConfig.IPAddress, ActionManager.mainConfig.Port, ActionManager.mainConfig.Secret);

                var getAllCustomBotsTask = Task.Run(async () => await haasonlineClient.CustomBotApi.GetAllBots());

                getAllCustomBotsTask.Wait();

                return getAllCustomBotsTask.Result.Result;
            }
            else
            {
                return null;
            }
        }


        public static BaseCustomBot GetCustomBotByName(string botName)
        {

            if (ActionManager.CheckHaasConnection())
            {
                // Find active bot markets
                foreach (var bot in ActionManager.GetAllCustomBots())
                {
                    if (bot.Name.Equals(botName))
                    {
                        return bot;
                    }
                }
            }

            return null;
        }


        public static void DeleteBot(string botGuid)
        {
            if (ActionManager.CheckHaasConnection())
            {
                HaasonlineClient haasonlineClient = new HaasonlineClient(ActionManager.mainConfig.IPAddress, ActionManager.mainConfig.Port, ActionManager.mainConfig.Secret);
                var deleteTask = Task.Run(async () => await haasonlineClient.CustomBotApi.RemoveBot(botGuid));

                deleteTask.Wait();
            }
        }

        public static void CreatePersistentBot(string market)
        {
            string[] accountGuidSplit = ActionManager.mainConfig.AccountGUID.Split('-');

            string botName = "PP-" + accountGuidSplit[0] + "-" + market + ":" + ActionManager.mainConfig.PrimaryCurrency;

            HaasonlineClient haasonlineClient = new HaasonlineClient(ActionManager.mainConfig.IPAddress, ActionManager.mainConfig.Port, ActionManager.mainConfig.Secret);

            var task = Task.Run(async () => await haasonlineClient.CustomBotApi.NewBot(Haasonline.Public.LocalApi.CSharp.Enums.EnumCustomBotType.PingPongBot, 
                botName, ActionManager.mainConfig.AccountGUID, market, ActionManager.mainConfig.PrimaryCurrency, ""));

            task.Wait();

            Thread.Sleep(1000);
        }

        public static bool PerformStartup()
        {
            if (LoadConfig())
            {
                return true;
            }
            else
            {
                ActionManager.mainConfig = new HaasConfig();
                return false;
            }
        }

    }
}
