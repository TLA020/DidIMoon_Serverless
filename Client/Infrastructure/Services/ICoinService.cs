using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Models;

namespace Client.Infrastructure.Services
{
    public interface ICoinService
    {
        Task<List<CryptoCurrency>> CoinList();
        Task<CryptoCurrency> GetByCode(string code);
        Task<CryptoCurrency> GetBySymbol(string symbol);
        Task<decimal> GetAmount(CryptoCurrency coin, decimal amount, DateTime date);
        Task<decimal> GetValue(CryptoCurrency coin, decimal amount, DateTime date);
    }
}