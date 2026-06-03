using Dapper;
using DapperLojistik.Models;
using System.Data;

namespace DapperLojistik.Data
{
    public interface IKargoRepository
    {
        Task<IEnumerable<KargoGonderisi>> GetAllAsync();
        Task<KargoGonderisi?> GetByIdAsync(int id);
        Task<int> AddAsync(KargoGonderisi kargo);
        Task UpdateAsync(KargoGonderisi kargo);
        Task DeleteAsync(int id);
        Task<(IEnumerable<KargoGonderisi> Veriler, int ToplamKayit)> GetPagedAsync(
            int sayfa, int sayfaBoyutu,
            string? takipNo, string? gondericiSehir, string? aliciSehir,
            string? durum, string? kargoFirmasi, string? urunKategorisi);
    }

    public class KargoRepository : IKargoRepository
    {
        private readonly IDbConnectionFactory _factory;

        public KargoRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<KargoGonderisi>> GetAllAsync()
        {
            using var conn = _factory.CreateConnection();
            return await conn.QueryAsync<KargoGonderisi>("SELECT * FROM KargoGonderileri");
        }

        public async Task<KargoGonderisi?> GetByIdAsync(int id)
        {
            using var conn = _factory.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<KargoGonderisi>(
                "SELECT * FROM KargoGonderileri WHERE Id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(KargoGonderisi kargo)
        {
            using var conn = _factory.CreateConnection();
            var sql = @"INSERT INTO KargoGonderileri 
                (TakipNo, GondericiAd, GondericiTelefon, GondericiSehir,
                 AliciAd, AliciTelefon, AliciSehir, AliciAdres,
                 UrunKategorisi, AgirlikKg, KargoFirmasi, AracTipi,
                 GonderiTarihi, TahminiTeslimTarihi, Durum,
                 OdemeTutari, OdemeYontemi, OdenmeMi, Notlar)
                VALUES
                (@TakipNo, @GondericiAd, @GondericiTelefon, @GondericiSehir,
                 @AliciAd, @AliciTelefon, @AliciSehir, @AliciAdres,
                 @UrunKategorisi, @AgirlikKg, @KargoFirmasi, @AracTipi,
                 @GonderiTarihi, @TahminiTeslimTarihi, @Durum,
                 @OdemeTutari, @OdemeYontemi, @OdenmeMi, @Notlar);
                SELECT SCOPE_IDENTITY();";
            return await conn.ExecuteScalarAsync<int>(sql, kargo);
        }

        public async Task UpdateAsync(KargoGonderisi kargo)
        {
            using var conn = _factory.CreateConnection();
            var sql = @"UPDATE KargoGonderileri SET
        AliciAd          = @AliciAd,
        AliciTelefon     = @AliciTelefon,
        AliciSehir       = @AliciSehir,
        AliciAdres       = ISNULL(@AliciAdres, ''),
        Durum            = @Durum,
        TeslimTarihi     = @TeslimTarihi,
        OdemeTutari      = @OdemeTutari,
        OdemeYontemi     = @OdemeYontemi,
        OdenmeMi         = @OdenmeMi,
        Notlar           = @Notlar
        WHERE Id = @Id";
            await conn.ExecuteAsync(sql, kargo);
        }

        public async Task DeleteAsync(int id)
        {
            using var conn = _factory.CreateConnection();
            await conn.ExecuteAsync(
                "DELETE FROM KargoGonderileri WHERE Id = @Id", new { Id = id });
        }

        public async Task<(IEnumerable<KargoGonderisi> Veriler, int ToplamKayit)> GetPagedAsync(
            int sayfa, int sayfaBoyutu,
            string? takipNo, string? gondericiSehir, string? aliciSehir,
            string? durum, string? kargoFirmasi, string? urunKategorisi)
        {
            using var conn = _factory.CreateConnection();

            var where = "WHERE 1=1";
            var param = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(takipNo))
            { where += " AND TakipNo LIKE @TakipNo"; param.Add("TakipNo", $"%{takipNo}%"); }

            if (!string.IsNullOrWhiteSpace(gondericiSehir))
            { where += " AND GondericiSehir = @GondericiSehir"; param.Add("GondericiSehir", gondericiSehir); }

            if (!string.IsNullOrWhiteSpace(aliciSehir))
            { where += " AND AliciSehir = @AliciSehir"; param.Add("AliciSehir", aliciSehir); }

            if (!string.IsNullOrWhiteSpace(durum))
            { where += " AND Durum = @Durum"; param.Add("Durum", durum); }

            if (!string.IsNullOrWhiteSpace(kargoFirmasi))
            { where += " AND KargoFirmasi = @KargoFirmasi"; param.Add("KargoFirmasi", kargoFirmasi); }

            if (!string.IsNullOrWhiteSpace(urunKategorisi))
            { where += " AND UrunKategorisi = @UrunKategorisi"; param.Add("UrunKategorisi", urunKategorisi); }

            var countSql = $"SELECT COUNT(*) FROM KargoGonderileri {where}";
            var toplam = await conn.ExecuteScalarAsync<int>(countSql, param);

            var offset = (sayfa - 1) * sayfaBoyutu;
            param.Add("Offset", offset);
            param.Add("SayfaBoyutu", sayfaBoyutu);

            var dataSql = $@"SELECT * FROM KargoGonderileri {where}
                ORDER BY GonderiTarihi DESC
                OFFSET @Offset ROWS FETCH NEXT @SayfaBoyutu ROWS ONLY";

            var veriler = await conn.QueryAsync<KargoGonderisi>(dataSql, param);

            return (veriler, toplam);
        }
    }
}