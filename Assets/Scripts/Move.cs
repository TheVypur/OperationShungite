using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public FirstPersonCamera m_Camera;
    protected Phys m_Phys;
    protected InputInterface m_InputInterface;
    protected Animator m_Anim;
    public Vector3 m_v3UserInput;

    protected Vector3 v3Flattened;

    protected HashSet<KeyCode> m_ActiveKeys;

    public float fForwardInfluence = 0;
    public float fRightInfluence = 0;

    public float fInfluenceDrainRate = 50f;
    public float fInfluenceGainRate = 50;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_Phys = GetComponent<Phys>();
        m_Camera = Camera.main.GetComponentInParent<FirstPersonCamera>();
        m_InputInterface = GetComponent<InputInterface>();
        m_Anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    public virtual void CheckControlsUpdate()
    {
        m_v3UserInput = Vector3.zero;
        m_ActiveKeys = new HashSet<KeyCode>();

        if (m_InputInterface.GetKey(KeyCode.W))
        {
            m_ActiveKeys.Add(KeyCode.W);
            v3Flattened = this.transform.forward;
            v3Flattened.y = 0;

            m_v3UserInput += v3Flattened.normalized;
            fForwardInfluence += fInfluenceGainRate * Time.deltaTime;
        }

        if (m_InputInterface.GetKey(KeyCode.A))
        {
            m_ActiveKeys.Add(KeyCode.A);
            v3Flattened = this.transform.right * -1;
            v3Flattened.y = 0;

            m_v3UserInput += v3Flattened.normalized;
            fRightInfluence += fInfluenceGainRate * Time.deltaTime;
        }

        if (m_InputInterface.GetKey(KeyCode.S))
        {
            m_ActiveKeys.Add(KeyCode.S);
            v3Flattened = this.transform.forward * -1;
            v3Flattened.y = 0;

            m_v3UserInput += v3Flattened.normalized;
            fForwardInfluence -= fInfluenceGainRate * Time.deltaTime;
        }

        if (m_InputInterface.GetKey(KeyCode.D))
        {
            m_ActiveKeys.Add(KeyCode.D);
            v3Flattened = this.transform.right;
            v3Flattened.y = 0;

            m_v3UserInput += v3Flattened.normalized;
            fRightInfluence -= fInfluenceGainRate * Time.deltaTime;
        }

        if (m_InputInterface.GetKeyDown(KeyCode.Space))
        {
            m_Phys.Jump();
        }

        fForwardInfluence *= 0.98f;
        fRightInfluence *= 0.98f;
    }

    public Vector3 GetUserInput()
    {
        return m_v3UserInput;
    }
}
