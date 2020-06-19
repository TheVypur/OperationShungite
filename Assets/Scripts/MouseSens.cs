using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSens : MonoBehaviour
{
    public static MouseSens instance = null;

    public float fSens;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;

        if (instance != this)
        {
            GameObject.Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void SetSensFromSlider(float val)
    {
        fSens = val / 10;
    }
}
