using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSyncer : MonoBehaviour
{
    private Material m_Mat;
    float fTimer = 0;
    public Color baseColor;
    public Color altColor;
    public float fIntensity = 4;
    public float fTimeMax = 0.7f;
    public float fStartOffset = 0.35f;

    public bool bAlternateColor;
    private bool bUseFirstColor = true;
    // Start is called before the first frame update
    void Start()
    {
        m_Mat = this.GetComponent<MeshRenderer>().sharedMaterial;
        fTimer = fStartOffset;
    }


    // Update is called once per frame
    void Update()
    {
        fTimer += Time.deltaTime;

        if (fTimer > fTimeMax)
        {
            fTimer -= fTimeMax;

            if (bAlternateColor)
            {
                if (bUseFirstColor)
                {
                    bUseFirstColor = false;
                    StartCoroutine(Pulse(0.7f, baseColor));
                }
                else
                {
                    bUseFirstColor = true;
                    StartCoroutine(Pulse(0.7f, altColor));
                }
                
            }
            else
            {
                StartCoroutine(Pulse(0.7f, baseColor));
            }
            
        }
    }

    IEnumerator Pulse(float fTime, Color c)
    {
        m_Mat.SetColor("_EmissionColor", c * fIntensity);

        yield return null;

        float fTimer = 0;
        while (fTimer < fTime)
        {
            fTimer += Time.deltaTime;
            m_Mat.SetColor("_EmissionColor", c * Mathf.LinearToGammaSpace(fIntensity * ( 1 - (fTimer / fTime)) + 0.1f));
            yield return null;
        }
    }
}
