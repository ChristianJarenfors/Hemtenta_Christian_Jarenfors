using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017.Account
{
    public class Account : IAccount
    {
        public double Amount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Deposit(double amount)
        {
            throw new NotImplementedException();
        }

        public void TransferFunds(IAccount destination, double amount)
        {
            throw new NotImplementedException();
        }

        public void Withdraw(double amount)
        {
            throw new NotImplementedException();
        }
    }
}
