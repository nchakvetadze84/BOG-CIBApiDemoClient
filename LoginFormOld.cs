using System;
using System.Windows.Forms;

namespace CIBApiDemoClient
{
    public partial class LoginFormOld : Form
    {
        public LoginFormOld()
        {
            InitializeComponent();
        }

        public string Url { get; set; }

        public Uri CallbackUri { get; set; }

        private void LoginFormLoad(object sender, EventArgs e)
        {
            webBrowser.Navigate(Url);
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
    }
}