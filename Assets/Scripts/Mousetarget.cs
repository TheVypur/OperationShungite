using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mousetarget : MonoBehaviour
{
    public GameObject goCam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = goCam.transform.position + (goCam.transform.forward * 10);
    }
}
