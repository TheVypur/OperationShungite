using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StadiumTrigger : MonoBehaviour
{
    public static StadiumTrigger instance = null;

    public bool bHasBeenTriggered = false;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;

        if (instance != this)
        {
            instance = this;
            GameObject.Destroy(this.gameObject);
        }


        DontDestroyOnLoad(this.gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (bHasBeenTriggered)
        {
            GameData.instance.goPlayer.GetComponent<CharacterController>().enabled = false;
            GameData.instance.goPlayer.transform.position = this.transform.position;
            GameData.instance.goPlayer.GetComponent<CharacterController>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Move>())
        {
            bHasBeenTriggered = true;
        }
    }
}
