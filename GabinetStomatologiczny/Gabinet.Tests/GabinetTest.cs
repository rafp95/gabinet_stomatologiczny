using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GabinetStomatologiczny.Models;
using System.Text.RegularExpressions;

namespace Gabinet.Tests
{
    [TestClass]
    public class GabinetTest
    {

        //Check phone number
        [TestMethod]
        public void VerifyPhoneNumber()
        {
            //Arrange

            string phone = "450-211-211";


            //Act 
            var isValid = PhoneNumberCheck(phone);

            //Assert
            Assert.AreEqual(true, isValid,"Błędny numer telefonu");
        }

        private bool PhoneNumberCheck(string email)
        {
            const string regular = @"([\+]){0,1}([0-9]{2})?[\-\s]?[-]?([0-9]{3})\-?[-\s]?([0-9]{3})[-\s]\-?([0-9]{3})$";
            return Regex.IsMatch(email, regular);
        }

        //end check phone number


        //check kod pocztowy
        [TestMethod]
        public void verifyPostalCode()
        {
           
            string code = "21-500";

            var isValid = CodeNumberCheck(code);

            Assert.AreEqual(true,isValid,"Błędny kod pocztowy");

        }

        
        private bool CodeNumberCheck(string code)
        {
            string pattern = @"^\d{2}-\d{3}$";
            return Regex.IsMatch(code, pattern);
        }

        //End Check kod pocztowy
    
        //Check pesel
        [TestMethod]
        public void peselCheck()
        {
            var pesel = "95030410314";

            var isValid = CheckPesel(pesel);

            Assert.AreEqual(true,isValid);

        }


        private bool CheckPesel(string pesel)
        {
            string regularPesel = @"^\d{11}$";

            return Regex.IsMatch(pesel, regularPesel);

        }

        //End check pesel
    }


}
