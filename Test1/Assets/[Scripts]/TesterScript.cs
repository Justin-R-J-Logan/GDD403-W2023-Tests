using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StandardDeck d = new StandardDeck();

        Debug.Log(d.DrawNextCard().ToString());
        Debug.Log(d.Count);
    }
}
