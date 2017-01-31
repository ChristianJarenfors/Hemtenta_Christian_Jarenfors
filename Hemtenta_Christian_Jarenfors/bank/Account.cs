using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017.Account
{
    public class Account : IAccount
    {
        private double hiddenAmount;
        public double Amount
        {
            get
            {
                return hiddenAmount;
            }
            set
            {
                hiddenAmount = value;
            }
        }
        public int MyProperty { get; set; }
        public void Deposit(double amount)
        {
            if (double.IsNaN(amount)||double.IsInfinity(amount)|| amount <=0)
            {
                throw new IllegalAmountException();
            }
            if (double.IsInfinity(Amount+amount))
            {
                throw new OperationNotPermittedException();
            }

            Amount = Amount +amount;
        }

        public void TransferFunds(IAccount destination, double amount)
        {
            if (destination==null)
            {
                throw new OperationNotPermittedException();
            }
            if (double.IsNaN(amount) || double.IsInfinity(amount) || amount <= 0)
            {
                throw new IllegalAmountException();
            }
            if((this.Amount - amount) < 0)
            {
                throw new InsufficientFundsException();
            }
            if (double.IsInfinity(destination.Amount + amount))
            {
                throw new OperationNotPermittedException();
            }
            this.Withdraw(amount);
            destination.Deposit(amount);
        }

        public void Withdraw(double amount)
        {
            if (double.IsNaN(amount) || double.IsInfinity(amount) || amount <= 0)
            {
                throw new IllegalAmountException();
            }
            if ((Amount - amount)<0)
            {
                throw new InsufficientFundsException();
            }
            Amount = Amount - amount;
        }
    }
}
