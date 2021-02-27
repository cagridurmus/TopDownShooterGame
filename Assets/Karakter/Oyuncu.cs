using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oyuncu : MonoBehaviour
{
    public float hiz;
    Animator anim1; // animator u tanimladik
    bool c; // comelme
    bool p; //surunme
    public Camera cam;
    public float can, zirh;//can ve zirh degerleri
    Silah silahKod; // silah i olusturduk
    public Text ArmorText, HealthText;
    public int pozisyonFark;
    bool reload;
    bool ee = false; // e ya basip basmadigimizi kontrol ediyoruz
    bool ff = false; // f ya basip basmadigimizi kontrol ediyoruz
    public GameObject SilahNesnesi;
    public bool silahEldeMi; // Silah update fonksiyonu kontrol etmek icin gerekli degisken -up
    CharacterController cc;
    public float sagsol, sagsolhiz;// kamera icin sagsol yaparken degerler
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        hiz = 5;
        
        c = false;
        p = false;
        anim1 = GetComponent<Animator>();
        silahKod = GameObject.Find("Silah").GetComponent<Silah>();  //silah sinifindan silah olusturduk
        cc = GetComponent<CharacterController>();//karaktercontrolleru cagirdik
        
    }
    void DurbunEtkisi() // bu fonksiyonda hangi slotdaysak o slot da ki silahin hangisi oldugunu anliyarak o silahin durbunpayina gore kameranin acisini degisitiyoruz
    {
        if (silahKod.a == true) // burda silah olmama durumu olmadigi icin
        {
            cam.orthographicSize = silahKod.slot1DurbunPayi;
        }
        else if(silahKod.b==true && silahKod.slot2.transform.childCount != 0) // slot 2 de silah olmadigi icin slot 2 de silahin olup olmama durumunu kontrol etmemiz lazim
        {
            cam.orthographicSize = silahKod.slot2DurbunPayi;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ArmorText.text = zirh.ToString();
        HealthText.text = can.ToString();
        reload = silahKod.reload;

        Comelme();
        Surunme();
        Rotasyon();
        DurbunEtkisi();
        
        
    }
    void Rotasyon() // karakterin fare ile rotasyon kismi
    {
        sagsol = Input.GetAxis("Mouse X") * sagsolhiz; // fare mizin X den gelen degeri ile faremizin sagsol yaparken cevirme hizini alip carpiyoruz
        transform.Rotate(0, sagsol, 0);//Y ekseni uzerinden sagsol olarak rotate liyoruz.
    }


    void Comelme()// Comelme yaptigimiz kisim
    {
        if (Input.GetKeyDown(KeyCode.C))// c tusuna bastiginda 
        {
            pozisyonFark = 1;
            if (c == false) // comelmemis isem
            {
                hiz = 2; // Comelme durumundayken hizin degerini 1 yapiyor.
                c = true;
                p = false;
                anim1.SetBool("comelme", true);// c tusuna basildigin comeldi
                anim1.SetBool("yatma", false);// comelmeden surunmeye gecmek icin
                anim1.SetBool("ayakta", false);
            }
            else //comelmis isem
            {
                c = false;
                anim1.SetBool("comelme", false);//geri ayaga kalkildi
                hiz = 3;
                anim1.SetBool("ayakta", true);
                pozisyonFark = 0;

            }
        }
    }

    void Surunme() //Surunme yaptigimiz kisim
    {
        if (Input.GetKeyDown(KeyCode.Z))// z tusuna basildiginda
        {
            pozisyonFark = 3;
            if (p == false)// surunmemis durumda isem
            {
                hiz = 0.33f; // Surunme durumundayken hizin degerini 0.5 yapiyor.
                p = true;
                c = false;
                anim1.SetBool("yatma", true);//z tusuna basildiginda surunuldu
                anim1.SetBool("comelme", false);// surunmeden yatmaya gecmek icin
                anim1.SetBool("ayakta", false);
            }
            else //surunme durumunda ise
            {
                p = false;
                anim1.SetBool("yatma", false);// z tusuna basildiginda ayaga kalkildi
                anim1.SetBool("ayakta", true);
                hiz = 3;
                pozisyonFark = 0;


            }
        }
    }
    // Fixed Update kisminda x ve y kismini alarak move fonksiyonuyla cagiriyoruz.
    void FixedUpdate() // hareket ettirmeye sagalayan fonksiyon
    {
        var xx = Input.GetAxis("Horizontal");//Dikeyde hareket etme
        var yy = Input.GetAxis("Vertical");//Yatayda hareket etme

        if (reload == false)//sarjor degistirmiyorsak  haraket edebiliriz
        {
            Move(xx, yy);
        }
        else 
        {
            if (anim1.GetCurrentAnimatorStateInfo(0).IsName("Reload-Walking")) //Yurumu animasyonunu yaparken sarjorumuzu degistirebilelim
            {
                Move(xx, yy);
            }
        }
        Etkilesim();
    }
    // Move kisminda karakterimizi ileri geri yaptiriyoruz
    void Move(float x,float y) //yurumemizi saglayan kisim
    {
        anim1.SetFloat("yatay", x/2);// x yonunde shifte basma
        anim1.SetFloat("dikey", y / 2);// y yonunde dogru shifte basma
        cc.SimpleMove(transform.forward * (hiz * y));//Karakter y ekseninde hareket ediyor
        cc.SimpleMove(transform.right * (hiz * x)); //Karakter x ekseninde hareket ediyor
    }
    void Etkilesim()
    {
        RaycastHit hit; // carptigimiz nesneyi kontrol edebilmek icin raycast olusturduk
        Sandik sandikKod; // Sandik sinifini olusturduk
        

        if (Physics.Raycast(transform.position, transform.forward,out hit,2.0f))// sandikla etkilesime gecebilmek icin isinlari kullanicagiz onun unityden fizik sinifini kullandim.
        {
            if (hit.collider.tag == "sandık")
            {
                GameObject s = hit.transform.gameObject;// carptigimiz noktadaki oyun nesnesi
                
                if (ee == false) // e ya basmazsak press f yazisi karsimiza hala cikmis oluyor
                {
                    s.GetComponentInChildren<TextMesh>().text = "Press F";// olusturdugumuz isinimiz sandiga carptiginda F e basili tutun yazisi cikacak.
                }
                
                if (Input.GetKeyDown(KeyCode.F)) //F e basildiginda kutu aciliyor ve isik yaniyor
                {
                    ff = true;
                    s.GetComponent<Animator>().enabled = true; 
                    s.GetComponentInChildren<Light>().enabled = true;
                    sandikKod = s.GetComponent<Sandik>(); //sandigin ozelliklerine ulasmak istiyoruz
                    sandikKod.sunumYap(); // sandik classinin sunum yap fonksiyonun cagiriyoruz
                }
                if (Input.GetKeyDown(KeyCode.E) && ff==true) // e ye bastigimizda ve f tusuna basilmissa bu kisim devreye giriyor
                {
                    ee = true;
                    int t = s.GetComponent<Sandik>().n; //sandiktan cektigimiz nesnelerin t sayisini index gibi kullaniyoruz
                    GameObject[] cekilenNesneler = s.GetComponent<Sandik>().nesneler;//sandiktan cekilen nesneleri karsilastirmak icin diziye atadik 
                    Animator amt = s.GetComponent<Animator>(); // olusturdugumuz sandik yok olma animasyonunu ekledik

                    if (cekilenNesneler[t].name == "can") //diziye atadigimiz t indexi ile birlikte isminin can olup olmadigini karsilastiriyoruz.eger ismi can ise karakterin canini 200 yapiyor
                    { 
                        can = 200;
                    }
                    if (cekilenNesneler[t].name == "Armor") //diziye atadigimiz t indexi ile birlikte isminin Armor olup olmadigini karsilastiriyoruz.eger ismi Armor ise karakterin zirhini 100 yapiyor
                    {
                        zirh = 100;
                    }
                    if (cekilenNesneler[t].name == "Ak-47")
                    {
                        SilahAlim(0, "Ak-47");
                    }
                    if (cekilenNesneler[t].name == "P-90")
                    {
                        SilahAlim(1, "P-90");
                    }
                    if (cekilenNesneler[t].name == "Shotgun")
                    {
                        SilahAlim(2, "Shotgun");
                    }
                    if (cekilenNesneler[t].name == "Sniper-Rifle")
                    {
                        SilahAlim(3, "Sniper-Rifle");
                    }

                    amt.SetBool("destroy", true);// //destroy animasyonu true olunca yok olucak
                    Destroy(s, 0.5f); // Sandiktaki nesneyi aldiktan sonra sandigi 0,5 saniye icinde yok eder
                }
                
            }
        }
    }
    void SılahGuncelleme(string cek) //Bu fonksiyonda sandiktan cikan silahin eldeki silahla ayni durumda olmasi halinde silah almiyor,farkliysa silahi aliyor.
    {
        if(silahKod.s1.text==cek || silahKod.s2.text == cek) 
        {
            silahEldeMi = true;
            if (silahKod.a == true) // eger slot 1 deysek ve elimizde sandiktan cikanla ayni silah varsa bu islemleri yapiyoruz
            {
                if (cek == "Ak-47") // sandiktan aldigimiz silah keles ise mermi atilmamis keles silahi almiyor oluyoruz
                {
                    silahKod.slot1Atilan = 0;
                    silahKod.slot1Toplam += 300;
                }
                if (cek == "P-90") // sandiktan aldigimiz p90 keles ise mermi atilmamis p90 silahi almiyor oluyoruz
                {
                    silahKod.slot1Atilan = 0;
                    silahKod.slot1Toplam += 150;
                }
                if (cek == "Shotgun") // sandiktan aldigimiz silah pompali ise mermi atilmamis pompali silahi almiyor oluyoruz
                {
                    silahKod.slot1Atilan = 0;
                    silahKod.slot1Toplam += 50;
                }
                if (cek == "Sniper-Rifle") // sandiktan aldigimiz silah sniper ise mermi atilmamis sniper silahi almiyor oluyoruz
                {
                    silahKod.slot1Atilan = 0;
                    silahKod.slot1Toplam += 30;
                }
            }
            else if (silahKod.b == true) // eger slot 2 deysek
            {
                if (cek == "Ak-47") // sandiktan aldigimiz silah keles ise mermi atilmamis keles silahi almiyor oluyoruz
                {
                    silahKod.slot2Atilan = 0;
                    silahKod.slot2Toplam += 300;
                }
                if (cek == "P-90") // sandiktan aldigimiz p90 keles ise mermi atilmamis p90 silahi almiyor oluyoruz
                {
                    silahKod.slot2Atilan = 0;
                    silahKod.slot2Toplam += 150;
                }
                if (cek == "Shotgun") // sandiktan aldigimiz silah pompali ise mermi atilmamis pompali silahi almiyor oluyoruz
                {
                    silahKod.slot2Atilan = 0;
                    silahKod.slot2Toplam += 50;
                }
                if (cek == "Sniper-Rifle") // sandiktan aldigimiz silah sniper ise mermi atilmamis sniper silahi almiyor oluyoruz
                {
                    silahKod.slot2Atilan = 0;
                    silahKod.slot2Toplam += 30;
                }
            }
        }
        else
        {
            
            silahEldeMi = false;
        }
    }
    void SilahAlim(int index,string cekilen) //Bu kisimda hangi silahi hangi slota almamiz gerektigini ogreniyoruz.
    {
        SılahGuncelleme(cekilen);
        if (silahEldeMi == false) // eger elimizde silah yoksa bu kismi yapiyoruz
        {
            if (silahKod.a == true)
            {
                if (silahKod.slot1.transform.childCount > 0)//slot 1 de silah varsa
                {
                    silahKod.slot1.transform.GetChild(0).transform.gameObject.SetActive(false); //slot 1 de ki silahin aktfligini kapatiyor
                    silahKod.slot1.transform.GetChild(0).transform.SetParent(SilahNesnesi.transform);//onu al silahlarin arasina atiyor
                    silahKod.silahlar[index].gameObject.SetActive(true);// sandiktan silahi index e ata ve aktif
                    silahKod.silahlar[index].transform.SetParent(silahKod.slot1.transform); //aktif ettigi silahi slot 1 e ata
                }
                else // elimizde silah yoksa
                {
                    silahKod.silahlar[index].gameObject.SetActive(true);
                    silahKod.silahlar[index].transform.SetParent(silahKod.slot1.transform);
                }
                silahKod.Arama();
                silahKod.Degerlendirme();
            }
            else if (silahKod.b == true)
            {
                if (silahKod.slot2.transform.childCount > 0)//slot 2 de silah varsa
                {
                    silahKod.slot2.transform.GetChild(0).transform.gameObject.SetActive(false); //slot 2 de ki silahin aktfligini kapatiyor
                    silahKod.slot2.transform.GetChild(0).transform.SetParent(SilahNesnesi.transform);//onu al silahlarin arasina atiyor
                    silahKod.silahlar[index].gameObject.SetActive(true);// sandiktan silahi index e ata ve aktif
                    silahKod.silahlar[index].transform.SetParent(silahKod.slot2.transform); //aktif ettigi silahi slot 2 e ata
                }
                else // elimizde silah yoksa
                {
                    silahKod.silahlar[index].gameObject.SetActive(true);
                    silahKod.silahlar[index].transform.SetParent(silahKod.slot2.transform);
                }
                silahKod.Arama();
                silahKod.Degerlendirme();
            }
        }
        
            
    }
}


