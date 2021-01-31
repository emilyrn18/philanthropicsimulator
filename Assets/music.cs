using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music : MonoBehaviour
{
    Component[] newAudio;
    // Start is called before the first frame update
    void Start()
    {
        newAudio = GetComponents(typeof(AudioSource));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            //stop all the sounds
            AudioSource[] allSounds;
            allSounds = FindObjectsOfType<AudioSource>();
            foreach(AudioSource a in allSounds){
                a.Stop();
            }
            //start the new sound
            foreach(AudioSource b in newAudio){
                b.Play();
            }
            
        }
    }
}
