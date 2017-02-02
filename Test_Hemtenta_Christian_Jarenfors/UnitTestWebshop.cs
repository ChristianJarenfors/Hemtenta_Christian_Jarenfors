using System;
using HemtentaTdd2017;
using HemtentaTdd2017.Webshop;
using Moq;
using NUnit.Framework;
namespace Test_Hemtenta_Christian_Jarenfors
{
    [TestFixture]
    public class UnitTestWebshop
    {
        /*Jag svarar på dessa 3 frågor löpande i detta kommentars fält.
         * 
         * Vilka metoder och properties behöver testas?
         * Ska några exceptions kastas?
         * Vilka är domänerna för IWebshop och IBasket?
         *
         * WEBSHOP
         * void Checkout(IBilling billing);
         * -Denna behöver Testas så att man har råd att betala
         *
         * billing kan vara: Object och null så det behöver kollas om det är det
         * kastar Exception vid null.
         * Vidare kan IBilling objectet ha för liten Balance så det behöver kollas
         * kastar Exception vid för lite pengar.
         * Dessutom om TotalCost från basket är 0 så behöver bankerna inte göra någon
         * överföring så då kan man antingen göra så att inget händer eller
         * att ett exception kastas.   I Mitt fall kastar jag ett exception.
         * 
         * BASKET
         * decimal TotalCost { get; }
         * -Denna behöver testas för att se att den returnerar rätt summa
         * Behöver kolla när man försöker ta bort produkter om inte finns att
         * det fungerar ändå och att det inte blir några spöksummor eller buggar
         * som tex om man försöker ta bort mer produkter än vad som finns i listan
         * 
         * void AddProduct(Product p, int amount);
         * void RemoveProduct(Product p, int amount);
         * 
         * product kan vara null eller object så det behöver kollas
         * amount får inte vara negativt då skall det kastas exception
         * 
         * Behöver även kolla om det blir rätt antal produkter i listan efter
         * add och remove. För det gör jag en hjälpmetod GetProductList() så att
         * man kan kolla vad som finns i listan. Tycker en sån metod är bra
         * att ha i en webshop också så man vet vad man vill ta bort.
         * 
         * * * OBS. Tar bort GetProductList() eftersom den inte finns i interfacet
         *
         * IBilling mock. Det är lite oklart men vad jag tycker så behöver 
         * Pay-metoden mockas något nämnvärt eftersom den inte returnerar nått.
         * Däremot behöver Balansen på kontot vara mer än det man skall betala
         * så den behöver mockas.
         */


        MyWebshop Shoppy =new MyWebshop();
        static Product Äpple1 = new Product() { Name = "Äpple", Price = 1 };
        static Product Päron10 = new Product() { Name = "Päron", Price = 10 };
        static Product Apelsin100 = new Product() { Name = "Apelsin", Price = 100 };
        Mock<IBilling> Account2900;


        [SetUp]
        public void init()
        {
            Shoppy = new MyWebshop();
            Account2900 = new Mock<IBilling>();
            Account2900.Setup((x) => x.Balance).Returns(2900);
        }


        #region Checkout
        [Test]
        public void Checkout_Success()
        {
            Shoppy.Basket.AddProduct(Apelsin100, 29);
            Assert.DoesNotThrow(() => Shoppy.Checkout(Account2900.Object));
        }
        [Test]
        public void Checkout_Fail_IBilling_Null()
        {
            Shoppy.Basket.AddProduct(Äpple1, 3);
            Assert.Throws<Exception>(() => Shoppy.Checkout(null));
        }
        [Test]
        public void Checkout_Fail_IBilling_To_Low_Balance()
        {
            Shoppy.Basket.AddProduct(Apelsin100, 30);
            Assert.Throws<Exception>(() => Shoppy.Checkout(Account2900.Object));
        }
        [Test]
        public void Checkout_Fail_TotalCost_In_Basket_0_Zero()
        {
            Assert.Throws<Exception>(() => Shoppy.Checkout(Account2900.Object));
        }
        #endregion


        #region TotalCost and ProductJuggling
        [Test]
        public void Totalcost_Successfully_Counted()
        {
            Shoppy.Basket.AddProduct(Äpple1, 3);
            Shoppy.Basket.AddProduct(Päron10, 5);
            Shoppy.Basket.AddProduct(Apelsin100, 10);
            Assert.AreEqual(1053, Shoppy.Basket.TotalCost);
            Shoppy.Basket.AddProduct(Päron10, 10);
            Assert.AreEqual(1153, Shoppy.Basket.TotalCost);
            //Här kollar jag att den inte ballar ur om man försöker ta bort 
            //för många. (Det finns bara 10 apelsiner i listan)
            Shoppy.Basket.RemoveProduct(Apelsin100, 11);
            Assert.AreEqual(153, Shoppy.Basket.TotalCost);
            Shoppy.Basket.RemoveProduct(Päron10, 15);
            Assert.AreEqual(3, Shoppy.Basket.TotalCost);
            Shoppy.Basket.RemoveProduct(Äpple1, 4);
            Assert.AreEqual(0, Shoppy.Basket.TotalCost);
        }
        [Test]
        public void Add_Remove_Fail_Null_Product()
        {
            Assert.Throws<Exception>(()=> Shoppy.Basket.AddProduct(null, 1));
            Assert.Throws<Exception>(() => Shoppy.Basket.RemoveProduct(null, 1));
        }
        [Test]
        public void Add_Remove_Fail_Negative_Products()
        {
            Assert.Throws<Exception>(() => Shoppy.Basket.AddProduct(Äpple1, -1));
            Assert.Throws<Exception>(() => Shoppy.Basket.RemoveProduct(Päron10, -2));
        }
        #endregion 

    }
}
