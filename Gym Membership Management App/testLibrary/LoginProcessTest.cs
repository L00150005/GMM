using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using databaseLibrary;
using Xunit;

namespace testLibrary
{
    public class LoginProcessTest
    {
        [Theory]
        [InlineData("ajames", "ajames12", true)]
        [InlineData("", "password", false)]
        [InlineData("brian", "", false)]
        [InlineData("a", "Letmein12345", false)]
        [InlineData("collison", "1", false)]
        [InlineData("123456789123456789123456789", "password", false)]
        [InlineData("brian", "123456789123456789123456789", false)]

        public void ValidateUserInput_StringShouldVerify(string username, string password, Boolean expected)
        {
            //Arange
            LoginProcess loginProcess = new LoginProcess();


            //Act
            Boolean actual = loginProcess.blnValidateUserInput(username, password);


            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
