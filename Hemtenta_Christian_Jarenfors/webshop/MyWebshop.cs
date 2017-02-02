using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017.Webshop
{
    public class MyWebshop : IWebshop
    {
        /*
         * Klassen behöver en basket där shoppingen kan sparas.
         * Sen behövs metoder för att lägga till och ta bort produkter men de
         * finns ju redan i Basket så jag tänker att man via Propertyn Basket
         * kommer åt dem. De behöver testas så att Summan blir rätt. 
         * Jag tänker att man anropar metoderna i MyWebshop i klineten och det är så jag kommer
         * testa den.
         */
        IBasket basket = new Basket();
        public IBasket Basket
        {
            get
            {
                return basket;
            }
        }
        public void Checkout(IBilling billing)
        {
            if (Basket.TotalCost==0)
            {
                throw new Exception("0 price is not handled");
            }
            if (billing==null)
            {
                throw new Exception("Null IBilling Object");
            }
            if (Basket.TotalCost>billing.Balance)
            {
                throw new Exception("Insufficient Funds");
            }
            billing.Pay(Basket.TotalCost);
        }
    }
}
