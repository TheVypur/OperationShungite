using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shungite : MonoBehaviour
{
    public float fBobSpeed = 0.1f;
    public float fSpinSpeed = 1;
    Vector3 v3Pos;

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(new Vector3(0, fSpinSpeed * Time.deltaTime, 0));
        v3Pos = this.transform.position;
        v3Pos.y += Mathf.Sin(Time.time) * fBobSpeed;
        this.transform.position = v3Pos;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Phys>())
        {
            //player hit
            GameData.instance.iShungitecollected += 1;
            gameObject.SetActive(false);
        }
    }
}
