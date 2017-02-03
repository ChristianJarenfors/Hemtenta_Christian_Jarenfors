using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017.Webshop
{
    class Basket : IBasket
    {
        //Basket har ingen lista på produkter. Jag lägger till den så
        //Att Jag kan lägga till och ta bort produkter men även så att
        //jag kan räkna ut TotalCost från något.
        List<Product> ProductBasket = new List<Product>();
        
        public decimal TotalCost
        {
            get
            {
                decimal sum=0;
                foreach (var item in ProductBasket)
                {
                    sum += item.Price;
                }
                return sum;
            }
        }
        //Denna har jag lagt till för att kunna testa Add och Remove
        //Men den kan vara bra att ha om man vill se vad man vill ha 
        //i sin kundkorg när man handlar.

        //Sen inser jag att detta inte finns i interfacet och 
        //därför inte kommer att synas i implemntationen så denna ligger som
        //en relik av en tanke.

        //public List<Product> GetProductList()
        //{
        //    return ProductBasket;
        //}
        public void AddProduct(Product p, int amount)
        {
            if (p==null)
            {
                throw new Exception("Product is null.");    
            }
            if (p.Price < 0)
            {
                throw new Exception("Negative price not allowed");
            }
            if (amount<0)
            {
                throw new Exception("No negative Amount allowed");
            }
            for (int i = 0; i < amount; i++)
            {
                ProductBasket.Add(p);
            }
        }

        public void RemoveProduct(Product p, int amount)
        {
            if (p == null)
            {
                throw new Exception("Product is null.");
            }
            if (p.Price < 0)
            {
                throw new Exception("Negative price not allowed");
            }
            if (amount < 0)
            {
                throw new Exception("No negative Amount allowed");
            }
            for (int i = 0; i < amount; i++)
            {
                ProductBasket.Remove(p);
            }
        }
    }
}
