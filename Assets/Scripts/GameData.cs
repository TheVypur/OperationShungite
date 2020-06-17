using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    public GameObject m_GeneralUI;
    public GameObject m_UIToShow;
    public Text m_ShungiteCollectedtext;
    public GameObject GameOverUI;
    public GameObject VictoryScreen;
    public GameObject goPlayer;
    public GameObject DefeatHimPanel;

    public bool bIsPaused = false;

    public static GameData instance = null;

    public bool bHasTriggeredDoc = false;

    public int iShungitecollected = 0;

    public DocMove DocMove;

    public GameObject[] m_LightsToTurnOff;

    public AudioSource m_MusicSource;

    public AudioSource m_IntroSource;

    public Gun m_Gun;

    public void TriggerDoc()
    {
        m_MusicSource.Stop();
        StartCoroutine(trigger());
        m_IntroSource.Stop();

    }

    public void PlayIntroAudio()
    {
        m_IntroSource.Play();
    }

    IEnumerator trigger()
    {
        for (int i = 0; i < m_LightsToTurnOff.Length; i++)
        {
            m_LightsToTurnOff[i].SetActive(false);
        }
        m_UIToShow.SetActive(true);
        m_ShungiteCollectedtext.text = "1/6";

        yield return new WaitForSeconds(3);
        DocMove.bActive = true;
        bHasTriggeredDoc = true;
       

    }

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;

        if (instance != this)
            instance = this;

        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 600;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (iShungitecollected > 0 && !bHasTriggeredDoc)
        {
            TriggerDoc();
        }

        if (iShungitecollected == 6)
        {
            m_Gun.gameObject.SetActive(true);
            DefeatHimPanel.SetActive(true);
            m_ShungiteCollectedtext.enabled = false;
        }

        m_ShungiteCollectedtext.text = iShungitecollected + "/6";
    }

    public void ShowGameOverScreen()
    {
        GameOverUI.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scenes/test_scene");
    }

    public void EndGame()
    {
        VictoryScreen.SetActive(true);
    }
}
