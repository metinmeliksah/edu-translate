Translate - Çoklu Dil Çeviri Windows Forms Uygulamasý

Açýklama
- Bu proje, Google Gemini (Generative Language) API benzeri bir uç nokta kullanarak çoklu dil arasý çeviri yapabilen basit bir Windows Forms uygulamasýdýr.
- Uygulama .NET Framework 4.7.2 hedeflemektedir.

Özellikler
- Türkçe arayüz.
- Kaynak dili seçme.
- Hedef dil seçimi.
- Gemini modeline istek gönderme (varsayýlan: gemini-2.5-flash / 2.0-exp endpoint olarak ayarlanabilir).
- API kimlik bilgisi uygulama yapýlandýrmasýndan okunur; formda gizlidir.

Güvenlik Notu
- App.config içinde saklanan API key veya tokenlarýn doðrudan depolanmasý güvenli deðildir. Üretimde bir sunucu tarafý proxy veya güvenli anahtar saklama (ör. Azure Key Vault, Google Secret Manager) kullanýn.

Kurulum ve Çalýþtýrma
1. Projeyi klonlayýn veya zip içeriðini çýkarýn.
2. Visual Studio 2019/2022 ile çözümü açýn (.NET Framework 4.7.2 yüklü olmalý).
3. `Translate/App.config` içinde `ApiCredential` anahtarýný ekleyin:
   - OAuth Bearer token (tercih): `Bearer <TOKEN>`
   - veya API key: `<KEY>` (bazý modellerde API key ile eriþim kýsýtlý olabilir)
4. Derleyin ve çalýþtýrýn.

App.config örneði
```xml
<configuration>
  <appSettings>
    <add key="ApiCredential" value="Bearer YOUR_OAUTH_TOKEN_HERE" />
  </appSettings>
</configuration>
```

Hata Ayýklama
- Eðer HTTP 404 alýyorsanýz:
  - Google Cloud Console'da Generative Language API etkin mi?
  - Kullanýlan model adý ve endpoint doðru mu?
  - API anahtarý doðru proje için mi oluþturuldu?
- OAuth token için `gcloud auth application-default print-access-token` ile test edebilirsiniz.

Geliþtirme
- Yanýt ayrýþtýrmayý iyileþtirmek için `Newtonsoft.Json` veya `System.Text.Json` ekleyebilirsiniz.
- API anahtarlarýný güvenli bir þekilde saklamak için gizli yönetim hizmetleri entegre edin.

Lisans
- Bu depo örnek amaçlýdýr, lisans belirtilmemiþtir.
