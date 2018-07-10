using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using Haasonline.Public.LocalApi.CSharp.DataObjects.CustomBots;
using NCmd;

namespace PPScreener
{
    class InteractiveShell : Cmd
    {
        private const string HistFileName = "ppscreener-cmd";

        [CmdCommand(Command = "exit", Description = StaticStrings.EXIT_HELP_TEXT)]
        public void ExitCommand(string arg)
        {

            ExitLoop();
        }

        [CmdCommand(Command = "clear", Description = StaticStrings.CLEAR_HELP_TEXT)]
        public void ClearCommand(string arg)
        {
            Console.Clear();
        }

        [CmdCommand(Command = "version", Description = StaticStrings.VERSION_HELP_TEXT)]
        public void ShowVersion(string arg)
        {
            WriteVersionStatement(new AutoProgramMetaData(GetType().Assembly), Console.Out);
        }

        [CmdCommand(Command = "show-config", Description = StaticStrings.SHOW_CONFIG_HELP_TEXT)]
        public void ShowConfigCommand(string arg)
        {
            ActionManager.ShowConfig();
        }

        [CmdCommand(Command ="set-config", Description = StaticStrings.SET_CONFIG_HELP_TEXT)]
        public void SetConfigCommand(string arg)
        {
            string[] arguments = Utils.SplitArgumentsSaftley(arg);

            if (arguments.Length >= 2)
            {
                switch (arguments[0])
                {
                    case "ipaddress":
                        ActionManager.SetConfigIpAddress(arguments[1]);
                        Console.WriteLine("[*] Haas Ip Address Set To {0}", arguments[1]);
                        break;

                    case "port":
                        int port_dead = 0;
                        if (Int32.TryParse(arguments[1], out port_dead)) { 
                            ActionManager.SetConfigPort(Convert.ToInt32(arguments[1]));
                            Console.WriteLine("[*] Haas Port Set To {0}", arguments[1]);
                        }
                        else
                        {
                            Console.WriteLine("[!] Argument Is Not A Number");
                        }
                        break;

                    case "secret":
                        ActionManager.SetConfigSecret(arguments[1]);
                        Console.WriteLine("[*] Haas Secret Set To {0}", arguments[1]);
                        break;

                    case "accountguid":
                        ActionManager.SetConfigSecret(arguments[1]);
                        Console.WriteLine("[*] Haas Account GUID Set To {0}", arguments[1]);
                        break;

                    case "keepthreshold":
                        decimal keepthreshold_dead = 0;
                        if (Decimal.TryParse(arguments[1], out keepthreshold_dead))
                        {
                            ActionManager.SetConfigKeepThreshold(Convert.ToDecimal(arguments[1]));
                            Console.WriteLine("[*] Keep Threshold Set To {0}", arguments[1]);
                        }
                        else
                        {
                            Console.WriteLine("[!] Argument Is Not A Decimal");
                        }
                        break;

                    case "backtestdelay":
                        int backtestdelay_dead = 0;
                        if (Int32.TryParse(arguments[1], out backtestdelay_dead))
                        {
                            ActionManager.SetConfigBacktestDelay(Convert.ToInt32(arguments[1]));
                            Console.WriteLine("[*] Backtest Delay Set To {0}", arguments[1]);
                        }
                        else
                        {
                            Console.WriteLine("[!] Argument Is Not A Number");
                        }
                        break;

                    case "fee":
                        decimal fee_dead = 0;
                        if (Decimal.TryParse(arguments[1], out fee_dead))
                        {
                            ActionManager.SetConfigKeepThreshold(Convert.ToDecimal(arguments[1]));
                            Console.WriteLine("[*] Fee Set To {0}", arguments[1]);
                        }
                        else
                        {
                            Console.WriteLine("[!] Argument Is Not A Decimal");
                        }
                        break;

                    case "backtestlength":
                        int backtestlength_dead = 0;
                        if (Int32.TryParse(arguments[1], out backtestlength_dead))
                        {
                            ActionManager.SetConfigBackTestLength(Convert.ToInt32(arguments[1]));
                            Console.WriteLine("[*] Backtest Length Set To {0}", arguments[1]);
                        }
                        else
                        {
                            Console.WriteLine("[!] Argument Is Not A Number");
                        }
                        break;

                    case "primarycurrency":
                        ActionManager.SetConfigPrimaryCurrency(arguments[1]);
                        Console.WriteLine("[*] Primary Currency Set To {0}", arguments[1]);
                        break;

                    case "writeresultstofile":
                        ActionManager.SetConfigWriteResultsToFile(Convert.ToBoolean(arguments[1]));
                        Console.WriteLine("[*] Write Results To File Set To {0}", Convert.ToBoolean(arguments[1]));
                        break;

                    case "persistbots":
                        ActionManager.SetConfigPersistBots(Convert.ToBoolean(arguments[1]));
                        Console.WriteLine("[*] Persist Bots Set To {0}", Convert.ToBoolean(arguments[1]));
                        break;

                    case "retrycount":
                        int retrycount_dead = 0;
                        if (Int32.TryParse(arguments[1], out retrycount_dead))
                        {
                            ActionManager.SetConfigRetryCount(Convert.ToInt32(arguments[1]));
                            Console.WriteLine("[*] Retry Count Set To {0}", arguments[1]);
                        }
                        else
                        {
                            Console.WriteLine("[!] Argument Is Not A Number");
                        }
                        break;

                    default:
                        Console.WriteLine("Argument not valid");
                        break;

                }

            }
            else
            {
                Console.WriteLine("[!] Not Enough Arguments Specified");
                Console.WriteLine("Ex. set-config <configOption> <value>");
            }

        }

        [CmdCommand(Command = "save-config", Description = StaticStrings.SAVE_CONFIG_HELP_TEXT)]
        public void SaveConfigCommand(string arg)
        {
            string[] arguments = Utils.SplitArgumentsSaftley(arg);

            if (arguments.Length == 1)
            {
                if(ActionManager.SaveConfig(arguments[0]))
                {
                    Console.WriteLine("[*] Saved Config With Filename {0}", arg[0]);
                }
                else
                {
                    Console.WriteLine("[!] Could Not Save Config {0}", arg[0]);
                }
            }
            else
            {
                if(ActionManager.SaveConfig())
                {
                    Console.WriteLine("[*] Saved Default Config File {0}", ActionManager.DefaultConfigName);
                }
                else
                {
                    Console.WriteLine("[!] Could Not Save Default Config File {0}", arg[0]);
                }
            }
        }

        [CmdCommand(Command = "load-config", Description = StaticStrings.LOAD_CONFIG_HELP_TEXT)]
        public void LoadConfigCommand(string arg)
        {
            string[] arguments = Utils.SplitArgumentsSaftley(arg);

            if (arguments.Length == 1)
            {
                if(ActionManager.LoadConfig(arguments[0]))
                {
                    Console.WriteLine("[*] Loaded Config With Filename {0}", arg[0]);
                } else
                {
                    Console.WriteLine("[!] Could Not Load Config {0}", arg[0]);
                }
            }
            else
            {
                if (ActionManager.LoadConfig())
                {
                    Console.WriteLine("[*] Loaded Default Config File {0}", ActionManager.DefaultConfigName);
                }
                else
                {
                    Console.WriteLine("[!] Could Not Load Default Config File {0}", arg[0]);
                }
            }
        }

        [CmdCommand(Command = "test-creds", Description = StaticStrings.TEST_CREDS_HELP_TEXT)]
        public void TestCredsCommand(string arg)
        {
            Console.WriteLine("[*] Verifying API Connection and Credentials");

            if (ActionManager.CheckHaasConnection())
            {
                Console.WriteLine("[*] Connection Succesfull");
            }
            else
            {
                Console.WriteLine("[!] Connection Failed Check ip:port/credentials");
            }

        }

        [CmdCommand(Command = "show-accounts", Description = StaticStrings.SHOW_ACCOUNTS_HELP_TEXT)]
        public void ShowAccountGuidsCommand(string arg)
        {
            int count = 1;

            Console.WriteLine("\n---- Current Active Accounts ----");

            foreach ( var account in ActionManager.GetAccountGUIDS())
            {
                Console.WriteLine("#{0} - {1} : {2}", count, account.Item1, account.Item2);
                count++;
            }

            Console.WriteLine();
            
        }

        [CmdCommand(Command = "set-account", Description = StaticStrings.SET_ACCOUNT_HELP_TEXT)]
        public void SetAccountCommand(string arg)
        {
            string[] arguments = Utils.SplitArgumentsSaftley(arg);

            if (arguments.Length == 1)
            {
                int dead = 0;
                if (Int32.TryParse(arguments[0], out dead))
                {
                    int index = Convert.ToInt32(arguments[0]);
                    var accounts = ActionManager.GetAccountGUIDS();

                    var accountPair = new Tuple<string, string>("","");

                    if (index > accounts.Count || index == 0)
                    {
                        Console.WriteLine("[!] Invalid Account Selection");
                    } 
                    else
                    { 
                        accountPair = ActionManager.GetAccountGUIDS()[Convert.ToInt32(arguments[0])-1];
                        ActionManager.SetConfigAccountGuid(accountPair.Item2);
                        Console.WriteLine("[*] Haas Account Set To {0} : {1}", accountPair.Item1, accountPair.Item2);
                    }

                }
                else
                {
                    Console.WriteLine("[!] Argument Is Not A Number");
                }
            }
            else
            {
                Console.WriteLine("Not Enough Arguments");
                Console.WriteLine("Ex. set-account <account-num>");
            }
        }

        [CmdCommand(Command = "show-markets", Description = StaticStrings.SHOW_MARKETS_HELP_TEXT)]
        public void ShowMarketsCommand(string arg)
        {
            var markets = ActionManager.GetMarkets();

            if (markets.Count == 0)
            {
                Console.WriteLine("[!] Could Not Obtain Market Information");
            }
            else
            {
                Console.WriteLine("\n---- Current Markets ----");
                foreach (string market in markets)
                {
                    Console.WriteLine("{0}/{1}", market, ActionManager.mainConfig.PrimaryCurrency);
                }
            }
            
        }

        [CmdCommand(Command = "start", Description = StaticStrings.START_SCREENER_HELP_TEXT)]
        public void StartScreenerCommand(string arg)
        {
            Console.WriteLine("[*] Starting Screening Process");

            var markets = ActionManager.GetMarkets();

            List<BackTestResult> backTestResults = new List<BackTestResult>();

            if (ActionManager.CreateTemplateBot().Equals(""))
            {
                Console.WriteLine("[!] Could Not Create Template Bot");
            }
            else
            {
                Console.WriteLine("[*] Created Base Template Bot");

                if (markets.Count == 0)
                {
                    Console.WriteLine("[!] Could Not Obtain Market Information");
                }
                else
                {
                    int index = 0;
                    foreach (string market in markets)
                    {

                        BaseCustomBot bot = new BaseCustomBot();

                        int retryCount = 0;

                        do
                        {
                            ActionManager.GrabMarketData(market);
                            Thread.Sleep(ActionManager.mainConfig.BackTestDelayInMiliseconds);
                            bot = ActionManager.PerformBackTest(market);

                            if (!(index > markets.Count - 1))
                            {
                                if (index == markets.Count - 1)
                                {
                                    ActionManager.GrabMarketData(markets[index]);
                                }
                                else
                                {
                                    ActionManager.GrabMarketData(markets[index + 1]);
                                }
                            }

                            if(retryCount == ActionManager.mainConfig.RetryCount)
                            {
                                break;
                            }

                            retryCount++;


                        } while (bot.ROI == 0);

                        string response = "";

                        if (bot.ROI >= ActionManager.mainConfig.KeepThreshold)
                        {
                            if (ActionManager.mainConfig.PersistBots)
                            {
                                ActionManager.CreatePersistentBot(market);
                                response = "Persisted";

                            }
                            else
                            {
                                response = "Valid";
                            }

                        }
                        else
                        {
                            response = "Ignored";
                        }

                        BackTestResult btResult = new BackTestResult();
                        btResult.AboveThreshold = (bot.ROI >= ActionManager.mainConfig.KeepThreshold);
                        btResult.Date = DateTime.Now;
                        btResult.Pair = market + "/" + ActionManager.mainConfig.PrimaryCurrency;
                        btResult.ROI = bot.ROI;

                        backTestResults.Add(btResult);

                        Console.WriteLine("[*] Results For {0}/{1} - {2} = Above Threshold : {3}", market, ActionManager.mainConfig.PrimaryCurrency, bot.ROI, response);

                        index++;
                    }

                    if (ActionManager.mainConfig.WriteResultsToFile)
                    {
                        using (TextWriter writer = new StreamWriter(@"BackTestResults.csv"))
                        {
                            var csv = new CsvWriter(writer);
                            csv.WriteRecords(backTestResults);
                            writer.Flush();
                        }
                    }
                }

                ActionManager.DeleteTemplateBot();

            }
        }

        public InteractiveShell()
        {

            Utils.ShowBanner();

            Intro = "";
            CommandPrompt = "$> ";
            HistoryFileName = HistFileName;
        }

        public override void PostCmd(string line)
        {
            base.PostCmd(line);
            //Console.WriteLine();
        }

        public override string PreCmd(string line)
        {
            return base.PreCmd(line);
        }

        public override void EmptyLine()
        {
            Console.WriteLine("Please enter a command or type 'help' for assitance.");
        }

        public override void PreLoop()
        {
            if(ActionManager.PerformStartup())
            {
                Console.WriteLine("[*] Succesfully Loaded Default Config {0} ", ActionManager.DefaultConfigName);
            }
            else
            {
                Console.WriteLine("[!] Failed To Load Default Config {0} ", ActionManager.DefaultConfigName);
                Console.WriteLine("[!] Generated New Config Config {0} ", ActionManager.DefaultConfigName);
                ActionManager.SaveConfig();
            }

        }

        public override void PostLoop()
        {

        }
    }
}
