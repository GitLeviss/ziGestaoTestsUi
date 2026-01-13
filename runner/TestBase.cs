using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using zCustodiaUi.utils;

namespace zCustodiaUi.runner
{
    public abstract class TestBase
    {
        protected IPage _page;
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private IBrowserContext? _context;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            VideoHelper.ClearOldVideos();
        }
        protected async Task<IPage> OpenBrowserAsync()
        {
            _playwright = await Playwright.CreateAsync();

            var launchOptions = new BrowserTypeLaunchOptions
            {
                Headless = false,
                Args = new[]
                {
                    "--no-sandbox",
                    "--disable-dev-shm-usage",
                    "--disable-gpu",
                    "--disable-setuid-sandbox"
                }
            };

            _browser = await _playwright.Chromium.LaunchAsync(launchOptions);

            var videosDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "videos");
            Directory.CreateDirectory(videosDir);

            var contextOptions = new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
                IgnoreHTTPSErrors = true,
                RecordVideoDir = videosDir,
                RecordVideoSize = new RecordVideoSize { Width = 1366, Height = 768 }
            };

            _context = await _browser.NewContextAsync(contextOptions);
            _page = await _context.NewPageAsync();
            _page.SetDefaultTimeout(60000);
            _page.SetDefaultNavigationTimeout(60000);
            _page.Console += (_, msg) =>
            {
                if (msg.Type == "error")
                    TestContext.Out.WriteLine($"⚠ Console Error Ignored: {msg.Text}");
            };
            var config = new ConfigurationManager();
            var envStg = Environment.GetEnvironmentVariable("ZIGESTORA_LINK");
            config.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
            var linkManager = config["Links:Manager"] ?? envStg;
            _page.DOMContentLoaded += async (sender, e) =>
            {
                // Injeta o estilo CSS para aplicar o zoom de 75%
                await _page.AddStyleTagAsync(new PageAddStyleTagOptions
                {
                    Content = "body { zoom: 0.75; }"
                });
            };
            await _page.GotoAsync(linkManager!, new PageGotoOptions
            {
                Timeout = 60000,
                WaitUntil = WaitUntilState.NetworkIdle
            });
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await WaitForOverlayToDisappear(_page);

            return _page;
        }
        protected async Task CloseBrowserAsync()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status.ToString();

            try
            {
                if (_page != null)
                {
                    await VideoUtils.ForceVideoFinalization(_page);
                }

                if (_context != null)
                {
                    await _context.CloseAsync();
                }

                if (_page != null)
                {
                    await VideoHelper.AttachVideoAsync(_page, status);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar vídeo no teardown: {ex.Message}");
            }
            finally
            {
                try
                {
                    if (_browser != null)
                    {
                        await _browser.CloseAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao fechar browser: {ex.Message}");
                }

                try
                {
                    _playwright?.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao dispose playwright: {ex.Message}");
                }
            }
        }


        /// <summary>
        /// Evita Angular Material bloquear cliques com overlays invisíveis.
        /// </summary>
        protected async Task WaitForOverlayToDisappear(IPage page)
        {
            await _page.WaitForFunctionAsync(
                "() => !document.querySelector('.cdk-overlay-backdrop')"
            );
        }
    }
}
