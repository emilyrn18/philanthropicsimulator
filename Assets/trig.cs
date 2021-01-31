using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trig : MonoBehaviour
{
    public GameObject b;
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            b.SetActive(true);
        }
    }
}
