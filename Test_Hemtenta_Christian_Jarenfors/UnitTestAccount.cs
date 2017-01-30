using System;
using HemtentaTdd2017.Account;
using HemtentaTdd2017;
using Moq;
using NUnit.Framework;
namespace Test_Hemtenta_Christian_Jarenfors
{
    [TestFixture]
    public class UnitTestAccount
    {
        Account TestAccount = new Account();
        [SetUp]
        public void Init()
        {

        }
        #region DepositMethod_Test
        // Sätter in ett belopp på kontot
        [Test]
        public void Deposit(double amount)
        {

        }
        [Test]       
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.MinValue)]
        [TestCase(double.NaN)]
        [TestCase(-1)]
        [TestCase(0)]
        public void Deposit_IncorrectParameter(double amount)
        {
            Assert.Throws<IllegalAmountException>(() => TestAccount.Deposit(amount), "Det kastas inget Exception för "+amount+", vilket är fel.");
        }
        #endregion

        #region WithdrawMethod_Test
        // Gör ett uttag från kontot
        [Test]
        public void Withdraw(double amount)
        {

        }
        #endregion

        #region TransferFundsMethod_Test
        // Överför ett belopp från ett konto till ett annat
        [Test]
        public void TransferFunds(IAccount destination, double amount)
        {

        }
        #endregion

    }
}
