using DapperLojistik.Data;
using DapperLojistik.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DapperLojistik.Pages
{
    public class OperasyonModel : PageModel
    {
        private readonly IKargoRepository _repo;

        [BindProperty]
        public KargoGonderisi Kargo { get; set; } = new();

        public KargoGonderisi? DuzenlenecekKargo { get; set; }

        public static readonly string[] Sehirler = [
            "Ýstanbul","Ankara","Ýzmir","Bursa","Antalya","Adana","Konya",
            "Gaziantep","Mersin","Kayseri","Eskiþehir","Trabzon","Diyarbakýr",
            "Samsun","Malatya","Erzurum","Kocaeli","Denizli","Þanlýurfa","Mardin"
        ];

        public static readonly string[] Kategoriler = [
            "Elektronik","Giyim","Mobilya","Gýda","Otomotiv","Kozmetik",
            "Kitap","Oyuncak","Spor","Ev Aletleri","Taký","Ýlaç"
        ];

        public static readonly string[] Durumlar = [
            "Beklemede","Depoda","Yolda","Teslim Edildi","Ýade","Hasar Gördü"
        ];

        public static readonly string[] AracTipleri = [
            "Kamyon","Minivan","Motokurye","TIR","Uçak Kargo"
        ];

        public static readonly string[] OdemeYontemleri = [
            "Kapýda Ödeme","Kredi Kartý","Havale/EFT","Online Ödeme"
        ];

        public static readonly string[] Firmalar = [
            "DapperKargo A.Þ.","HýzlýTaþýt Ltd.","AnadoluLojistik",
            "KaradenizTaþýmacýlýk","EgeLojistik"
        ];

        public OperasyonModel(IKargoRepository repo)
        {
            _repo = repo;
        }

        public async Task OnGetAsync(int? id)
        {
            if (id.HasValue)
                DuzenlenecekKargo = await _repo.GetByIdAsync(id.Value);
        }

        public async Task<IActionResult> OnPostEkleAsync()
        {
            Kargo.TakipNo = "DPR" + Random.Shared.Next(10000000, 99999999);
            Kargo.GonderiTarihi = DateTime.Now;
            Kargo.TahminiTeslimTarihi = DateTime.Now.AddDays(3);

            await _repo.AddAsync(Kargo);
            TempData["Mesaj"] = $"? Kargo eklendi! Takip No: {Kargo.TakipNo}";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostGuncelleAsync()
        {
            await _repo.UpdateAsync(Kargo);
            TempData["Mesaj"] = "? Kargo baþarýyla güncellendi!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSilAsync(int id)
        {
            await _repo.DeleteAsync(id);
            TempData["Mesaj"] = "??? Kargo silindi.";
            return RedirectToPage();
        }
    }
}