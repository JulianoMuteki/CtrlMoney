using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Repository;
using CtrlMoney.Infra.Context;
using CtrlMoney.Infra.Repository.Common;

namespace CtrlMoney.Infra.Repository.Repositories
{
   public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(CtrlMoneyContext context)
            : base(context)
        {

        }
    }
}
