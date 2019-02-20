using BLL.DomainClasses;
using DAL.UnitOfWork;

namespace BusinessServices
{
    public class TraderServices
    {
        private UnitOfWork uow = new UnitOfWork();

        //public void AddSuperTrader(Trader trader)
        //{
            
        //}


        public int update(Trader trader)
        {
            if (uow.TraderRepo.CheckEmailExists(trader.Email, trader.ID) == 1)
                return 1;

            if (uow.TraderRepo.CheckOrderExists(trader.SortOrder, trader.ID) == 1)
                return 2;
            //Trader t = uow.TraderRepo.FindTraderById(trader.ID);
 
            uow.TraderRepo.Update(trader);
            uow.SaveChanges();
            return 0;
        }
    }
}