using Dapper;
using DapperLojistik.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DapperLojistik.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly IDbConnectionFactory _factory;

        public int ToplamGonderi { get; set; }
        public int TeslimEdilen { get; set; }
        public int Yolda { get; set; }
        public int Beklemede { get; set; }
        public decimal ToplamCiro { get; set; }
        public decimal OrtalamaUcret { get; set; }

        public List<string> AyLabels { get; set; } = new();
        public List<int> AyData { get; set; } = new();

        public List<string> DurumLabels { get; set; } = new();
        public List<int> DurumData { get; set; } = new();

        public List<string> SehirLabels { get; set; } = new();
        public List<int> SehirData { get; set; } = new();

        public DashboardModel(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task OnGetAsync()
        {
            using var conn = _factory.CreateConnection();

            // Özet istatistikler
            var ozet = await conn.QueryFirstAsync(@"
                SELECT
                    COUNT(*) AS ToplamGonderi,
                    SUM(CASE WHEN Durum = 'Teslim Edildi' THEN 1 ELSE 0 END) AS TeslimEdilen,
                    SUM(CASE WHEN Durum = 'Yolda' THEN 1 ELSE 0 END) AS Yolda,
                    SUM(CASE WHEN Durum = 'Beklemede' THEN 1 ELSE 0 END) AS Beklemede,
                    SUM(OdemeTutari) AS ToplamCiro,
                    AVG(OdemeTutari) AS OrtalamaUcret
                FROM KargoGonderileri");

            ToplamGonderi = (int)ozet.ToplamGonderi;
            TeslimEdilen = (int)ozet.TeslimEdilen;
            Yolda = (int)ozet.Yolda;
            Beklemede = (int)ozet.Beklemede;
            ToplamCiro = (decimal)ozet.ToplamCiro;
            OrtalamaUcret = (decimal)ozet.OrtalamaUcret;

            // Aylýk trend (son 12 ay)
            var aylik = await conn.QueryAsync(@"
                SELECT FORMAT(GonderiTarihi, 'yyyy-MM') AS Ay, COUNT(*) AS Sayi
                FROM KargoGonderileri
                WHERE GonderiTarihi >= DATEADD(MONTH, -12, GETDATE())
                GROUP BY FORMAT(GonderiTarihi, 'yyyy-MM')
                ORDER BY Ay");

            foreach (var a in aylik)
            {
                AyLabels.Add((string)a.Ay);
                AyData.Add((int)a.Sayi);
            }

            // Durum daðýlýmý
            var durumlar = await conn.QueryAsync(@"
                SELECT Durum, COUNT(*) AS Sayi
                FROM KargoGonderileri
                GROUP BY Durum");

            foreach (var d in durumlar)
            {
                DurumLabels.Add((string)d.Durum);
                DurumData.Add((int)d.Sayi);
            }

            // Top 5 þehir
            var sehirler = await conn.QueryAsync(@"
                SELECT TOP 5 GondericiSehir, COUNT(*) AS Sayi
                FROM KargoGonderileri
                GROUP BY GondericiSehir
                ORDER BY Sayi DESC");

            foreach (var s in sehirler)
            {
                SehirLabels.Add((string)s.GondericiSehir);
                SehirData.Add((int)s.Sayi);
            }
        }
    }
}