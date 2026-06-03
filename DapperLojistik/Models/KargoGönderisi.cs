namespace DapperLojistik.Models
{
    public class KargoGonderisi
    {
        public int Id { get; set; }
        public string TakipNo { get; set; } = string.Empty;
        public string GondericiAd { get; set; } = string.Empty;
        public string GondericiTelefon { get; set; } = string.Empty;
        public string GondericiSehir { get; set; } = string.Empty;
        public string AliciAd { get; set; } = string.Empty;
        public string AliciTelefon { get; set; } = string.Empty;
        public string AliciSehir { get; set; } = string.Empty;
        public string AliciAdres { get; set; } = string.Empty;
        public string UrunKategorisi { get; set; } = string.Empty;
        public decimal AgirlikKg { get; set; }
        public string KargoFirmasi { get; set; } = string.Empty;
        public string AracTipi { get; set; } = string.Empty;
        public DateTime GonderiTarihi { get; set; }
        public DateTime? TeslimTarihi { get; set; }
        public DateTime TahminiTeslimTarihi { get; set; }
        public string Durum { get; set; } = string.Empty;
        public decimal OdemeTutari { get; set; }
        public string OdemeYontemi { get; set; } = string.Empty;
        public string OdenmeMi { get; set; } = string.Empty;
        public string? Notlar { get; set; }
    }
}