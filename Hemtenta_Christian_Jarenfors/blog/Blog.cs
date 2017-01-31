using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017.Blog
{
    public class Blog : IBlog
    {
        IAuthenticator auth;
        public void AuthenticatorSetter(IAuthenticator a)
        {
            auth = a;
        }
        private bool userLogged =false;
        public bool UserIsLoggedIn
        {
            get
            {
                return userLogged;
            }
            set
            {
                userLogged = value;
            }
        }

        public void LoginUser(User u)
        {
            if (u==null)
            {
                throw new ArgumentNullException("Null User Object.");
            }
            if (string.IsNullOrEmpty(u.Name)||string.IsNullOrEmpty(u.Password))
            {
                UserIsLoggedIn = false;
            }
            else
            {
                User fromDB = auth.GetUserFromDatabase(u.Name);
                if (fromDB != null)
                {
                    if (fromDB.Password == u.Password)
                    {
                        UserIsLoggedIn = true;
                    }
                    else
                        UserIsLoggedIn = false;
                }
                else
                    UserIsLoggedIn = false;
            }
        }

        public void LogoutUser(User u)
        {
            if (u==null)
            {
                throw new ArgumentNullException("User är Null.");
            }
            UserIsLoggedIn = false;
        }

        public bool PublishPage(Page p)
        {
            if (!UserIsLoggedIn)
            {
                return false;
            }
            if (p==null)
            {
                throw new Exception("Page är null.");
            }
            if (string.IsNullOrEmpty(p.Title) ||string.IsNullOrEmpty(p.Content))
            {
                throw new Exception("Null or Empty in Title or Content");
            }
            return true;
        }

        public int SendEmail(string address, string caption, string body)
        {
            if (!UserIsLoggedIn)
            {
                return 0;
            }
            if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(caption)||string.IsNullOrEmpty(body))
            {
                return 0;
            }
            int count = 0;
            for (int i = 0; i < address.Length; i++)
            {
                if (address[i]=='@')
                {
                    count++;
                }
            }
            if (count!=1)
            {
                return 0; 
            }
            return 1;
        }
    }
}
