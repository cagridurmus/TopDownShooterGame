using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Silah : MonoBehaviour
{

    public GameObject[] silahlar; //Silahlarimizi olusturduk
    public string eldekiSilahAdi;//silah adi

    public Transform namluucu; //nerden ateslendigini degiskene aticaz
    public float zaman; // zaman
    bool silahVarmi; //Silahimizin olup olmadigini kontrol etmek icin variable
    AudioSource sesKaynagi; //Ses kaynagi olusturduk
    public AudioClip[] silahSesleri;
    public AudioClip sarjorSesi;
    public GameObject muzzlePrefab; // ates ederken gorunen namluda ki flas
    public Material[] mats;
    
    bool atesEttiMi;
    public bool reload; //reload yapilma durumunu kontrol etme
    public GameObject mermi;
    public int pozisyonFark;
    public float sekmeDeger;
    public Text MermiBilgiText, MermiBilgiText2;
    Oyuncu karakterKod;
    public bool a, b; // Silah degisimlerini kontrol etmek icin
    public GameObject ok1, ok2; // ok1 ve ok2 
    public Text s1, s2; // Isimlerin yazili oldugu textleri erisebilmek icin
    Animator anim1; // animator u tanimladik
    GameObject karakter;
    //--------------------------------ENVANTER-------------------------------------------------

    public GameObject slot1, slot2; // Slot 1 ve slot kisacasi silahlari koycagimiz yerler
     
    public class Ak47
    {
        public float fireRate = 0.2f;// her silahin  kendine ozel sekme sayisi var
        public float toplamMermi = 300;// her silahin  kendine ozel toplam mermi sayisi var
        public float atilanMermi;
        public float sarjorBasinaMermi = 30;// her silahin  kendine ozel sarjorunde mermi sayisi var
        public float menzil = 6000.0f;// her silahin  kendine ozel menzili var 
        public float hasar = 10.0f; // her silahin  kendine ozel hasari var 
        public float minSekme = -5.0f; // her silahin kendine ozel minimum sekmasi var  
        public float makSekme = 5.0f; // her silahin kendine ozel maksimum sekmasi var  
        public int s = 0; // her silaha gore belirledigimiz ses dosyasinin index degeri
        public int m = 0; // ates ederken gorunen namluda ki flas in index degeri
        public bool dd = false; // Silah gecisleri icin degerlendirme yapiyor
        public int durbunPayi = 15; //her silahin farkli menzili oldugu icin o silahin menziline gore kamerayi ayarlamaya yariyacal
    }
    public class P90
    {
        public float fireRate = 0.1f;
        public float toplamMermi = 150;
        public float atilanMermi;
        public float sarjorBasinaMermi = 45;
        public float menzil = 3000.0f;
        public float hasar = 5.0f;
        public float minSekme = -3.0f;
        public float makSekme = 3.0f;
        public int s = 1;
        public int m = 1;
        public bool dd = false;
        public int durbunPayi = 10;
    }
    public class Shotgun
    {
        public float fireRate = 0.33f;
        public float toplamMermi = 50;
        public float atilanMermi;
        public float sarjorBasinaMermi = 8;
        public float menzil = 1500.0f;
        public float hasar = 15.0f;
        public float minSekme = -4.0f;
        public float makSekme = 4.0f;
        public int s = 2;
        public int m = 2;
        public bool dd = false;
        public int durbunPayi = 10;
    }
    public class SniperRifle
    {
        public float fireRate = 1.0f;
        public float toplamMermi = 30;
        public float atilanMermi;
        public float sarjorBasinaMermi = 5;
        public float menzil = 10000.0f;
        public float hasar = 50.0f;
        public float minSekme = -6.0f;
        public float makSekme = 6.0f;
        public int s = 3;
        public int m = 3;
        public bool dd = false;
        public int durbunPayi = 25;

    }

    public Ak47 keles = new Ak47();
    public P90 p90 = new P90();
    public Shotgun pompali = new Shotgun();
    public SniperRifle sniper = new SniperRifle();
    public float fireRate, sarjorBasinaMermi, menzil, hasar, minSekme, makSekme;
    public int s, m;
    public float slot1Atilan,slot1Toplam,slot2Atilan,slot2Toplam,slot1DurbunPayi, slot2DurbunPayi;
    public bool benden;
    //--------------------------------ENVANTER-------------------------------------------------
    // Start is called before the first frame update
    void Start()
   {
        //--------------------------------------------------------------------
        foreach (GameObject item in silahlar)
        {
            item.SetActive(false); //butun silahlari basta aktiflikten cikariyoruz
        }
        //--------------------------------------------------------------------
        slot1.SetActive(false);
        slot2.SetActive(false);
        
        int random = Random.Range (0, silahlar.Length-1);// Oyunun basinda random verecegi silahin sayi degeri
        silahlar[random].transform.SetParent(slot1.transform); // Sectigimiz silahi slot 1 e atma islemi
        silahlar[random].SetActive(true); // ilk verdigi silahi aktif etmek

        sesKaynagi = GetComponent<AudioSource>();//Ses kaynagimiza erismek icin
        anim1 = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        silahVarmi = false;
        atesEttiMi = false;
        reload = false;

        ok1.SetActive(false); //ok1 i oyunun basinda kapatiyoruz.
        ok2.SetActive(false); //ok2 i oyunun basinda kapatiyoruz.

        a = false;
        b = false;

        karakterKod = GameObject.FindGameObjectWithTag("Player").GetComponent<Oyuncu>();
        karakter = GameObject.FindGameObjectWithTag("Player");
   }

    // Update is called once per frame
    void Update()
    {
        SilahDegis();

        pozisyonFark = karakterKod.pozisyonFark;
        zaman += Time.deltaTime;//Gececek sureyi kontrol etcek

        if (Input.GetMouseButtonDown(0)  && silahVarmi == true && reload == false)
        //mouse un sol tusu basili mi ve atilan mermi sarjordeki mermiye esit degilse ve silah eldeyse ve reload yapilmiyorsa ates ediliyor
        {
            if(a==true && slot1Atilan != sarjorBasinaMermi && slot1Toplam != 0)
            {
                atesEttiMi = true; // ates ediyor
                reload = false;// ates ettigi icin reload yapamaz
                AtesEtme();
            }
            if (b == true && slot2Atilan != sarjorBasinaMermi && slot2Toplam != 0)
            {
                atesEttiMi = true; // ates ediyor
                reload = false;// ates ettigi icin reload yapamaz
                AtesEtme();
            }

        }
        if (Input.GetMouseButtonUp(0)) // ates edip etmedigimizi kontrol ediyoruz
        {
            atesEttiMi = false;
        }
        if (Input.GetKeyUp(KeyCode.R) && silahVarmi == true) // sarjor degistiriyormuyuz ve elimizde silah varmi 
        {
            reload = true;
            atesEttiMi = false; // sarjor degistirdigi icin ates edemez
            StartCoroutine("Reload");
        }
        if (a == true)
        {
            MermiBilgiText.text = (sarjorBasinaMermi - slot1Atilan + "/ " + slot1Toplam.ToString());
        }
        else if (b == true && slot2.transform.childCount!=0) // slot 2 de silah varsa
        {
            MermiBilgiText2.text = (sarjorBasinaMermi - slot2Atilan + "/ " + slot2Toplam.ToString());
        }
    }
    
    public void Arama() //Silahi verdigimiz adiyla cagirirken arama kismini yapan fonksiyon
    {
        if (a==true && slot1.transform.childCount>0)// slot 1 de silah varsa o silahi eldeki silaha adi yap
        {
            eldekiSilahAdi = slot1.transform.GetChild(0).gameObject.name;
            s1.text = eldekiSilahAdi;
        } 
        else if(b==true && slot2.transform.childCount > 0)// slot 2 de silah varsa o silahi eldeki silaha adi yap
        { 
            eldekiSilahAdi = slot2.transform.GetChild(0).gameObject.name;
            s2.text = eldekiSilahAdi;
        }
    }
    public void Degerlendirme()
    {
        if (eldekiSilahAdi == "")
        {
            silahVarmi = false;
        }
        if (eldekiSilahAdi == "Ak-47")
        {
            silahVarmi = true;

            fireRate = keles.fireRate;
            if (keles.dd == false)  // elimizdeki silahi degistirdigimiz slotlardaki mermilerin degisimini yapmak icin kullaniyoruz
            {
                if (a == true) // 1 e bastigimizda elimize keles geldiyse slot1'nin mermisini keles mermisine esitliyoruz
                {
                    slot1Toplam = keles.toplamMermi;
                    slot1DurbunPayi = keles.durbunPayi;
                }
                else if (b == true) // 2 e bastigimizda elimize keles geldiyse slot2'nin mermisini keles mermisine esitliyoruz
                {
                    slot2Toplam = keles.toplamMermi;
                    slot2DurbunPayi = keles.durbunPayi;
                }
                keles.dd = true;
            }
            
            sarjorBasinaMermi = keles.sarjorBasinaMermi;
            menzil = keles.menzil;
            hasar = keles.hasar;
            minSekme = keles.minSekme;
            makSekme = keles.makSekme;
            s = keles.s;
            m = keles.m;
        }
        if (eldekiSilahAdi == "P-90")
        {
            
            silahVarmi = true;

            fireRate = p90.fireRate;
            if (p90.dd == false)  // elimizdeki silahi degistirdigimiz slotlardaki mermilerin degisimini yapmak icin kullaniyoruz
            {
                if (a == true) // 1 e bastigimizda elimize p90 geldiyse slot1'nin mermisini p90 mermisine esitliyoruz
                {
                    slot1Toplam = p90.toplamMermi;
                    slot1DurbunPayi = p90.durbunPayi;
                }
                else if (b == true) // 2 e bastigimizda elimize p90 geldiyse slot2'nin mermisini p90 mermisine esitliyoruz
                {
                    slot2Toplam = p90.toplamMermi;
                    slot2DurbunPayi = p90.durbunPayi;
                }
                p90.dd = true;
            }
            sarjorBasinaMermi = p90.sarjorBasinaMermi;
            menzil = p90.menzil;
            hasar = p90.hasar;
            minSekme = p90.minSekme;
            makSekme = p90.makSekme;
            s = p90.s;
            m = p90.m;
        }
        if (eldekiSilahAdi == "Shotgun")
        {
            
            silahVarmi = true;

            fireRate = pompali.fireRate;
            if (pompali.dd == false)  // elimizdeki silahi degistirdigimiz slotlardaki mermilerin degisimini yapmak icin kullaniyoruz
            {
                if (a == true) // 1 e bastigimizda elimize pompali geldiyse slot1'nin mermisini pompali mermisine esitliyoruz
                {
                    slot1Toplam = pompali.toplamMermi;
                    slot1DurbunPayi = pompali.durbunPayi;
                }
                else if (b == true) // 2 e bastigimizda elimize pompali geldiyse slot2'nin mermisini pompali mermisine esitliyoruz
                {
                    slot2Toplam = pompali.toplamMermi;
                    slot2DurbunPayi = pompali.durbunPayi;
                }
                pompali.dd = true;
            }
            sarjorBasinaMermi = pompali.sarjorBasinaMermi;
            menzil = pompali.menzil;
            hasar = pompali.hasar;
            minSekme = pompali.minSekme;
            makSekme = pompali.makSekme;
            s = pompali.s;
            m = pompali.m;
        }
        if (eldekiSilahAdi == "Sniper-Rifle")
        {
            
            silahVarmi = true;

            fireRate = sniper.fireRate;
            if (sniper.dd == false)  // elimizdeki silahi degistirdigimiz slotlardaki mermilerin degisimini yapmak icin kullaniyoruz
            {
                if (a == true) // 1 e bastigimizda elimize sniper geldiyse slot1'nin mermisini sniper mermisine esitliyoruz
                {
                    slot1Toplam = sniper.toplamMermi;
                    slot1DurbunPayi = sniper.durbunPayi;
                }
                else if (b == true) // 2 e bastigimizda elimize sniper geldiyse slot2'nin mermisini sniper mermisine esitliyoruz
                {
                    slot2Toplam = sniper.toplamMermi;
                    slot2DurbunPayi = sniper.durbunPayi;
                }
                sniper.dd = true;
            }
            sarjorBasinaMermi = sniper.sarjorBasinaMermi;
            menzil = sniper.menzil;
            hasar = sniper.hasar;
            minSekme = sniper.minSekme;
            makSekme = sniper.makSekme;
            s = sniper.s;
            m = sniper.m;
        }

    }
    void SlotAtes()
    {
        if (a = true && slot1.transform.childCount>0) // 1.slottaysak ve elimizde silah varsa  onun mermisini harciyoruz
        {
            slot1Atilan++;
            /*sesKaynagi.PlayOneShot(silahSesleri[s]);
            MuzzleEfekt();
            GameObject g = Instantiate(mermi, namluucu.transform.position, Quaternion.identity); //Merminin namlu ucundan cikacagini belirterek mermiyi olusturuyoruz
            g.GetComponent<Bullet>().hasar = hasar; // olusturdugumuz mermiye ne kadar hasar verecegini tanimliyoruz
            Rigidbody rb = g.GetComponent<Rigidbody>();// Yercekimini olusturduk
            rb.velocity = karakter.transform.forward * menzil * Time.deltaTime; //Mermiyi ileriye dogru goturme
            rb.AddForce(Vector3.right * sekmeDeger * Time.deltaTime, ForceMode.Acceleration);//Silahin saga sola sekmesi
            rb.AddForce(Vector3.up * sekmeDeger * Time.deltaTime, ForceMode.Acceleration);// Silahin yukari asagi sekmesi
            Destroy(g, 3.0f);// mermiyi ortadan kaldirma*/
            Atesleme();
        }
        else if (b == true && slot2.transform.childCount > 0) // 2.slottaysak ve elimizde silah varsa  onun mermisini harciyoruz
        {
            slot2Atilan++;
            /*sesKaynagi.PlayOneShot(silahSesleri[s]);
            MuzzleEfekt();
            GameObject g = Instantiate(mermi, namluucu.transform.position, Quaternion.identity); //Merminin namlu ucundan cikacagini belirterek mermiyi olusturuyoruz
            g.GetComponent<Bullet>().hasar = hasar; // olusturdugumuz mermiye ne kadar hasar verecegini tanimliyoruz
            Rigidbody rb = g.GetComponent<Rigidbody>();// Yercekimini olusturduk
            rb.velocity = karakter.transform.forward * menzil * Time.deltaTime; //Mermiyi ileriye dogru goturme
            rb.AddForce(Vector3.right * sekmeDeger * Time.deltaTime, ForceMode.Acceleration);//Silahin saga sola sekmesi
            rb.AddForce(Vector3.up * sekmeDeger * Time.deltaTime, ForceMode.Acceleration);// Silahin yukari asagi sekmesi
            Destroy(g, 3.0f);// mermiyi ortadan kaldirma*/
            Atesleme();
        }
    }
    void SlotSarjor()
    {
        if (a == true && slot1.transform.childCount > 0) // 1.slottaysak ve elimizde silah varsa  onun sarjorunu degistiriyoruz
        {
            slot1Toplam -= slot1Atilan;
            slot1Atilan = 0;
        }
        else if (b == true && slot2.transform.childCount > 0) // 2.slottaysak ve elimizde silah varsa  onun sarjorunu degistiriyoruz
        {
            slot2Toplam -= slot2Atilan;
            slot2Atilan = 0;
        }
    }
    IEnumerator Reload() //Bu kisim silahimizin sarjorunu degistirdigimiz fonksiyon
    {

        if (reload == true) // sarjor degisiriliyor mu?
        {

            SlotSarjor();

            sesKaynagi.PlayOneShot(sarjorSesi);//sarjor sesimizi ses kaynagina gosteriyoruz
            anim1.SetBool("reload", true); //animasyonumuza sarjor degistirmenin yapildigini atiyoruz
        }
        yield return new WaitForSeconds(3.0f);
        anim1.SetBool("reload", false); // animasyonumuza sarjor degistirmenin yapilmadigini atiyoruz
        reload = false;
    }
    void MuzzleEfekt()
    {
        GameObject g = Instantiate(muzzlePrefab, namluucu.transform);// namlu ucunun  yerini olusturuyoruz
        g.transform.SetParent(namluucu.transform);// namlu ucuna bagli kalmasini sagliyoruz
        g.GetComponent<ParticleSystem>().GetComponent<Renderer>().material = mats[m];// yukledigimiz namlu flaslarinin oldugu materyallere erismek icin 
        Destroy(g, 0.20f);// atesi bitirdigimiz anda namlu ucundan gelen isigi bitirmek icin
    }
    void Atesleme()
    {
        sesKaynagi.PlayOneShot(silahSesleri[s]);
        MuzzleEfekt();
        GameObject g = Instantiate(mermi, namluucu.transform.position, Quaternion.identity); //Merminin namlu ucundan cikacagini belirterek mermiyi olusturuyoruz
        g.GetComponent<Bullet>().hasar = hasar; // olusturdugumuz mermiye ne kadar hasar verecegini tanimliyoruz
        g.GetComponent<Bullet>().ben = true;
        Rigidbody rb = g.GetComponent<Rigidbody>();// Yercekimini olusturduk
        rb.velocity = karakter.transform.forward * menzil * Time.deltaTime; //Mermiyi ileriye dogru goturme
        rb.AddForce(Vector3.right * sekmeDeger * Time.deltaTime, ForceMode.Acceleration);//Silahin saga sola sekmesi
        rb.AddForce(Vector3.up * sekmeDeger * Time.deltaTime, ForceMode.Acceleration);// Silahin yukari asagi sekmesi
        Destroy(g, 3.0f);// mermiyi ortadan kaldirma
    }
    void AtesEtme()
    {
        if (zaman > fireRate) //zaman ates etme sıklıgindan buyukse ates edecek
        {
            if (atesEttiMi == true)
            {
                sekmeDeger = Random.Range(minSekme + pozisyonFark, makSekme - pozisyonFark); //Silahlarin sekme degeri olusturduk
                zaman = 0;

                SlotAtes();
            }
        }
        else
        {
            zaman += Time.deltaTime;
        }
    }

    void SilahDegis()
    {
       
        if (Input.GetKeyDown(KeyCode.Alpha1)) //Silah degistirmemiz icin 1'e basiyoruz ve 1 tusuyla ilk silahimizi alabiliyoruz
        {
            if (a == false)
            {

                a = true;
                b = false;
                
                ok1.SetActive(true); // 1 ci silagimizi sectigimiz icin ok1 aktif olcak
                ok2.SetActive(false);
                Arama();
                Degerlendirme();
                slot1.SetActive(true);
                slot2.SetActive(false);
            
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // 2.silahi aktif etmeye yariyor
        {
            if (b == false)
            {
                b = true;
                a = false;
                
                ok1.SetActive(false);
                ok2.SetActive(true); // 2 ci silagimizi sectigimiz icin ok2 aktif olcak
            
                Arama();
                Degerlendirme();
                slot1.SetActive(false);
                slot2.SetActive(true);
            
            }
        }
        

    }
}
