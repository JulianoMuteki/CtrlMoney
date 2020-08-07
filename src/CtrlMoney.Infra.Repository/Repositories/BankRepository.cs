using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Repository;
using CtrlMoney.Infra.Context;
using CtrlMoney.Infra.Repository.Common;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CtrlMoney.Infra.Repository.Repositories
{
    public class BankRepository : GenericRepository<Bank>, IBankRepository
    {
        public BankRepository(CtrlMoneyContext context)
            : base(context)
        {

        }

        public ICollection<Bank> GetAllBanks()
        {
            var dir = Directory.GetParent(Directory.GetCurrentDirectory());
            string path = Path.Combine(dir.FullName, @"CtrlMoney.Resources\febraban_banks.json");
            var jsonString = File.ReadAllText(path);
            var banks = JsonSerializer.Deserialize<IList<Bank>>(jsonString);
            return banks;
        }
    }
}
