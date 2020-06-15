using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    public Checkpoint checkpoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Phys>())
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = checkpoint.transform.position;
            other.GetComponent<CharacterController>().enabled = true;
        }
    }
}
