using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SmokeController.cs
//Justin Logan
//A00066979
//04-18-2023


public class SmokeController : MonoBehaviour
{
    private AudioSource hitSound;

    // Start is called before the first frame update
    void Start()
    {
        hitSound = GetComponent<AudioSource>();
        hitSound.Play();

        Invoke("DestroyYourself", 0.8f);
    }

    public void DestroyYourself()
    {
        Destroy(this.gameObject);
    }
}
