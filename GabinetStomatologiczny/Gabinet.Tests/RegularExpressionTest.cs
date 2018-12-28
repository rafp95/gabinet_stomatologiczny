using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GabinetStomatologiczny.Models;
using System.Text.RegularExpressions;

namespace Gabinet.Tests
{
    [TestClass]
    public class RegularExpressionTest
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
            Assert.AreEqual(true, isValid, "Błędny numer telefonu");
        }

        private bool PhoneNumberCheck(string email)
        {
            const string regular = @"([\+]){0,1}([0-9]{2})?[\-\s]?[-]?([0-9]{3})\-?[-\s]?([0-9]{3})[-\s]\-?([0-9]{3})$";
            return Regex.IsMatch(email, regular);
        }

        //end check phone number


        //check kod pocztowy
        [TestMethod]
        public void VerifyPostalCode()
        {
            //Arrange
            string code = "21-400";
            string codeFailed = "dd-dssd";
            //Act 
            var isValid = CodeNumberCheck(code);
            var isFailde = CodeNumberCheck(codeFailed);
            //Assert
            Assert.AreEqual(true, isValid);
            Assert.AreEqual(false, isFailde);
        }


        private bool CodeNumberCheck(string code)
        {
            string pattern = @"^\d{2}-\d{3}$";
            return Regex.IsMatch(code, pattern);
        }

        //End Check kod pocztowy

        //Check pesel
        [TestMethod]
        public void PeselCheck()
        {
            var pesel = "95030410314";

            var isValid = CheckPesel(pesel);

            Assert.AreEqual(true, isValid);

        }


        private bool CheckPesel(string pesel)
        {
            string regularPesel = @"^\d{11}$";

            return Regex.IsMatch(pesel, regularPesel);

        }

        //End check pesel

        //zabieg price test
        [TestMethod]
        public void CheckPriceIsValid()
        {
            var price1 = "100";
            var price2 = "-100";

            Assert.AreEqual(true, CheckPrice(price1));
            Assert.AreEqual(false, CheckPrice(price2));
        }


        private bool CheckPrice(string price)
        {
            string regularPrice = @"^\s*(?=.*[1-9])\d*(?:\,\d{1,2})?\s*$";

            return Regex.IsMatch(price, regularPrice);
        }

        //zabieg price test end
    }


}
