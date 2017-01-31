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

        Account TestAccount;
        [SetUp]
        public void Init()
        {
            TestAccount = new Account();
        }
        #region DepositMethod_Test
        // Sätter in ett belopp på kontot
        [Test]
        [TestCase(double.MaxValue)]
        [TestCase(1.00)]
        public void Deposit_Success(double amount)
        {
            //Kastar inget exception
            TestAccount.Deposit(amount);
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
            Assert.Throws<IllegalAmountException>(
                () => TestAccount.Deposit(amount),
                "Det kastas inget Exception för "+amount+", vilket är fel.");
        }
        [Test]
        [TestCase(double.MaxValue, double.MaxValue)]
        [TestCase(double.MaxValue/2+double.MaxValue/4, double.MaxValue/2)]
        public void Deposit_To_Large_Amount(double amount1, double amount2)
        {
            TestAccount.Deposit(amount1);
            
            Assert.Throws<OperationNotPermittedException>(
                ()=> TestAccount.Deposit(amount2),
                "Det gick att sätta in " + amount1 + " och " + amount2 + ".");
        }
        #endregion

        #region WithdrawMethod_Test
        // Gör ett uttag från kontot
        [Test]
        [TestCase(double.MaxValue,1000)]
        [TestCase(1000, 1000)]
        public void Withdraw_Success(double amountDepo, double amountWith)
        {
            TestAccount.Deposit(amountDepo);
            TestAccount.Withdraw(amountWith);
            Assert.That(TestAccount.Amount== (amountDepo - amountWith),
                TestAccount.Amount+" är inte lika med"+(amountDepo-amountWith) );
        }
        [Test]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.MinValue)]
        [TestCase(double.NaN)]
        [TestCase(-1)]
        [TestCase(0)]
        public void Withdraw_IncorrectParameter(double amount)
        {
            Assert.Throws<IllegalAmountException>(
                () => TestAccount.Withdraw(amount),
                "Det kastas inget Exception för " + amount + ", vilket är fel.");
        }
        [Test]
        [TestCase(100, 1000)]
        [TestCase(10, 100)]
        [TestCase(0.0001, 0.001)]
        public void Withdraw_ToLargeAmount(double amountDepo,double amountWith)
        {
            TestAccount.Deposit(amountDepo);
            Assert.Throws<InsufficientFundsException>(
                () => TestAccount.Withdraw(amountWith),
                "Det gick att ta ut " + amountWith + "från konto med " + TestAccount.Amount + ".");
        }
        #endregion

        #region TransferFundsMethod_Test
        // Överför ett belopp från ett konto till ett annat
        //IAccount kan vara null och object.
        //Det kan vara dålig balans på konto man skall ta från.
        //Kontot kan bli fullt så det är också ett fel.
        //double har Nan +-Infinity max min och valid.
        //0 amount är inte ok. 
        [Test]
        public void TransferFunds_Success()
        {
            Account Destination = new Account() { Amount = 0 };
            TestAccount.Deposit(12345.6789);
            TestAccount.TransferFunds(Destination, 12345.6789);
            Assert.That(TestAccount.Amount == 0, "Kontot är inte tomt.");
            Assert.That(Destination.Amount == 12345.6789, "Inga pengar överförda.");
        }
        [Test]
        public void TransferFunds_NullAccount()
        {
            Assert.Throws<OperationNotPermittedException>(
                () => TestAccount.TransferFunds(null, 100),
                "Det blev inte exception vid null.");
        }
        [Test]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.MinValue)]
        [TestCase(double.NaN)]
        [TestCase(-1)]
        [TestCase(0)]
        public void TransferFunds_IncorrectParameter( double amount)
        {
            Assert.Throws<IllegalAmountException>(
                ()=>TestAccount.TransferFunds(new Account() {Amount=0 }, amount),
                "Det gick att överföra " + amount+".");
        }
        [Test]
        public void TransferFunds_InsufficientFunds()
        {
            TestAccount.Deposit(100);

            Assert.Throws<InsufficientFundsException>(
                () => TestAccount.TransferFunds(new Account() { Amount = 0 }, 1000),
                "Det fanns tillräckligt med pengar på kontot som skulle skicka pengar.");
        }
        [Test]
        public void TransferFunds_ExceedAccountMAX()
        {
            TestAccount.Deposit(double.MaxValue);

            Assert.Throws<OperationNotPermittedException>(
                () => TestAccount.TransferFunds(new Account()
                { Amount = double.MaxValue / 2 + double.MaxValue / 4 },
                double.MaxValue/2), 
                "Kontot blir fullt med så mycket pengar.");
        }
        #endregion

    }
}
