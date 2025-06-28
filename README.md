# Dynamic Configuration Project

## Proje Amacı

Bu proje, dinamik konfigürasyon yönetimi sağlar.  
. NET 8 tabanlı, MSSQL veritabanı kullanan ve RabbitMQ ile mesajlaşma entegrasyonu yapılmıştır.

## Özellikler

- MSSQL üzerinde konfigürasyon kayıtları tutulur.  
- `ConfigurationReader` DLL’i ile konfigürasyonlar cache’den okunur.  
- Config değişikliklerinde RabbitMQ üzerinden mesajlaşma ile anlık güncelleme yapılır.  
- API katmanında CRUD operasyonları mevcuttur.  
- Eşzamanlılık için SemaphoreSlim ile concurrency kontrol sağlanmıştır.

## Kullanılan Teknolojiler

- .NET 8  
- MSSQL Server  
- RabbitMQ (Docker container ile)  
- Docker (projeyi containerize etmek için)  
- Entity Framework Core  
- Swagger (API testi için)

## Çalıştırma

1. MSSQL veritabanını oluşturun ve `appsettings.json` içinde connection string’i güncelleyin.  
2. RabbitMQ Docker container’ını başlatın (örn: `docker run -d --hostname my-rabbit --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management`)  
3. Projeyi Visual Studio’dan derleyip çalıştırın.  
4. API için Swagger arayüzünden endpointleri test edin.

## Proje Yapısı

- `ConfigurationLibrary`: Konfigürasyon DLL’i  
- `ConfigWebApi`: API katmanı  
- `ConfigReaderTestApp`: Konsol uygulaması ile testler

## Geliştirme Notları

- Konfigürasyonlar belirli aralıklarla yenilenir ve cache güncellenir.  
- RabbitMQ mesajları ile ilgili servisin cache’i anlık yenilenir.  
- Unit testler ve TDD uygulanamadı (zaman yetersizliği nedeniyle).  
- Docker-compose entegrasyonu yapılmadı.

---

**İletişim:** karaermustafabirkan@gmail.com  
