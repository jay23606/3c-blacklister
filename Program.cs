using System;
using System.Linq;
using XCommas.Net;
using XCommas.Net.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace blacklister
{
    class Program
    {
        static string key = "xxx";
        static string secret = "xxx";
        static XCommasApi api;

        static void Main() { MainAsync().GetAwaiter().GetResult(); }
        static async Task MainAsync()
        {
            HashSet<string> pairs = new HashSet<string>();
            api = new XCommasApi(key, secret, default, UserMode.Real);
            var accts = await api.GetAccountsAsync();
            foreach (var acct in accts.Data)
            {
                var marketPairs = await api.GetMarketPairsAsync(acct.MarketCode);
                foreach (string pair in marketPairs.Data) if ((pair.EndsWith("3L") || pair.EndsWith("3S")) && !pairs.Contains(pair)) pairs.Add(pair);
            }
            var data = new BotPairsBlackListData { Pairs = pairs.ToArray() };
            var res = await api.SetBotPairsBlackListAsync(data);
            if (res.IsSuccess) Console.WriteLine($"Successfully blacklisted {String.Join(", ", data.Pairs)}");
            else Console.WriteLine(res.Error);
        }
    }
}
