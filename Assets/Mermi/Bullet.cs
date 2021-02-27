using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float hasar;
    BoxCollider bc;
    public bool ben, yz;
    // Start is called before the first frame update
    void Start()
    {
       
        bc = GetComponent<BoxCollider>(); // box collider oyunun basinda aktif ettik
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other) // mermi oyundaki herhangi bir karaktere carpmazsa ne olacagini belirten kisim
    {
        if (other.gameObject.tag == "enemy") //mermi dusmandan geliyorsa
        {
            bc.enabled = false;
        }
        else
        {
            Destroy(gameObject); // karaktere carparsa yok ediyor
        }
        
    }
}
