using System;
using ConfigurationLibrary.Services;
using Microsoft.Extensions.Configuration;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // appsettings.json'dan bağlantı dizesini oku
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = configuration.GetConnectionString("DefaultConnection");
        int refreshIntervalMs = 60000; // 1 dakika yenileme süresi

        // SERVICE-A için ConfigurationReader oluştur
        var configReaderA = new ConfigurationReader("SERVICE-A", connectionString, refreshIntervalMs);

        string siteName = configReaderA.GetValue<string>("SiteName");
        Console.WriteLine($"SiteName (SERVICE-A): {siteName}");

        int maxItemCount = configReaderA.GetValue<int>("MaxItemCount");
        Console.WriteLine($"MaxItemCount (SERVICE-A): {maxItemCount}");

        // SERVICE-B için ConfigurationReader oluştur
        var configReaderB = new ConfigurationReader("SERVICE-B", connectionString, refreshIntervalMs);

        bool isBasketEnabled = configReaderB.GetValue<bool>("IsBasketEnabled");
        Console.WriteLine($"IsBasketEnabled (SERVICE-B): {isBasketEnabled}");

        Console.ReadLine();
    }
}
