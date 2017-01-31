using System;
using HemtentaTdd2017;
using HemtentaTdd2017.Blog;
using Moq;
using NUnit.Framework;
namespace Test_Hemtenta_Christian_Jarenfors
{
    [TestFixture]
    public class UnitTestBlogg
    {
        Blog Bloggy;
        User validUser;
         const string ValidPageTitle = "Harry Potter",
            ValidPageContent = "En spännande Bok",
            ValidAdress="adress@gmail.com",
            ValidCaption="Hej!",
            ValidBody="Tjena! Läget?";
        [SetUp]
        public void init()
        {
            var authMock = new Mock<IAuthenticator>();
            authMock.Setup((x) => x.GetUserFromDatabase("Christian")).Returns(new User("Christian"));
            validUser = new User("Christian");
            Bloggy = new Blog();
            Bloggy.AuthenticatorSetter(authMock.Object);
        }
        // Försöker logga in en användare. Man kan
        // se om inloggningen lyckades på property
        // UserIsLoggedIn.
        // Kastar ett exception om User är null.

        #region LoginUser
        [Test]
        public void LoginUser_Success()
        {
            Bloggy.LoginUser(validUser);
            Assert.True(Bloggy.UserIsLoggedIn, "Det gick inte att logga in.");
        }
        [Test]
        [TestCase(null, "Guest")]
        [TestCase("", "Guest")]
        [TestCase("Christian", null)]
        [TestCase("Christian", "")]
        //Funderar på om det skall kastas exception vid null Username eller 
        //Password. Men jag håller mig  till Kravspesen: Om Objectet är null:
        //DÅ Kastas Exception
        public void LoginUser_Fail_NullOrEmptyString(string UName, string PWord)
        {
            User BadUser = new User(UName);
            BadUser.Password = PWord;
            Bloggy.LoginUser(BadUser);
            Assert.False(Bloggy.UserIsLoggedIn, "Det gick att logga in.");
        }
        [Test]
        public void LoginUser_Fail_UserNotInDatabase()
        {
            User UserDoesNotExist = new User("DoesNotExist");
            Bloggy.LoginUser(UserDoesNotExist);
            Assert.False(Bloggy.UserIsLoggedIn, "Det gick att logga in.");
        }
        [Test]
        public void LoginUser_Fail_BadPassword()
        {
            User UserBadPassword = new User("Christian");
            UserBadPassword.Password = "BadPassword";
            Bloggy.LoginUser(UserBadPassword);
            Assert.False(Bloggy.UserIsLoggedIn, "Det gick att logga in.");
        }
        [Test]
        public void LoginUser_Fail_NullUser()
        {
            //Hoppas att ArgumentNullException passar bra här
            Assert.Throws<ArgumentNullException>(() => Bloggy.LoginUser(null), "Det gick att logga in trots nullUser.");
        }
        #endregion


        // Försöker logga ut en användare. Kastar
        // exception om User är null.

        #region LogoutUser
        [Test]
        public void LogoutUser_Success()
        {
            //Arrange
            Bloggy.LoginUser(validUser);
            Assert.True(Bloggy.UserIsLoggedIn,
                "Det gick inte att logga in. OBS. Detta är förberedande");
            //Act
            Bloggy.LogoutUser(validUser);
            //Assert
            Assert.False(Bloggy.UserIsLoggedIn, "Det gick inte att logga ut.");
        }
        [Test]
        public void LogoutUser_Fail_NullUser()
        {
            //Arrange
            Bloggy.LoginUser(validUser);
            Assert.True(Bloggy.UserIsLoggedIn, 
                "Det gick inte att logga in. OBS. Detta är förberedande");
            //Act & Assert
            Assert.Throws<ArgumentNullException>(() => Bloggy.LogoutUser(null),
                "Inget Exception kastas");
        }
        #endregion

        // För att publicera en sida måste Page vara
        // ett giltigt Page-objekt och användaren
        // måste vara inloggad.
        // Returnerar true om det gick att publicera,
        // false om publicering misslyckades och
        // exception om Page har ett ogiltigt värde.

        #region PublishPage

        //Returns bool
        [Test]
        public void PublishPage_Success()
        {
            //Arrange
            Bloggy.LoginUser(validUser);
            Page ValidPage = new Page()
            {
                Title = ValidPageTitle,
                Content = ValidPageContent
            };
            //Act annd Assert
            Assert.True(Bloggy.PublishPage(ValidPage), 
                "Det gick inte att publisera min korrekta sida");
        }
        [Test]
        public void PublishPage_Fail_NoUserLoggedIn()
        {
            //Arrange
            Page ValidPage = new Page()
            {
                Title = ValidPageTitle,
                Content = ValidPageContent
            };
            //Act annd Assert
            Assert.False(Bloggy.PublishPage(ValidPage), 
                "Det gick att publisera min korrekta sida trots att ingen var inloggad.");
        }
        //Dessa ser jag som ogiltliga värden och därför förväntas exception
        [Test]
        [TestCase(null, ValidPageContent)]
        [TestCase("", ValidPageContent)]
        [TestCase(ValidPageTitle, null)]
        [TestCase(ValidPageTitle, "")]
        public void PublishPage_Fail_InvalidTitleOrContent(string pTitle,string pContent)
        {

            //Arrange
            Bloggy.LoginUser(validUser);
            Page invalidPage = new Page()
            {
                Title = pTitle,
                Content = pContent
            };
            //Act annd Assert
            //Man kan så klart ha andra Exceptions men jag vet inte vilket så jag 
            //tar detta standard Exception
            Assert.Throws<Exception>(()=>Bloggy.PublishPage(invalidPage),
                "Det gick att publisera min inkorrekta sida.");
        }
        [Test]
        public void PublishPage_Fail_NullPage()
        {
            Bloggy.LoginUser(validUser);

            //Man kan så klart ha andra Exceptions men jag vet inte vilket så jag 
            //tar detta standard Exception
            Assert.Throws<Exception>(() => Bloggy.PublishPage(null),
                "Det gick att publisera nullPage");
        } 
        #endregion


        // För att skicka e-post måste användaren vara
        // inloggad och alla parametrar ha giltiga värden.
        // Returnerar 1 om det gick att skicka mailet,
        // 0 annars.

        #region SendMail
        //Return int
        [Test]
        [TestCase(ValidAdress, ValidCaption, ValidBody)]
        public void SendEmail_Success(string address, string caption, string body)
        {
            Bloggy.LoginUser(validUser);
            Assert.AreEqual(1, Bloggy.SendEmail(address, caption, body));
        }
        [Test]
        [TestCase(null, ValidCaption,ValidBody )]
        [TestCase("", ValidCaption, ValidBody )]
        [TestCase(ValidAdress, null, ValidBody)]
        [TestCase(ValidAdress, "", ValidBody)]
        [TestCase(ValidAdress, ValidCaption, null)]
        [TestCase(ValidAdress, ValidCaption, "")]
        public void SendEmail_Fail_NullOrEmptyString(string address, string caption, string body)
        {
            Bloggy.LoginUser(validUser);
            Assert.AreEqual(0, Bloggy.SendEmail(address, caption, body));
        }
        [Test]
        public void SendEmail_Fail_NotLoggedIn()
        {
            Assert.AreEqual(0, Bloggy.SendEmail(ValidAdress, ValidCaption, ValidBody));
        }
        [Test]
        [TestCase("No_Snabel_A")]
        [TestCase("Too_Many_@@")]
        public void SendEmail_Fail_WrongAmountOf_CurlyA(string address)
        {
            Bloggy.LoginUser(validUser);
            Assert.AreEqual(0, Bloggy.SendEmail(address, ValidCaption, ValidBody));
        }
        #endregion

    }
}
