# Haas-PPScreener
A PingPong Bot Screen For Haasonline

Code to bruteforce crypto scalping pairs via backtesting

Make sure you have your local api server setup for haas [Link To Wiki](https://wiki.haasonline.com/Local_API_Server)

After that just follow the instructions in the app.. More documentation to come.

SPECIAL: This bot has a different setting requirment along side the account guid you want to use you will need to specify the 
"ExchangeSelection" setting. The below values are avalible. You need the exchange to match the account you are using. 

```
Bitfinex = 1
BTCe = 4
CexIO = 5
OKCoinCOM = 8
OKCoinFutures = 9
Bitstamp = 10
Poloniex = 11
Coinbase = 12
Bittrex = 13
NovaExchange = 14
Kraken = 15
BitMEX = 17
ScriptedDriver = 18
CCex = 19
Gemini = 20
Binance = 21
HitBTC = 22
OKEX = 23
Huobi = 26
KuCoin = 27
```

Special: You can also tell the bot to not load markets at first run by modifying the "CollectDataFirst" to either be "true" or "false". Default for built is true.

Note: Make sure to delete your bot after each run or you will get werid results.

Note: IF YOU DO NOT ALREADY HAVE THE MARKET DATA DOWNLOADED THE BOT MIGHT MISS SOME BACKTEST. FOR NEW MARKETS OPEN THE MARKET VIEW AND VIEW AS FAR BACK HAS YOU PLAN ON BACKTESTING.
