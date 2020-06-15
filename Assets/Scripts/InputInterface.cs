using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputInterface : MonoBehaviour
{
    public HashSet<KeyCode> m_ActiveKeys;
    private HashSet<KeyCode> m_prevKeys;
    public Dictionary<string, float> m_AxisValues;
    // Start is called before the first frame update
    void Start()
    {
        m_ActiveKeys = new HashSet<KeyCode>();
        m_prevKeys = new HashSet<KeyCode>();
        m_AxisValues = new Dictionary<string, float>();
    }

    private void Update()
    {
        m_ActiveKeys.Clear();
    }

    // Update last to clear active keys
    void LateUpdate()
    {


        m_prevKeys = m_ActiveKeys;

    }

    public bool GetKeyDown(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            m_ActiveKeys.Add(key);
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool GetKey(KeyCode key)
    {
        if (Input.GetKey(key))
        {
            m_ActiveKeys.Add(key);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetKeyUp(KeyCode key)
    {
        if (Input.GetKeyUp(key))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetAxis(string sAxis)
    {
        return Input.GetAxis(sAxis);
    }

    public bool GetMouseButton(int val)
    {
        return Input.GetMouseButton(val);
    }
}
