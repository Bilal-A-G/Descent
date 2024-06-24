using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public GameObject on;
    public GameObject off;
    private bool isON;
    // Start is called before the first frame update
    void Start()
    {
        on.SetActive(false);
        off.SetActive(true);
        isON = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {

            if (isON)
            {
                on.SetActive(false);
                off.SetActive(true);
            }

            if(!isON)
            {
                on.SetActive(true);
                off.SetActive(false);
            }


            isON = !isON;
        }

    }
}
