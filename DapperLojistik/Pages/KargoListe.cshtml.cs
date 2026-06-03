using DapperLojistik.Data;
using DapperLojistik.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DapperLojistik.Pages
{
    public class KargoListeModel : PageModel
    {
        private readonly IKargoRepository _repo;

        public List<KargoGonderisi> Kargolar { get; set; } = new();
        public int ToplamKayit { get; set; }
        public int ToplamSayfa { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Sayfa { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public string? TakipNo { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? GondericiSehir { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? AliciSehir { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Durum { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? KargoFirmasi { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? UrunKategorisi { get; set; }

        public static readonly string[] Sehirler = [
            "Ýstanbul","Ankara","Ýzmir","Bursa","Antalya","Adana","Konya",
            "Gaziantep","Mersin","Kayseri","Eskiþehir","Trabzon","Diyarbakýr",
            "Samsun","Malatya","Erzurum","Kocaeli","Denizli","Þanlýurfa","Mardin"
        ];

        public static readonly string[] Durumlar = [
            "Beklemede","Depoda","Yolda","Teslim Edildi","Ýade","Hasar Gördü"
        ];

        public static readonly string[] Kategoriler = [
            "Elektronik","Giyim","Mobilya","Gýda","Otomotiv","Kozmetik",
            "Kitap","Oyuncak","Spor","Ev Aletleri","Taký","Ýlaç"
        ];

        public static readonly string[] Firmalar = [
            "DapperKargo A.Þ.","HýzlýTaþýt Ltd.","AnadoluLojistik",
            "KaradenizTaþýmacýlýk","EgeLojistik"
        ];

        public KargoListeModel(IKargoRepository repo)
        {
            _repo = repo;
        }

        public async Task OnGetAsync()
        {
            if (Sayfa < 1) Sayfa = 1;

            var (veriler, toplam) = await _repo.GetPagedAsync(
                Sayfa, 12,
                TakipNo, GondericiSehir, AliciSehir,
                Durum, KargoFirmasi, UrunKategorisi);

            Kargolar = veriler.ToList();
            ToplamKayit = toplam;
            ToplamSayfa = (int)Math.Ceiling((double)toplam / 12);
        }

        public async Task<IActionResult> OnPostSilAsync(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}