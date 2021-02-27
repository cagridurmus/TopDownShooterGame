using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Islemler : MonoBehaviour
{
    public GameObject YapayZeka;
    public GameObject[] spawn; //spawnlari kontrol etmek icin
    public GameObject[] alandakiler; // alandaki dusmanlari kontrol etmek icin
    public int sayi; // kac tane dusman olcagi
    public bool hepsiOlduMu;
    public Text olenDusmanSayisi,waveBilgi;
    public int oluSayisi;
    GameObject player;
    int dalga;
    public GameObject wi;
    
    //public int q=0; // oldurdugun dusman sayisi

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dalga = 1;
        DusmanSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        Kontrol();
        YeniDusmanSpawn();
        olenDusmanSayisi.text = oluSayisi.ToString(); // olu sayisini string e cevirerek olen dusman sayisi textine yaziyor
        waveBilgi.text = "Wave: " + dalga.ToString(); // dalga sayisini string e cevirerek wave bilgi textine yaziyor
        if (player.GetComponent<Oyuncu>().can <= 0) // can degeri 0 yada 0 dan kucuk oldugu zaman gameover fonksiyonunu cagir
        {
            
            GameOver();
        }
    }

    void GameOver() // Oyunun bitis kismi
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // oyunu yeniden baslatiyor
        
    }
   
    void Kontrol() // butun dusmanlarin olup olmedigini kontrol eden kisim
    {
        alandakiler = GameObject.FindGameObjectsWithTag("enemy"); // alandaki dusmanlari enemy tagi sayesiyle buluyor
        
        if (alandakiler.Length == 0) // alandaki dusmanlarin sayisi sifir olunca hepsiOlduMu true oluyor
        {
            hepsiOlduMu = true;
        }
        else // hepsi olmediyse hepsiOlduMu false oluyor
        {
            hepsiOlduMu = false;
        }
        

    }

    void YeniDusmanSpawn() // dusman oldukce yeni dusman spawnliyor
    {
        if (hepsiOlduMu == true) // dusmanlarin hepsi olduyse olum sayisini artiriyor ve DusmanSpawn() fonksiyonunu cagiriyor
        {
            sayi += 2;
            dalga++;
            
            DusmanSpawn();
            hepsiOlduMu = false;
        }
    }

    IEnumerator wiGoster() // bu kisim gelen dusman dalgasinin gostermeye yariyor
    {
        wi.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        wi.SetActive(false);
    }

    void DusmanSpawn() // dusmanlarin rastgele olarak dogmasini yarayan kisim
    {
        
        StartCoroutine("wiGoster"); // dusman spawni  basladiginda dusman dalgasini gosteriyor
        for(int i = 0; i < sayi; i++)
        {
            int random = Random.Range(0, 3);
            GameObject g = Instantiate(YapayZeka,spawn[random].transform.position,Quaternion.identity); // dusmanlari yaratacagimiz yeri belirliyoruz
        }
    }
}
