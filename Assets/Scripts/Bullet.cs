using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject m_GoShooter;
    public float m_fVelocity;
    public Vector3 m_v3Direction;
    protected float m_fInternalTimer;
    public float m_fMaxTimer = 4;
    public string m_sTag;
    public float m_fDamage = 10;
    public float fPunch = 20;

    // Update is called once per frame
    protected virtual void Update()
    {
        m_fInternalTimer += Time.deltaTime;

        this.transform.position += m_v3Direction * m_fVelocity * Time.deltaTime;

        if (m_fInternalTimer > m_fMaxTimer)
        {
            gameObject.SetActive(false);
        }
    }


    public void Shoot(Transform tInitialPos, GameObject goShooter, float Velocity, Vector3 v3Direction, float fMaxTimer)
    {
        this.transform.SetParent(null);
        this.transform.position = tInitialPos.position;
        this.transform.rotation = tInitialPos.rotation;
        m_fInternalTimer = 0;
        m_GoShooter = goShooter;
        m_v3Direction = v3Direction;
        m_fVelocity = Velocity;
        m_fMaxTimer = fMaxTimer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DocPhys>())
        {
            other.gameObject.SetActive(false);
            DetonateOrDestroy();
        }       
        else
            return;

        
    }

    void DetonateOrDestroy()
    {
        gameObject.SetActive(false);
    }
}
