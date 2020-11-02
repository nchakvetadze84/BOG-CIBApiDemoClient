using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

using CIBApiDemoClient.Model;

using Newtonsoft.Json.Linq;

using Thinktecture.IdentityModel.Client;

namespace CIBApiDemoClient
{
    public partial class DemoClientMainForm : Form
    {
        private string CallbackUri = ConfigurationManager.AppSettings["CallbackUri"];
        private string ClientId = ConfigurationManager.AppSettings["ClientId"];

        private string SecretKey = ConfigurationManager.AppSettings["SecretKey"];
        private string SecretKeyClientId = ConfigurationManager.AppSettings["SecretKeyClientId"];
        private string SecretKeyTokenIssuer = ConfigurationManager.AppSettings["SecretKeyTokenIssuer"];

        private const string format = "yyyy-MM-dd";

        private string AuthorizeUrl = ConfigurationManager.AppSettings["AuthorizeUrl"];
        private string AuthorizeUrlOld = ConfigurationManager.AppSettings["AuthorizeUrlOld"];
        private string ApiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

        private TokenResponse tokenResponse;
        private AuthorizeResponse authorizeResponse;

        private Timer timer;
        private long counter;

        public DemoClientMainForm()
        {
            InitializeComponent();

            InitializeForm();

            //Do not use in production code. The certificate in live environment will be valid so do not ignore validation errors
#if DEBUG
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
#endif
        }

        private void InitializeForm()
        {
            statementCurrencyComboBox.SelectedIndex = accountCurrencyComboBox.SelectedIndex = currencyComboBox.SelectedIndex = currencyToComboBox.SelectedIndex = 0;
            domesticPaymentBindingSource.DataSource = new DomesticPayment();
            foreignPaymentBindingSource.DataSource = new ForeignPayment();
            conversionPaymentBindingSource.DataSource = new ConversionPayment();

            balanceSheetBindingSource.DataSource = new BalanceSheet();
            balanceSheetEntryBindingSource.DataSource = new BalanceSheetEntry();

            statementFromDateTimePicker.Value = DateTime.Today.AddDays(-7);
            statementDetailBindingSource.DataSource = new List<StatementDetail>();
            todayActivityDetailBindingSource.DataSource = new List<TodayActivityDetail>();

            domesticDocumentKeyBindingSource.ResetBindings(false);
            foreignPaymentBindingSource.ResetBindings(false);
            conversionPaymentBindingSource.ResetBindings(false);
            statementDetailBindingSource.ResetBindings(false);
            todayActivityDetailBindingSource.ResetBindings(false);
        }

        private void AssertToken()
        {
            if ((tokenResponse == null || tokenResponse.IsError) && (authorizeResponse == null || authorizeResponse.ResponseType == AuthorizeResponse.ResponseTypes.Error))
            {
                throw new InvalidOperationException("Get the token");
            }
        }

        private HttpClient InitializeClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(ApiBaseUrl)
            };

            client.SetBearerToken(tokenResponse?.AccessToken ?? authorizeResponse?.AccessToken);
            return client;
        }


        private void GetTokenButtonClick(object sender, EventArgs e)
        {
            var client = new OAuth2Client(new Uri(AuthorizeUrl));
            var state = Guid.NewGuid().ToString();

            var startUrl = client.CreateAuthorizeUrl(ClientId, "token", "corp", CallbackUri, null, state, null, null, null, new Dictionary<string, string> { { "kc_locale", "ka" } });

            var loginForm = new LoginFormNew(startUrl) { Url = startUrl, CallbackUri = new Uri(CallbackUri) };

            loginForm.ShowDialog();

            if (loginForm.AuthorizeResponse != null)
            {
                authorizeResponse = loginForm.AuthorizeResponse;
                ShowResponseInfo(authorizeResponse);

                InitializeForm();

                counter = authorizeResponse.ExpiresIn;
                timer = new Timer();
                timer.Tick += CounterTimerTick;
                timer.Interval = 1000;
                timer.Start();

                counterLabel.Text = $"{counter} seconds";
            }
        }

        private void CounterTimerTick(object sender, EventArgs e)
        {
            counter--;
            if (counter == 0)
            {
                timer.Stop();
                statusLabel.Text = "Token Expired!";
                counterLabel.Text = string.Empty;
            }
            else
            {
                counterLabel.Text = $"{counter} seconds";
            }
        }



        private async void SecretKeyTokenButtonClick(object sender, EventArgs e)
        {
            var client = new OAuth2Client(new Uri(SecretKeyTokenIssuer), SecretKeyClientId, SecretKey);
            tokenResponse = await client.RequestClientCredentialsAsync("corp");
            ShowResponseInfo(tokenResponse);
        }

        private void CopyTokenButtonClick(object sender, EventArgs e)
        {
            try
            {
                AssertToken();
                Clipboard.SetText(tokenResponse.AccessToken);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void ShowResponseInfo(TokenResponse token)
        {
            if (token.IsError)
            {
                statusLabel.Text = string.Format("Error: {0}", token.Error);
            }
            else
            {
                statusLabel.Text = "Token available. Expires in: ";
            }
        }

        private void ShowResponseInfo(AuthorizeResponse response)
        {
            if (response.ResponseType == AuthorizeResponse.ResponseTypes.Error)
            {
                statusLabel.Text = string.Format("Error: {0}", response.Error);
            }
            else
            {
                statusLabel.Text = "Token available. Expires in: ";
            }
        }


        private static async Task ShowErrorMessage(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            MessageBox.Show(string.IsNullOrEmpty(content) ? response.ReasonPhrase : content, "Error");
        }

        private static void NullsToEmptyString<T>(T instance)
        {
            var type = instance.GetType();
            var properties = type.GetProperties();
            foreach (var propertyInfo in properties)
            {
                var property = type.GetProperty(propertyInfo.Name, typeof(string));
                if (property != null)
                {
                    var value = (string)property.GetValue(instance);

                    if (value == null)
                    {
                        property.SetValue(instance, "");
                    }
                }
            }
        }


        private async void NbgRateButtonClick(object sender, EventArgs e)
        {
            try
            {
                AssertToken();

                var client = InitializeClient();
                var response = await client.GetAsync(string.Format("rates/nbg/{0}", currencyComboBox.SelectedItem));

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    rateLabel.Text = json;
                }
                else
                {
                    MessageBox.Show(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BogRateButtonClick(object sender, EventArgs e)
        {
            try
            {
                AssertToken();

                var client = InitializeClient();
                var response = await client.GetAsync(string.Format("rates/commercial/{0}", currencyComboBox.SelectedItem));

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(json);
                    rateLabel.Text = string.Format("Buy: {0} Sell: {1}", jObject["Buy"].Value<decimal>(), jObject["Sell"].Value<decimal>());
                }
                else
                {
                    MessageBox.Show(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void CrossRatebuttonClick(object sender, EventArgs e)
        {
            try
            {
                AssertToken();

                var client = InitializeClient();
                var response = await client.GetAsync(string.Format("rates/commercial/{0}/{1}", currencyComboBox.SelectedItem, currencyToComboBox.SelectedItem));

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    rateLabel.Text = json;
                }
                else
                {
                    MessageBox.Show(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void NbgHistoryButtonClick(object sender, EventArgs e)
        {
            try
            {
                AssertToken();

                var client = InitializeClient();

                var response = await client.GetAsync(string.Format("rates/nbg/{0}/{1}/{2}/", currencyComboBox.SelectedItem,
                                                     rateFromDateTimePicker.Value.ToString(format), rateToDateTimePicker.Value.ToString(format)));

                if (response.IsSuccessStatusCode)
                {
                    var history = await response.Content.ReadAsAsync<List<NbgCurrencyHistory>>();
                    nbgCurrencyHistoryBindingSource.DataSource = history;
                    nbgCurrencyHistoryBindingSource.ResetBindings(false);
                }
                else
                {
                    await ShowErrorMessage(response);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void BalanceButtonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(accountTextBox.Text))
            {
                MessageBox.Show("Enter Account Number");
                return;
            }

            AssertToken();

            var client = InitializeClient();

            var response = await client.GetAsync(string.Format("accounts/{0}/{1}", accountTextBox.Text, accountCurrencyComboBox.SelectedItem));
            if (response.IsSuccessStatusCode)
            {
                var balance = await response.Content.ReadAsAsync<AccountBalance>();
                accountBalanceBindingSource.DataSource = balance;
                accountBalanceBindingSource.ResetBindings(false);
            }
            else
            {
                await ShowErrorMessage(response);
            }
        }


        private async void CreatePaymentButtonClick(object sender, EventArgs e)
        {
            AssertToken();

            var client = InitializeClient();

            var payment = domesticPaymentBindingSource.DataSource as DomesticPayment;

            NullsToEmptyString(payment);

            bulkDomesticLabel.Text = "";

            if (bulkDomesticCheckBox.Checked)
            {
                var response = await client.PostAsJsonAsync("documents/bulk/domestic", new[] { payment });
                if (response.IsSuccessStatusCode)
                {
                    var bulkKey = await response.Content.ReadAsAsync<long>();
                    bulkDomesticLabel.Text = bulkKey.ToString();
                }
                else
                {
                    await ShowErrorMessage(response);
                }
            }
            else
            {
                var response = await client.PostAsJsonAsync("documents/domestic", new[] { payment });
                if (response.IsSuccessStatusCode)
                {
                    var keys = await response.Content.ReadAsAsync<DocumentKey[]>();
                    domesticDocumentKeyBindingSource.DataSource = keys[0];
                }
                else
                {
                    await ShowErrorMessage(response);
                }
            }
        }

        private async void CreateForeignPaymentButtonClick(object sender, EventArgs e)
        {
            AssertToken();

            var client = InitializeClient();

            var payment = foreignPaymentBindingSource.DataSource as ForeignPayment;

            NullsToEmptyString(payment);

            var response = await client.PostAsJsonAsync("documents/foreign", new[] { payment });

            if (response.IsSuccessStatusCode)
            {
                var keys = await response.Content.ReadAsAsync<DocumentKey[]>();
                foreignDocumentKeyBindingSource.DataSource = keys[0];
            }
            else
            {
                await ShowErrorMessage(response);
            }
        }

        private async void CreateConversionPaymentButtonClick(object sender, EventArgs e)
        {
            AssertToken();

            var client = InitializeClient();

            var payment = conversionPaymentBindingSource.DataSource as ConversionPayment;

            NullsToEmptyString(payment);

            var response = await client.PostAsJsonAsync("documents/conversion", new[] { payment });

            if (response.IsSuccessStatusCode)
            {
                var keys = await response.Content.ReadAsAsync<DocumentKey[]>();
                conversionDocumentKeybindingSource.DataSource = keys[0];
            }
            else
            {
                await ShowErrorMessage(response);
            }
        }


        private async void StatementButtonClick(object sender, EventArgs e)
        {
            AssertToken();

            var client = InitializeClient();

            var response = await client.GetAsync(string.Format("statement/{0}/{1}/{2}/{3}", statementAccountTextBox.Text, statementCurrencyComboBox.SelectedItem,
                                                 statementFromDateTimePicker.Value.ToString(format), statementToDateTimePicker.Value.ToString(format)));
            if (response.IsSuccessStatusCode)
            {
                var statement = await response.Content.ReadAsAsync<Statement>();
                statementDetailBindingSource.DataSource = statement.Records;
                statementDetailBindingSource.ResetBindings(false);

                var summaryResponse = await client.GetAsync(String.Format("statement/summary/{0}/{1}/{2}/", statementAccountTextBox.Text, statementCurrencyComboBox.SelectedItem, statement.Id));

                if (summaryResponse.IsSuccessStatusCode)
                {
                    var summary = await summaryResponse.Content.ReadAsAsync<StatementSummary>();
                    globalSummaryBindingSource.DataSource = summary.GlobalSummary;
                    dailySummaryBindingSource.DataSource = summary.DailySummaries;

                    globalSummaryBindingSource.ResetBindings(false);
                    dailySummaryBindingSource.ResetBindings(false);
                }
            }
            else
            {
                await ShowErrorMessage(response);
            }
        }


        private async void DocumentStatusButtonClick(object sender, EventArgs e)
        {
            AssertToken();

            var client = InitializeClient();

            var response = await client.GetAsync(string.Format("documents/statuses/{0}", documentKeyTextBox.Text));

            if (response.IsSuccessStatusCode)
            {
                var status = await response.Content.ReadAsAsync<List<DocumentStatus>>();
                documentStatusBindingSource.DataSource = status;
                documentStatusBindingSource.ResetBindings(false);
            }
            else
            {
                await ShowErrorMessage(response);
            }
        }

        private async void BulkStatusButtonClick(object sender, EventArgs e)
        {
            AssertToken();

            var client = InitializeClient();

            var response = await client.GetAsync(string.Format("documents/bulk/status/{0}", bulkIDTextBox.Text));

            if (response.IsSuccessStatusCode)
            {
                var status = await response.Content.ReadAsAsync<BulkPaymentStatus>();

                bulkPaymentStatusBindingSource.DataSource = status;
                documentStatusesBindingSource.DataSource = status.DocumentStatuses;

                documentStatusesBindingSource.ResetBindings(false);
                bulkPaymentStatusBindingSource.ResetBindings(false);
            }
            else
            {
                await ShowErrorMessage(response);
            }
        }



        private async void CancelDocumentButtonClick(object sender, EventArgs e)
        {
            AssertToken();

            var client = InitializeClient();

            var response = await client.DeleteAsync(string.Format("documents/{0}", documentKeyTextBox.Text));

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("გაუქმებულია");
            }
            else
            {
                await ShowErrorMessage(response);
            }
        }

        private async void CancelBulkButtonClick(object sender, EventArgs e)
        {
            AssertToken();

            var client = InitializeClient();

            var response = await client.DeleteAsync(string.Format("documents/bulk/{0}", bulkIDTextBox.Text));

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("გაუქმებულია");
            }
            else
            {
                await ShowErrorMessage(response);
            }
        }



        private async void TodayActivityButtonClick(object sender, EventArgs e)
        {
            AssertToken();

            var client = InitializeClient();

            var response = await client.GetAsync(string.Format("documents/todayactivities/{0}/{1}", todayActivityAccountTextBox.Text, todayActivityCurrencyComboBox.SelectedItem));
            if (response.IsSuccessStatusCode)
            {
                var todayActivities = await response.Content.ReadAsAsync<List<TodayActivityDetail>>();
                todayActivityDetailBindingSource.DataSource = todayActivities;
                todayActivityDetailBindingSource.ResetBindings(false);
            }
            else
            {
                await ShowErrorMessage(response);
            }
        }

        private async void CreateBalanceSheetButtonClick(object sender, EventArgs e)
        {
            AssertToken();

            var client = InitializeClient();

            var balanceSheet = balanceSheetBindingSource.DataSource as BalanceSheet;
            var balanceSheetEntry = balanceSheetEntryBindingSource.DataSource as BalanceSheetEntry;

            balanceSheet.BalanceSheetEntries = new List<BalanceSheetEntry> { balanceSheetEntry };

            //NullsToEmptyString(balanceSheet);
            //NullsToEmptyString(balanceSheetEntry);

            var response = await client.PostAsJsonAsync("BalanceSheet", new { balanceSheet });

            if (response.IsSuccessStatusCode)
            {
                resultCodelblBalanceSheet.Text = "OK";
            }
            else
            {
                await ShowErrorMessage(response);
            }
        }

        private void getTokenOldButton_Click(object sender, EventArgs e)
        {
            var client = new OAuth2Client(new Uri(AuthorizeUrlOld));
            var state = Guid.NewGuid().ToString();
            var startUrl = client.CreateAuthorizeUrl(ClientId, "token", "corp", CallbackUri, state);

            var loginForm = new LoginFormOld { Url = startUrl, CallbackUri = new Uri(CallbackUri) };

            loginForm.ShowDialog();

            if (loginForm.AuthorizeResponse != null)
            {
                authorizeResponse = loginForm.AuthorizeResponse;
                ShowResponseInfo(authorizeResponse);

                InitializeForm();

                counter = authorizeResponse.ExpiresIn;
                timer = new Timer();
                timer.Tick += CounterTimerTick;
                timer.Interval = 1000;
                timer.Start();

                counterLabel.Text = $"{counter} seconds";
            }
        }
    }
}