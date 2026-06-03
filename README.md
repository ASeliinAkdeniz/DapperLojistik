# 📦 DapperLojistik - Kurumsal Kargo Yönetim Paneli
DapperLojistik, gerçek bir kargo firmasının operasyonel ihtiyaçlarını karşılamak üzere tasarlanmış, **1 Milyon satırlık büyük veri (Big Data)** üzerinde yüksek performansla çalışan kurumsal bir iç yönetim paneli simülasyonudur. Projede veri tabanı performansını maksimumda tutmak amacıyla ORM olarak **Dapper** tercih edilmiştir.

---

## 🚀 Öne Çıkan Özellikler & Sayfalar

Proje, kargo operasyonlarının baştan sona yönetilebilmesi için 3 temel modülden oluşmaktadır:

* **📊 Dashboard (Yönetici Paneli):** Yöneticilerin anlık durum takibi yapabileceği kontrol merkezidir. Toplam gönderi, teslim edilen/yoldaki kargolar ve anlık ciro bilgisi tek bakışta görülür. *Chart.js* entegrasyonu ile aylık trendler ve durum dağılımları grafiksel olarak raporlanır.
* **🔍 Kargo Listesi (Yüksek Performanslı Filtreleme):** 1 Milyon kayıt arasından takip numarası, şehir, durum, firma ve kategoriye göre anlık filtreleme yapılır. Veri yoğunluğunu önlemek için sayfa başına 12 kayıt gösteren **Server-Side Pagination (Sayfalama)** mimarisi kurulmuştur. Tüm filtreleme ve sayfalama yükü SQL kanadında çözülür.
* **🛠️ Operasyon Merkezi (CRUD):** Yeni kargo kayıtlarının sisteme girildiği, mevcut kargoların durumlarının güncellendiği (yolda, teslim edildi vb.) ve kayıtların yönetildiği operasyonel alandır.

---

## 🛠️ Kullanılan Teknolojiler

Projenin mimarisinde kararlılık, hız ve modern arayüz standartları ön planda tutulmuştur:

| Katman | Teknoloji | Kullanım Amacı / Neden? |
| :--- | :--- | :--- |
| **Veritabanı** | MS SQL Server | 1M satır veriyi ilişkisel, güvenli ve indeksli şekilde saklamak. |
| **ORM** | Dapper | SQL sorgularını C# nesnelerine en düşük işlemci/bellek yüküyle (High Performance) dönüştürmek. |
| **Backend** | ASP.NET Core (.NET 8) | Güçlü web sunucusu altyapısı ve iş mantığının (Business Logic) kurgulanması. |
| **Sayfa Yapısı**| Razor Pages | HTML ve C# kodlarını daha sade ve sayfa odaklı bir yapıda birleştirmek. |
| **Arayüz** | Bootstrap 5 | Tamamen mobil uyumlu (Responsive), modern ve temiz bir kullanıcı deneyimi. |
| **Grafikler** | Chart.js | Dashboard üzerindeki finansal ve operasyonel verileri görselleştirmek. |
| **İkonlar** | Bootstrap Icons | Arayüzün görsel kalitesini artıran modern ikon setleri. |
| **Veri Üretimi**| Python & Claude AI | 1 Milyon satırlık, birbiriyle tutarlı ve gerçekçi büyük test verisi setinin üretilmesi. |

---

## 📐 Veritabanı ve Performans Yaklaşımı

1 Milyon satır gibi büyük veri setlerinde geleneksel yöntemler yavaşlamaya sebep olacağı için projede şu performans teknikleri uygulanmıştır:
* **Server-Side Processing:** Filtreleme (`WHERE`) ve sayfalama (`OFFSET-FETCH`) işlemleri tamamen SQL Server üzerinde çalıştırılarak web sunucusuna sadece ihtiyaç duyulan 12 satırlık verinin gelmesi sağlanmıştır.
* **Dapper Hafifliği:** Entity Framework gibi ağır kurumsal araçlar yerine, direkt ham SQL gücünü kullanan Dapper tercih edilerek milisaniyeler seviyesinde veri listeleme başarısı yakalanmıştır.
