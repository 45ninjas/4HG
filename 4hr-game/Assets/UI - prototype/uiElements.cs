using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiElements : MonoBehaviour
{

    GameObject gs; //getseed
    GameObject storescript; 



    // Start is called before the first frame update
    void Start()
    {

        gs = GameObject.Find("default");
        storescript = gs.GetComponent < default > ();



    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(gs.seed);
        

    }
}
