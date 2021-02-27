using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandik : MonoBehaviour
{
    public GameObject[] nesneler;
    Light isik;
    bool sunumYapildiMi; // her f bastigimizda sandik degismesin diye bool degiskeni tanimladik
    public int n;
    // Start is called before the first frame update
    void Start()
    {
        sunumYapildiMi = false; // bu bool degiskenin basta herhangi bir sandik acmadigimizdan false atadik
    }
    public void sunumYap()
    {
         
        if (sunumYapildiMi==false)// eger f e basilmadiysa bu kisim yapilcak
        {
            isik = GetComponentInChildren<Light>(); //Sandik acilgidindaki isik tanimladik
            n = Random.Range(0, 5); // random obje verecek 6 obje oldugundan ondan dolayi (0,5) dedik
            nesneler[n].SetActive(true); // objeleri aktif ettik
            if (n == 0)
            {
                isik.color = Color.grey;
            }
            else if (n == 1)
            {
                isik.color = Color.cyan;
            }
            else if (n == 2)
            {
                isik.color = Color.yellow;
            }
            else if (n == 3)
            {
                isik.color = Color.green;
            }
            else if (n == 4)
            {
                isik.color = Color.red;
            }
            else if (n == 5)
            {
                isik.color = Color.blue;
            }
            sunumYapildiMi = true;// f e bascagimiz icin sandik acilcak ve obje cikcak ondan dolayi bool degiskenimizi true atadik
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
