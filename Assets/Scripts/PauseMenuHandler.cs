using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour
{
    public GameObject menu;
    public GameObject goPlayer;
    public bool bIsPaused = false;

    // Update is called once per frame
    void Update()
    {

        if (!bIsPaused && menu.activeInHierarchy)
        {
            menu.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (bIsPaused)
            {
                menu.SetActive(false);
                Time.timeScale = 1;
                GameData.instance.bIsPaused = false;
                bIsPaused = false;
                goPlayer.GetComponent<Move>().enabled = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                menu.SetActive(true);
                Time.timeScale = 0;
                GameData.instance.bIsPaused = true;
                bIsPaused = true;
                goPlayer.GetComponent<Move>().enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene("Scenes/IntroScene");
    }
}


