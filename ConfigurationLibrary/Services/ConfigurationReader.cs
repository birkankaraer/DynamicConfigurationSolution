using ConfigurationLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Timers;

namespace ConfigurationLibrary.Services
{
    public class ConfigurationReader
    {
        private readonly string _applicationName;
        private readonly string _connectionString;
        private readonly int _refreshInterval;
        private readonly ConcurrentDictionary<string, ConfigurationRecord> _configCache;
        private readonly System.Timers.Timer _refreshTimer;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalInMs)
        {
            _applicationName = applicationName;
            _connectionString = connectionString;
            _refreshInterval = refreshTimerIntervalInMs;
            _configCache = new ConcurrentDictionary<string, ConfigurationRecord>();

            // İlk yükleme - async metodu çağırmak için GetAwaiter().GetResult() kullanalım
            LoadConfigurationsAsync().GetAwaiter().GetResult();

            _refreshTimer = new System.Timers.Timer(_refreshInterval);
            _refreshTimer.Elapsed += async (sender, e) => await LoadConfigurationsAsync();
            _refreshTimer.AutoReset = true;
            _refreshTimer.Start();
        }

        private async Task LoadConfigurationsAsync()
        {
            if (!await _semaphore.WaitAsync(0))
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Güncelleme atlandı: Zaten devam ediyor.");
                return;
            }

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Güncelleme başladı.");

            try
            {
                var options = new DbContextOptionsBuilder<ConfigurationDbContext>()
                    .UseSqlServer(_connectionString)
                    .Options;

                await using var context = new ConfigurationDbContext(options);

                // Simüle etmek için biraz gecikme ekleyelim
                await Task.Delay(3000); // 3 saniye

                var records = await context.ConfigurationRecords
                    .Where(r => r.IsActive && r.ApplicationName == _applicationName)
                    .ToListAsync();

                foreach (var record in records)
                {
                    _configCache[record.Name] = record;
                }

                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Güncelleme tamamlandı.");
            }
            catch
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Güncelleme sırasında hata oluştu.");
            }
            finally
            {
                _semaphore.Release();
            }
        }



        public T GetValue<T>(string key)
        {
            if (_configCache.TryGetValue(key, out var record))
            {
                try
                {
                    return (T)Convert.ChangeType(record.Value, typeof(T));
                }
                catch
                {
                    throw new InvalidCastException($"Key '{key}' değeri '{record.Value}' tipine dönüştürülemedi.");
                }
            }

            throw new KeyNotFoundException($"Key '{key}' bulunamadı.");
        }
    }
}
