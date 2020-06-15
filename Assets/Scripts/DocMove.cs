using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocMove : MonoBehaviour
{
    DocPhys m_Phys;

    public GameObject m_goTarget;
    public DocAudioController m_Audio;

    protected Animator m_Anim;
    public Vector3 m_v3UserInput;

    protected Vector3 v3Flattened;

    protected HashSet<KeyCode> m_ActiveKeys;

    public float fForwardInfluence = 0;
    public float fRightInfluence = 0;

    public float fInfluenceDrainRate = 50f;
    public float fInfluenceGainRate = 50;

    private float fActionTimer = 0;
    private float fActionMaxCheck = 3.5f;

    int iRand = 0;

    public bool bActive = true;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_Phys = GetComponent<DocPhys>();

        m_Anim = GetComponentInChildren<Animator>();
        m_Audio = GetComponent<DocAudioController>();
    }

    private void Update()
    {
        fActionMaxCheck = 3.0f - (GameData.instance.iShungitecollected / 2f);
    }

    // Update is called once per frame
    public virtual void CheckControlsUpdate()
    {
        m_v3UserInput = Vector3.zero;
        m_ActiveKeys = new HashSet<KeyCode>();


        fActionTimer += Time.deltaTime;

        if (fActionTimer > fActionMaxCheck && bActive)
        {
            //Lots of logic
            iRand = Random.Range(0, 2);

            if (iRand == 0)
            {
                m_Audio.PlayIdleClip();
                fActionTimer = 1.5f;
            }
            else
            {
                m_Phys.Leap();
                fActionTimer = 0;
            }


            

            
        }


        fForwardInfluence *= 0.98f;
        fRightInfluence *= 0.98f;
    }

    public Vector3 GetUserInput()
    {
        return m_v3UserInput;
    }
}
