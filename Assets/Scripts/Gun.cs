using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject m_goCamera;
    public GameObject prefBullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(this.transform.position + m_goCamera.transform.forward);

        if (Input.GetMouseButtonDown(0))
        {
            GameObject obj = GameObject.Instantiate(prefBullet);
            Bullet b = obj.GetComponent<Bullet>();
            b.Shoot(this.transform, this.gameObject, 80, this.transform.forward, 2);
        }
    }
}
