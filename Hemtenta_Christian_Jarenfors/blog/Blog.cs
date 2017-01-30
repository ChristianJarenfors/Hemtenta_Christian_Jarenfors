using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017.Blog
{
    public class Blog : IBlog
    {
        public bool UserIsLoggedIn
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void LoginUser(User u)
        {
            throw new NotImplementedException();
        }

        public void LogoutUser(User u)
        {
            throw new NotImplementedException();
        }

        public bool PublishPage(Page p)
        {
            throw new NotImplementedException();
        }

        public int SendEmail(string address, string caption, string body)
        {
            throw new NotImplementedException();
        }
    }
}
