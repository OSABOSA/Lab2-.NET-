using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace Lab2_GUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

     
    }
    //public class ExchangeRateResponse
    //{
    //    public string @base { get; set; }
    //    public DateTime time { get; set; }
    //    public System.Collections.Generic.Dictionary<string, decimal> rates { get; set; }
    //}

}