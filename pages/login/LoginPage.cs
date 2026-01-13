using Microsoft.Playwright;
using zCustodiaUi.utils;
using ziGestaoTestsUi.data.login;
using ziGestaoTestsUi.locators.login;

namespace ziGestaoTestsUi.pages.login
{
    public class LoginPage
    {
        Utils _utils;
        LoginElements _el = new LoginElements();
        private readonly LoginData _data = new LoginData();
        private readonly IPage _page;
        public LoginPage(IPage _page, LoginData data = null)
        {
            this._page = _page;
            _utils = new Utils(_page);
            data = _data ?? new LoginData();
        }

        public async Task DoLogin()
        {
            await _utils.Write(_el.EmailField, _data.Email, "write email user on email Field to do Login");
            await _utils.Write(_el.PasswordField, _data.Password, "write password user on password Field to do Login");
            await _utils.Click(_el.SubmitButton, "Click on submit button to do Login");
        }



        public async Task ValidateUrl(string expectedUrl)
        {
            await _utils.ValidateUrl(expectedUrl, "Validate Url After been Did Login");
        }



    }
}
