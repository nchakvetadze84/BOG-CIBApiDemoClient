using System;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace CIBApiDemoClient
{
    public partial class LoginFormNew : Form
    {
        public ChromiumWebBrowser chromeBrowser;

        public LoginFormNew(string url)
        {
            Url = url;
            InitializeComponent();
            // Start the browser after initialize global component
            InitializeChromium();
        }

        public string Url { get; set; }

        public Uri CallbackUri { get; set; }

        private void LoginFormLoad(object sender, EventArgs e)
        {
            //webBrowser.Navigate(Url);
        }
        
        public AuthorizeResponse AuthorizeResponse { get; set; }

        private void WebBrowserNavigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url.ToString().StartsWith(CallbackUri.AbsoluteUri))
            {
                AuthorizeResponse = new AuthorizeResponse(e.Url.AbsoluteUri);
                
                Close();
            }
        }
        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            // Initialize cef with the provided settings
            Cef.Initialize(settings);
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser(Url);
            // Add it to the form and fill it to the form window.
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

            chromeBrowser.AddressChanged += AddressChanged;

        }

        private void LoginForm_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        private void AddressChanged(object sender, AddressChangedEventArgs e)
        {
            if (e.Address.ToString().StartsWith(CallbackUri.AbsoluteUri))
            {
                AuthorizeResponse = new AuthorizeResponse(e.Address);


                //Close();
            }
        }
    }
}