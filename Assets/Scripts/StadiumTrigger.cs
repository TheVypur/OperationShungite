using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            GameObject.DestroyImmediate(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (bHasBeenTriggered)
        {
            GameData.instance.goPlayer.GetComponent<CharacterController>().enabled = false;
            GameData.instance.goPlayer.transform.position = this.transform.position;
            GameData.instance.goPlayer.GetComponent<CharacterController>().enabled = true;
            Camera.main.transform.parent.GetComponent<FirstPersonCamera>().enabled = true;
            Camera.main.transform.parent.GetComponent<Animator>().Play("Default");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (bHasBeenTriggered)
        {
            GameData.instance.goPlayer.GetComponent<CharacterController>().enabled = false;
            GameData.instance.goPlayer.transform.position = this.transform.position;
            GameData.instance.goPlayer.GetComponent<CharacterController>().enabled = true;
            Camera.main.transform.parent.GetComponent<FirstPersonCamera>().enabled = true;
            Camera.main.transform.parent.GetComponent<Animator>().Play("Default");
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
