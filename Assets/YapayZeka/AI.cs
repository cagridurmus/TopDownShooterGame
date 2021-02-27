using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Yapay zeka kutuphanesi
public class AI : MonoBehaviour
{
    NavMeshAgent agent; //Yapay zeka
    AudioSource sesKaynak; //Ses kaynagi 
    Animator anim;
    public AudioClip[] silahSesleri;
    public float fireRate, sarjorBasinaMermi, menzil, hasar, minSekme, makSekme,seviyeEtkisi, toplamMermi, atilanMermi,sekmeDegeri;
    public int s, m;
    public Material[] renkler; //renk materyalleri dizisi
    int random; //renk degiskenini atamak icin random renk sayisi
    public Material[] muzzle;
    public GameObject nu; // namluucu
    public GameObject mermiPrefab;
    public GameObject muzzlePrefab;
    public GameObject[] silahlar;
    int secim;
    public GameObject SilahNesnesi;
    public GameObject ic, dis;
    GameObject Player;
    public float mesafe;
    public float hiz;
    public bool fire;
    float zaman;
    public float can, zirh;
    public bool oldu, sarjor, intihar, patladi;
    public GameObject patlama;
    Islemler islem; //islemler sinifi
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player"); //player 
        islem = GameObject.Find("Islemler").GetComponent<Islemler>(); //islemler nesnenin componentini olusturdu
        anim =GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>(); // yapay zeka
        sesKaynak = GetComponent<AudioSource>(); // ses kaynagi 
        secim = Random.Range(0, 3); //rastgele silah atama
        random = Random.Range(0, 3);//rastgele renk atama
        ic.GetComponent<SkinnedMeshRenderer>().material = renkler[random]; // dusmanin ic material rengi
        dis.GetComponent<SkinnedMeshRenderer>().material = renkler[random]; // dusmanin dis material rengi
        Seviye();
        GameObject g= Instantiate(silahlar[secim], SilahNesnesi.transform);//Dusmanin silahini yaratmak
        Silah();
    }
    void Silah() // dusmanin silah kismi
    {
        if (secim == 0)
        {
            fireRate = 0.2f;
            toplamMermi = 300;
            sarjorBasinaMermi = 30;
            menzil = 6000.0f; 
            hasar = 10.0f;  
            minSekme = -150.0f;   
            makSekme = 150.0f;   
            s = 0; 
            m = 0;
            sekmeDegeri = Random.Range(minSekme + seviyeEtkisi, makSekme - seviyeEtkisi); // silahin sekme degerini seviye etkisine gore random atiyoruz
        }
        else if (secim == 1)
        {
            fireRate = 0.1f;
            toplamMermi = 180;
            sarjorBasinaMermi = 45;
            menzil = 3000.0f;
            hasar = 5.0f;
            minSekme = -100.0f;
            makSekme = 100.0f;
            s = 1;
            m = 1;
            sekmeDegeri = Random.Range(minSekme + seviyeEtkisi, makSekme - seviyeEtkisi); // silahin sekme degerini seviye etkisine gore random atiyoruz
        }
        else if (secim == 2)
        {
            fireRate = 0.33f;
            toplamMermi = 50;
            sarjorBasinaMermi = 8;
            menzil = 1500.0f;
            hasar = 15.0f;
            minSekme = -120.0f;
            makSekme = 120.0f;
            s = 2;
            m = 2;
            sekmeDegeri = Random.Range(minSekme + seviyeEtkisi, makSekme - seviyeEtkisi); // silahin sekme degerini seviye etkisine gore random atiyoruz
        }
        else if (secim == 3)
        {
            fireRate = 1.0f;
            toplamMermi = 30;
            sarjorBasinaMermi = 5;
            menzil = 10000.0f;
            hasar = 50.0f;
            minSekme = -150.0f;
            makSekme = 150.0f;
            s = 3;
            m = 3;
            sekmeDegeri = Random.Range(minSekme + seviyeEtkisi, makSekme - seviyeEtkisi); // silahin sekme degerini seviye etkisine gore random atiyoruz
        }
    }
    void Seviye() // dusmanlarin rengine gore zorluk derecesi atadigimiz kisim
    {
        if (random == 0)
        {
            seviyeEtkisi = 0;
            can = 50;
            zirh = 25;
        }
        else if (random == 1)
        {
            seviyeEtkisi = 5;
            can = 75;
            zirh = 50;
        }
        else if (random == 2)
        {
            seviyeEtkisi = 10;
            can = 100;
            zirh = 50;
        }
        else if (random == 3)
        {
            seviyeEtkisi = 15;
            can = 150;
            zirh = 100;
        }
    }
    void Move(bool onay,float hiz) // dusmanimizin hareket ettigi kisim
    {
        if (sarjor == false)// sarjor degistirmiyorsa hareket ediyor
        {
            if (onay == true) //onay verirse hareket et
            {
                transform.LookAt(Player.transform.position); // karakterimizin posizyona bakcak
                agent.Move(transform.forward * hiz); // y yonunde

                anim.SetBool("idle", false);
            }
            else
            {
                transform.LookAt(Player.transform.position);
                hiz = 0;
                anim.SetBool("idle", true);
            }
        }
        
    }
    void MesafeKontrol() //karakter ile dusman arasindaki mesafeyi kontrol eden kisim
    {
        mesafe = Vector3.Distance(transform.position, Player.transform.position); // dusman ile karakter pozisyonu arasindaki farki aliyor 
        if (mesafe > 50 + seviyeEtkisi)
        {
            
            fire = false;
            anim.SetFloat("dikey", 1);
            Move(true,hiz=0.1f);


        }
        else if(mesafe >= 30 + seviyeEtkisi && mesafe < 50 + seviyeEtkisi)
        {
            
            fire = true;
            anim.SetFloat("dikey", 1);
            Move(true, hiz = 0.075f);
        }
        else if(mesafe>=10 + seviyeEtkisi && mesafe < 30 + seviyeEtkisi)
        {
            
            fire = true;
            anim.SetFloat("dikey", 0);
            Move(true, hiz);
        }
        else if (mesafe >= 0 && mesafe < 10 + seviyeEtkisi)
        {
            fire = true;
            Move(false,0);
            
            
        }

    }
    void AtesEtme() // dusmanin ates etmesini saglayan fonksiyon
    {
        if (fire == true && zaman>fireRate && oldu==false && sarjor==false && intihar==false)
        {
            atilanMermi++;
            sesKaynak.PlayOneShot(silahSesleri[s]);
            MuzzleEfekt();
            GameObject g = Instantiate(mermiPrefab, nu.transform.position, Quaternion.identity); //Merminin namlu ucundan cikacagini belirterek ve yaratiyoruz
            g.GetComponent<Bullet>().hasar = hasar;  //olusturdugumuz mermiye ne kadar hasar verecegini tanimliyoruz
            g.GetComponent<Bullet>().yz = true;
            Rigidbody rb = g.GetComponent<Rigidbody>();// Yercekimini olusturduk
            rb.velocity = transform.forward * menzil * Time.deltaTime; //Mermiyi ileriye dogru goturme
            rb.AddForce(Vector3.right * sekmeDegeri * Time.deltaTime, ForceMode.Acceleration);//Silahin saga sola sekmesi
            rb.AddForce(Vector3.up * sekmeDegeri * Time.deltaTime, ForceMode.Acceleration);// Silahin yukari asagi sekmesi
            zaman = 0;
            Destroy(g, 3.0f);// mermiyi ortadan kaldirma
        }
        
    }

    IEnumerator Sarjor()
    {
        anim.SetBool("reload", true); //sarjor degistirme animasyonun true yapiyoruz
        yield return new WaitForSeconds(2.6f);// oynatiy
        sarjor = false;
        anim.SetBool("reload", false);// sarjor degistirme bittikten sonra false yapiyoruz
        atilanMermi = 0;  
        toplamMermi -= atilanMermi;// toplam mermiyi attigimiz mermiden cikartiyoruz
    }

    IEnumerator Intihar() // dusmanin belirli bir can degerine dustugu zaman kendini oldurmesini yarayan fonksiyon
    {
        if (can <= 25 || toplamMermi <= 0 && oldu == false) // dusmanin can degeri 25 den az veya mermisi yoksa ve olmediyse dusman kendini olduruyor
        {
            intihar = true;
            anim.SetFloat("dikey", 1);
            Move(true, hiz = 0.12f);
            if(mesafe<=5 && patladi == false ) //mesafe 5 se,karakter onceden patlamadiysa ve olmediyse dusman kendini patlatiyor
            {
                yield return new WaitForSeconds(1.0f);
                GameObject g = Instantiate(patlama, transform.position, Quaternion.identity);
                patladi = true;
                Player.GetComponent<Oyuncu>().can -= 50;
                Destroy(gameObject);
            }
        }

    }

    IEnumerator Olum() // dusmanin cani 0 dan kucukse hareket etmeyi false yapiyor ve olum animasyonun oynatiyor ve 2sn de dusman yok oluyor
    {
        oldu = true;
        intihar = false;
        Move(false, 0);
        anim.SetBool("olum", true);
        yield return new WaitForSeconds(2.0f);
        islem.oluSayisi++; //dusman olunce olu sayisini bir kere artiyor
        Destroy(gameObject);
    }
   
    void MuzzleEfekt()
    {
        GameObject g = Instantiate(muzzlePrefab, nu.transform);// namlu ucunun  yerini olusturuyoruz
        g.transform.SetParent(nu.transform);// namlu ucuna bagli kalmasini sagliyoruz
        g.GetComponent<ParticleSystem>().GetComponent<Renderer>().material = muzzle[m];// yukledigimiz namlu flaslarinin oldugu materyallere erismek icin 
        Destroy(g, 0.20f);// atesi bitirdigimiz anda namlu ucundan gelen isigi bitirmek icin
    }
    // Update is called once per frame
    void Update()
    {
        if (atilanMermi == sarjorBasinaMermi) // dusmanin attigi mermiyle sarjorundeki mermi esit olunca sarjor degistirmeyi baslatiyor
        {
            sarjor = true;
            StartCoroutine("Sarjor");
        }
        else
        {
            sarjor = false;
        }
        zaman += Time.deltaTime;
        MesafeKontrol();
        AtesEtme();
        if (can <= 0) // can sifirin altina dustugu zaman olum fonksiyonunu cagiriyor
        {
            StartCoroutine("Olum");
        }

        StartCoroutine("Intihar");
    }
}
