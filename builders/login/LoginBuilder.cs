using zCustodiaUi.builders;
using ziGestaoTestsUi.pages.login;

namespace ziGestaoTestsUi.builders.login
{
    public class LoginBuilder : TestBuilder
    {
        private readonly LoginPage _loginPage;

        public LoginBuilder(LoginPage loginPage)
        {
            _loginPage = loginPage;
        }

        public LoginBuilder DoLogin()
        {
            AddStep(async () => await _loginPage.DoLogin());
            return this;
        }


        public LoginBuilder ValidateUrl(string expectedUrl)
        {
            AddStep(async () => await _loginPage.ValidateUrl(expectedUrl));
            return this;
        }
    }
}
