using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiElements : MonoBehaviour
{

    public string seedID;
    public Text seed;
    

    // Start is called before the first frame update
    void Start()
    {

        

    }

    // Update is called once per frame
    void Update()
    {

        seed.text = "Seed: " + seedID;

    }
}
