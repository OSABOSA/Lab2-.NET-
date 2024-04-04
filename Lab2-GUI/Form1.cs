using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lab2_GUI
{
    public partial class Form1 : Form
    {
        private HttpClient client;
        
        public Form1()
        {
            InitializeComponent();
            LoadCurrenciesAsync();
            client = new HttpClient();
            baseCurrencyComboBox.SelectedIndex = 149;
            baseCurrencyComboBox.SelectedIndexChanged += comboBoxBaseCurrency_SelectedIndexChanged;
            baseAmount.ValueChanged += baseAmount_ValueChanged;
            comboBoxNewCurrency.SelectedIndexChanged += comboBoxNewCurrency_SelectedIndexChanged;
        }

        private void CalculateAmount()
        {
            string selectedBaseCurrency = baseCurrencyComboBox.SelectedItem.ToString().Split(" ")[0];
            string selectedNewCurrency = comboBoxNewCurrency.SelectedItem.ToString().Split(" ")[0];
            chosenCurrency.Text = selectedNewCurrency;

            // Show all exchange rates (optional)
            ShowAllExchangeRates();

            using (var dbContext = new CurrencyDbContext())
            {
                // Ensure the table for the base currency exists
                var tableExists = dbContext.Database.CanConnect();
                if (!tableExists)
                {
                    MessageBox.Show($"Table for {selectedBaseCurrency} does not exist.");
                    return;
                }

                var exchangeRate = dbContext.Set<ExchangeRate>().FromSqlRaw($"SELECT * FROM {selectedBaseCurrency} WHERE CurrencyCode = '{selectedNewCurrency}'").FirstOrDefault();

                if (exchangeRate != null)
                {
                    decimal value = baseAmount.Value;
                    decimal convertedAmount = value * exchangeRate.Rate;
                    newAmount.Text = convertedAmount.ToString();
                }
                else
                {
                    MessageBox.Show($"Exchange rate not found for the selected pair {selectedBaseCurrency} to {selectedNewCurrency}.");
                }
            }
        }

        private async void comboBoxBaseCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            await UpdateExchangeRatesAsync();
            CalculateAmount();
        }

        private async void comboBoxNewCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            await UpdateExchangeRatesAsync();
            CalculateAmount();
        }

        private async void baseAmount_ValueChanged(object sender, EventArgs e)
        {
            CalculateAmount();
        }

        private async Task LoadCurrenciesAsync()
        {
            // Check if currencies exist in the database
            List<Currency> currenciesFromDatabase;
            using (var dbContext = new CurrencyListDbContext())
            {
                currenciesFromDatabase = await dbContext.Currencies.ToListAsync();
            }

            if (currenciesFromDatabase != null && currenciesFromDatabase.Any())
            {
                currenciesLoadedLabel.Text = "Loaded from database";
                // Currencies exist in the database, populate ComboBox
                foreach (var currency in currenciesFromDatabase)
                {
                    baseCurrencyComboBox.Items.Add($"{currency.Code} - {currency.Name}");
                    comboBoxNewCurrency.Items.Add($"{currency.Code} - {currency.Name}");
                }

                // Select the first currency
                //baseCurrencyComboBox.SelectedIndex = 0;
            }
            else
            {
                currenciesLoadedLabel.Text = "Loaded from API";
                // Currencies not found in the database, fetch from API
                string baseUrl = "https://openexchangerates.org/api/currencies.json";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Add API key to query string
                        string url = $"{baseUrl}";

                        // Make the API request
                        HttpResponseMessage response = await client.GetAsync(url);

                        // Check if the request was successful
                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            var currencies = JsonSerializer.Deserialize<Dictionary<string, string>>(responseBody);

                            using (var dbContext = new CurrencyListDbContext())
                            {
                                // Add currencies to database
                                foreach (var currency in currencies)
                                {
                                    dbContext.Currencies.Add(new Currency { Code = currency.Key, Name = currency.Value });
                                }

                                await dbContext.SaveChangesAsync();
                            }

                            // Populate ComboBox
                            foreach (var currency in currencies)
                            {
                                baseCurrencyComboBox.Items.Add($"{currency.Key} - {currency.Value}");
                                comboBoxNewCurrency.Items.Add($"{currency.Key} - {currency.Value}");
                            }

                            // Select the first currency
                            //baseCurrencyComboBox.SelectedIndex = 0;

                            MessageBox.Show("Currencies saved to database and ComboBox populated successfully.");
                        }
                        else
                        {
                            MessageBox.Show($"Error occurred while fetching currencies. Status code: {response.StatusCode}");
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        MessageBox.Show($"An error occurred: {e.Message}");
                    }
                }
            }
        }

        private async Task UpdateExchangeRatesAsync()
        {
            string apiKey = "9f0de56e2457422e8e5cebb93e5f8426";
            string baseUrl = "https://openexchangerates.org/api/latest.json";

            if (comboBoxNewCurrency.SelectedIndex == -1 || baseCurrencyComboBox.SelectedIndex == -1)
            {
                return;
            }

            string baseCurrency = baseCurrencyComboBox.SelectedItem.ToString().Split(" ")[0];
            string targetCurrency = comboBoxNewCurrency.SelectedItem.ToString().Split(" ")[0];

            var queryParams = new Dictionary<string, string>
    {
        { "app_id", apiKey },
        { "base", baseCurrency },
        { "symbols", targetCurrency }
    };

            string queryString = string.Join("&", queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
            string url = $"{baseUrl}?{queryString}";

            using (var dbContext = new CurrencyDbContext())
            {
                // Ensure the table for the base currency exists
                dbContext.Database.EnsureCreated();

                // Check if the table exists
                var tableExists = dbContext.Database.CanConnect();
                if (!tableExists)
                {
                    MessageBox.Show($"Table for {baseCurrency} does not exist.");
                    return;
                }

                // Make the API request
                HttpResponseMessage response = await client.GetAsync(url);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<ExchangeRateResponse>(responseBody);

                    // Add new exchange rates to the table
                    foreach (var rate in data.rates)
                    {
                        // Check if the currency already exists in the database
                        var existingRate = dbContext.ExchangeRates.FirstOrDefault(r => r.CurrencyCode == rate.Key);

                        // If it doesn't exist, insert it into the database
                        if (existingRate == null)
                        {
                            dbContext.Database.ExecuteSqlRaw($"CREATE TABLE IF NOT EXISTS {baseCurrency} (CurrencyCode TEXT PRIMARY KEY, Rate REAL);");
                            dbContext.Database.ExecuteSqlRaw($"INSERT INTO {baseCurrency} (CurrencyCode, Rate) VALUES ('{rate.Key}', {rate.Value});");
                        }
                    }
                }
            }
        }


        private void ShowAllExchangeRates()
        {
            string selectedBaseCurrency = baseCurrencyComboBox.SelectedItem.ToString().Split(" ")[0];

            using (var dbContext = new CurrencyDbContext())
            {
                // Ensure the table for the base currency exists
                var tableExists = dbContext.Database.CanConnect();
                if (!tableExists)
                {
                    MessageBox.Show($"Table for {selectedBaseCurrency} does not exist.");
                    return;
                }

                // Retrieve all exchange rates from the appropriate table
                var allExchangeRates = dbContext.Set<ExchangeRate>().FromSqlRaw($"SELECT * FROM {selectedBaseCurrency}").ToList();

                // Clear the text box
                allExchangeRatesTextBox.Clear();

                // Display exchange rates in the text box
                foreach (var rate in allExchangeRates)
                {
                    allExchangeRatesTextBox.AppendText($"{rate.CurrencyCode}: {rate.Rate}\n");
                }
            }
        }
    }

    public class ExchangeRate
    {
        [Key]
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
    }

    public class CurrencyDbContext : DbContext
    {
        public DbSet<ExchangeRate> ExchangeRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExchangeRate>().ToTable("ExchangeRates");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=rates.db");
        }
    }

    public class Currency
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class CurrencyListDbContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }

        public CurrencyListDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=currencies_list.db");
    }

    public class ExchangeRateResponse
    {
        public string @base { get; set; }
        public DateTime time { get; set; }
        public System.Collections.Generic.Dictionary<string, decimal> rates { get; set; }

        public ExchangeRateResponse() { }

        public override string ToString()
        {
            string s_rates = "";
            for (int i = 0;
                i < rates.Count;
                i++
                )
            {
                s_rates += $"{rates.ElementAt(i).Key}: {rates.ElementAt(i).Value}\n";
            }
            return $"Base: {@base}, Time: {time}, Rates: \n" + s_rates;
        }
    }


}
