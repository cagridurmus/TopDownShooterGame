using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasarAl : MonoBehaviour
{
    public GameObject kanEfekt;
    Bullet mermiKod;
    public string tagim;
    public GameObject anaNesne;
    public float hasarPay;
    // Start is called before the first frame update
    void Start()
    {
        tagim = this.gameObject.tag;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void TagKontrol() // dusmani hasar verdigimizde neresine vurdugumuzu kontrol ederek o vurdugumuz kisma ait hasar payini gosteren kisim
    {
        if (tagim == "solbacak")
        {
            hasarPay = -2.0f;
        }
        if (tagim == "sagbacak")
        {
            hasarPay = -2.0f;
        }
        if (tagim == "solkol")
        {
            hasarPay = -2.5f;
        }
        if (tagim == "sagkol")
        {
            hasarPay = -2.5f;
        }
        if (tagim == "govde")
        {
            hasarPay = -5.0f;
        }
        if (tagim == "kafa")
        {
            hasarPay = -10.0f;
        }
    }
    private void OnTriggerEnter(Collider mermi) //merminin carpip carmagini kontrol ediyoruz
    {

        if (mermi.gameObject.tag == "mermi")
        {
            mermiKod = mermi.GetComponent<Bullet>(); //mermi olusturduk
            if (anaNesne.tag == "enemy" && mermiKod.ben==true) // dusman kendine siktiysa ve karakter siktiysa 
            {
                TagKontrol();
                GameObject k = Instantiate(kanEfekt, transform); // kan efektini olusturduk
                Destroy(k, 1.0f); // kan efektiniz 1 sn sonra yok ediyor
                if (anaNesne.GetComponent<AI>().zirh > 0)
                {
                    anaNesne.GetComponent<AI>().zirh -= (mermiKod.hasar + hasarPay); //merminin hasariyla vucuduna gelen kismina verdigi hasarpayiyla topliyarak dusmanin zirhindan cikartarak dusmanin kac zirhi kaldigini hesapliyoruz
                }
                else
                {
                    anaNesne.GetComponent<AI>().can -= (mermiKod.hasar + hasarPay); //merminin hasariyla vucuduna gelen kismina verdigi hasarpayiyla topliyarak dusmanin canindan cikartarak dusmanin kac cani kaldigini hesapliyoruz
                }
                
                
            }
            if (anaNesne.tag == "Player" && mermiKod.yz==true) // player siktiysa ve yapay zekadan kendine gelen mermiyse
            {
                TagKontrol();
                GameObject k = Instantiate(kanEfekt, transform); // kan efektini olusturduk
                
                Destroy(k, 1.0f); // kan efektiniz 1 sn sonra yok ediyor
                if (anaNesne.GetComponent<Oyuncu>().zirh > 0)
                {
                    anaNesne.GetComponent<Oyuncu>().zirh -= (mermiKod.hasar + hasarPay); //merminin hasariyla vucuduna gelen kismina verdigi hasarpayiyla topliyarak karakterimizin zirhindan cikartarak dusmanin kac zirhi kaldigini hesapliyoruz
                }
                else
                {
                    anaNesne.GetComponent<Oyuncu>().can -= (mermiKod.hasar + hasarPay); //merminin hasariyla vucuduna gelen kismina verdigi hasarpayiyla topliyarak karakterimizin canindan cikartarak dusmanin kac cani kaldigini hesapliyoruz
                }
            }
        }
    }
}

