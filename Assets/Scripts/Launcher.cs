using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Phys>() != null)
        {
            other.GetComponent<Phys>().m_v3MoveDirection = new Vector3(-62.5f, 85, 0);
            other.GetComponent<Phys>().bBlockInput = true;
            GameData.instance.PlayIntroAudio();
        }
    }
}
