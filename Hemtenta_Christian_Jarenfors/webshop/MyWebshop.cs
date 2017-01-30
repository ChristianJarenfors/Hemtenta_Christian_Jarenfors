using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017.Webshop
{
    public class MyWebshop : IWebshop
    {
        public IBasket Basket
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Checkout(IBilling billing)
        {
            throw new NotImplementedException();
        }
    }
}
