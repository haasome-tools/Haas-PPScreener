### PPScreener - Haasonline PingPong Bot Screener
##### Documentation Author: Commissarr

The PPScreener is designed to test every possible market on an exchange by testing each coin traded against the specified primarycurrency (BasePair). For Example if we point it at Binance and use BTC. It will test every pair ada/btc,bnb/btc, etc so on so forth.

#### Preperation In Haas Server

In order for the software to work you need to setup the API portion of your haas software. Here is some helpful information from the wiki: [Wiki Link](https://wiki.haasonline.com/Local_API_Server) Remember that all three fields need to be populated even if it looks like the greyed out information there was typed. If you didn't type it yourself it won't work.

Example:


![LocalApi](https://i.imgur.com/K61gNx3.png)

First download the zipfile of the newest release to your haas server/pc. Extract it to a folder and run the exe file now see the instructions below in this document

```
Note: before exiting the program type the following to save your settings:
save-config
```

#### General Configuration

Now to Setup the local api information

```
# Set your local api ipaddress
set-config ipaddress type-your-local-api-address-here

# Set your local api port
set-config port type-your-local-api-port-here

# Set your local api secret
set-config secret your-local-api-secret-here
```

Example Output:
![ExampleSetupOutput](https://i.imgur.com/tsqvE7c.png)

Now Test your settings to ensure connection is succesfull

```
# Test Connection Settings
test-creds
```

Example Output:

![ExampleTestCredsOutput](https://i.imgur.com/WUvEDCu.png)

Next you need to pick a wallet/account in haas which PPScreener will use, a simulated account/wallet is recommended (Note you need to set these accounts/wallets up in Haas yourself, this program won’t do it for you)

```
# See a list of avalible accounts
show-accounts
```

Example Output:
![ExampleShowAccounts](https://i.imgur.com/uSEYdFC.png)

Then we select an account

```
# Selects an account
set-account type-the-account-index-number
```

Example Output:
![ExampleSelectAccount](https://i.imgur.com/tR2OWWg.png)

Here you set the ROI (return on investment) target the backtested bots must reach before the automatically created and tested bots this program creates will keep the bots (that reached the target after testing) in your botlist under Ping Pong bots. Note you have to type a % of your choosing, this example is 3% but you can type decimals as well like 2.5

```
# Set the keeptreshold (What Roi Bots To Keep) Ex. For 3% we set 3.0
set-config keepthreshold 3.0

# Set to persist bots (Keep bots that are above keepthreshold in bot list)
set-config persistbots true
```

Now to choose on which market in the chosen exchange to test all the coins against: USD, BTC, ETH, USDT, etc. In this example we choose BTC markets

```
# Set the base currency to use (BTC,ETH,ETC)
set-config primarycurrency type-chosen-market
```

Finally we start the screener

```
# Start the testing process
start
```

Example Output:
![ExampleStartOutput](https://i.imgur.com/xCwOgBw.png)

### Advanced Commands

######Backtest Delay
The delay in milliseconds between each conducted backtest of bots created by this program
note that 1second = 1000 milliseconds
```
set-config backtestdelay type-amount-in-milliseconds
example: set-config backtestdelay 500
```

######Set the exchange fee
To set the fee of the exchange/wallet you are on. For example 0.25% for bittrex (at the time of writing)
```
set-config fee type-number-and-decimal
Example: set-config fee 0.25 is 0.25%
```

######Save PPScreener backtest history
This will depending on the true or false setting save the backtests conducted by PPScreener to a CSV file
```
set-config writeresultstofile true/false
Example: set-config writeresultstofile true would result in the file being saved
```


######Set retry on receiving market data
How many times to retry receiving the market
```
set-config retrycount type-amount-number 
Example: set-config retrycount 10 will retry 10 times
```