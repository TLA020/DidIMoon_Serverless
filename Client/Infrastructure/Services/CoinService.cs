using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Shared.Models;

namespace Client.Infrastructure.Services
{
    public class CoinService : ICoinService
    {
        private readonly HttpClient _client;

        private List<CryptoCurrency> _coinList { get; set; }
        
        public CoinService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task SetCoinList()
        {
            _coinList  = await _client.GetFromJsonAsync<List<CryptoCurrency>>("api/FetchTokenList");
        }

        public async Task<List<CryptoCurrency>> CoinList()
        {
            if (_coinList == null) 
            {
                await SetCoinList();
            }

            return _coinList;
        }

        // todo move serverside
        public async Task<CryptoCurrency> GetByCode(string code)
        {
            return (await CoinList())
                .FirstOrDefault(c => string.Equals(c.Code, code, StringComparison.OrdinalIgnoreCase));
        }

        // todo move serverside
        public async Task<CryptoCurrency> GetBySymbol(string symbol)
        {
            return (await CoinList())
                .FirstOrDefault(c => string.Equals(c.Symbol, symbol, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<decimal> GetAmount(CryptoCurrency coin, decimal amount, DateTime date)
        {
            if (date == default)
            {
                date = DateTime.UtcNow;
            }

            var info = await FetchCoin(coin.Id, date);
            return amount / info.Price;
        }
        
        public async Task<decimal> GetValue(CryptoCurrency coin, decimal amount, DateTime date)
        {
            if (date == default)
            {
                date = DateTime.UtcNow;
            }

            var info = await FetchCoin(coin.Id, date);
            return info.Price * amount;
        }

        private async Task<CurrencyInfo> FetchCoin(long id, DateTime date, string baseCurrency = "USD")
        {
            return await _client.GetFromJsonAsync<CurrencyInfo>($"/api/FetchToken?coinId={id}&date={date.ToString("yyyy-MM-dd")}&currency={baseCurrency}");
        }
    }
}