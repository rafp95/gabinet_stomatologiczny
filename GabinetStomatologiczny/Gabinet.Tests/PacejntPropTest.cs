using GabinetStomatologiczny.DAO.Models;
using GabinetStomatologiczny.Share;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gabinet.Tests
{
    [TestClass]
    public class PacejntPropTest
    {
        [TestMethod]
        public void TestValidationAtributePacjents()
        {
            CheckPropertyValidation check = new CheckPropertyValidation();

            var p = new Pacjent()
            {
                Imie = "Patryk",
                Nazwisko = "Rafał",
                DataUrodzenia = DateTime.MinValue,
                NrTelefonu = "672-211-211",
                Email = "patrykrafal33@o2.pl",
            };

            var errorcount = check.myValidation(p).Count;

            Assert.AreEqual(0, errorcount);
        }

        [TestMethod]
        public void TestPacjentProp()
        {
            const string expectedFirstName = "Patryk";
            const string expectedLastName = "Rafał";
            const string expectedEmail = "patrykrafal33@o2.pl";
            const OddzialNfz expetcedOddzialNfz = OddzialNfz.Kujawsko_Pomorski;
            DateTime expectedRegisterDate = DateTime.UtcNow;

            var pacjent = new Pacjent()
            {
                Imie = expectedFirstName
                ,
                Nazwisko = expectedLastName
                ,
                Email = expectedEmail
                 ,
                OdzialNfz = expetcedOddzialNfz
                ,
                DataRejestracji = expectedRegisterDate
            };


            Assert.AreEqual(pacjent.Imie, expectedFirstName, false, "Błędne imie");
            Assert.AreEqual(pacjent.Nazwisko, expectedLastName, false, "Błędne nazwisko");
            Assert.AreEqual(pacjent.Email, expectedEmail, false, "Błędne email");
            Assert.AreEqual(pacjent.OdzialNfz, expetcedOddzialNfz, "Błędny oddzial NFZ");
            Assert.AreEqual(pacjent.DataRejestracji, expectedRegisterDate, "Błędna data rejestracji");
            

        }
    }
}
