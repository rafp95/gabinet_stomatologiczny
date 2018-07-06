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

            string phone = "123-123-434";


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
           
            string code = "21-400";

            var isValid = CodeNumberCheck(code);

            Assert.AreEqual(true,isValid,"Błędny cod pocztowy");

        }

        
        private bool CodeNumberCheck(string code)
        {
            string pattern = @"^\d{2}-\d{3}$";
            return Regex.IsMatch(code, pattern);
        }

        //End Check kod pocztowy

        //Check date format
        [TestMethod]
        public  void verifyTestDateFormat()
        {
            string code = "21-405";

            var isValid = CodeDateCheck(code);

            Assert.AreEqual(true, isValid);
        }

        private bool CodeDateCheck(string code)
        {
            string pattern = @"^\d{2}-\d{3}$";
            return Regex.IsMatch(code,pattern);
        }
        //End Check date format

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

        //Check number

        [TestMethod]
        public void verifyNumberFormat()
        {
            var number = "333434234234324432423424";

            var isValid = CheckNumber(number);

            Assert.AreEqual(true, isValid);
        }

        private bool CheckNumber(string number)
        {
            string patter = "^[0-9]";
            return Regex.IsMatch(number, patter);
        }

        //end check number


    }


}
