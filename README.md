# Haas-PPScreener
```
___  ___  ____ ____ ____ ____ ____ _  _ ____ ____
|__] |__] [__  |    |__/ |___ |___ |\ | |___ |__/
|    |    ___] |___ |  \ |___ |___ | \| |___ |  \
```

A PingPong Bot Screen For Haasonline.

### Use Case
This tool allows a individual to rapdily test the ping pong bot startegy across an entire
market place while only keeping the profitable bots.

### Requirments
* Haasonline Trading Software
* The Haas Local API Setup [Link To Wiki](https://wiki.haasonline.com/Local_API_Server)
* Basic common sense

### Setup
First you will want to open the PPScreen.json file and update the following settings to match
what you set in the Haas Interface.

```
"IPAddress":"127.0.0.1"
"Port":8096
"Secret":"SomeSecretHere"
```

Second once you have ran the software and the credential check passes the software will present
you with a list of accounts and the GUID associated. You will want to pick the simulated account
connected to the exchange you want to test. Modify the Following parameter in the PPScreener.json

```
"AccountGUID":"ReplaceMeWithGuid"
```

Finally you will need to select the correct exchange for the bot to test against. You want this to
be the same exchange associated with the account guid you have selected. Modify the following settings

```
"ExchangeSelection":1
```

With one of the following values to match your exchange.

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

### Additional Configuration

The Bot also supports additional configuration options that are listed below.

```
# The delay between each BackTest in seconds. By default this is set to 1 second.
# You can increase this if you have a slower machine and need a longer delay between test.
# Note: If you decrease these please ensure you have a fast enough machine
"DelayBTInMiliseconds":1000

# The Keep Threshold is what percentage does the backtest ROI have to be higher than
# In order for the bot to not be automatically deleted. Default is 2%
"KeepThreshold":2.0

# The PrimarySecondaryCurrency (Yeah the name sucks) this is the base coin for the market
# For example this can be BTC,ETH,LTC,BNB,ETC
"PrimarySecondaryCurrency":"BTC"

# This is the fee to be used when calculating the ROI and setting up the bot
# By default its set to 0.1% (Binance)
"Fee":0.1

# This is the timeframe to backtest in minutes.
# By default it is set to 1 day.
"MinutesToBackTest":1440

# In order to backtest a market haas must first download the market information
# To stream line our backtesting and avoid issues we can set this to true to
# Force haas to download market data before performing our testing.
# Default is set to false.
"CollectDataFirst":false

# For convience and history we have the option to log everything to a file
# Default is True
"WriteToFile":false
```
