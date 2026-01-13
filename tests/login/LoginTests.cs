using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using zCustodiaUi.runner;
using ziGestaoTestsUi.builders.login;
using ziGestaoTestsUi.data.login;
using ziGestaoTestsUi.pages.login;

namespace zCustodiaUi.tests.login
{
    [AllureNUnit]
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [AllureOwner("Levi")]
    [AllureSeverity(SeverityLevel.critical)]
    [Category("Critícity: Critical")]
    [Category("Regression Tests")]
    [AllureSuite("Login UI")]
    public class LoginTests : TestBase
    {

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            _page = await OpenBrowserAsync();
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await CloseBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Should Do Login With Valid Credentials")]
        public async Task Should_Do_Login_With_Valid_Credentials()
        {
            var loginPage = new LoginPage(_page);
            await new LoginBuilder(loginPage)
                .DoLogin()
                .ValidateUrl("https://zigestao.zitec.ai/inicio")
                .Execute();
        }
        [Test, Order(2)]
        [AllureName("Do Not Should Do Login With Invalid Email")]
        public async Task Do_Not_Should_Do_Login_With_Invalid_Email()
        {
            var dataTest = new LoginData
            {
                Email = "Invalid@email.com"
            };
            var loginPage = new LoginPage(_page, dataTest);
            await new LoginBuilder(loginPage)
                .DoLogin()
                .Execute();
        }
        [Test, Order(3)]
        [AllureName("Do Not Should Do Login With Invalid Password")]
        public async Task Do_Not_Should_Do_Login_With_Invalid_Password()
        {
            var dataTest = new LoginData
            {
                Password = "Invalid"
            };
            var loginPage = new LoginPage(_page, dataTest);
            await new LoginBuilder(loginPage)
                .DoLogin()
                .Execute();
        }

    }
}
