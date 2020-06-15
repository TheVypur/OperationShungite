using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    private float fYRotation;
    private float fXRotation;

    public float fClampAngle = 85f;

    public float fSensitivity = 15;

    public GameObject goPlayerObj;
    public Transform tCamera;
    private Phys playerPhys;

    public bool bIsHoldingModifier = false;
    private float fYRotationDesired = 0;


    public float fSmooth = 2;
    public float fLerp = 0.975f;

    public Vector3 v3Offset = new Vector3(0, 1, 0);

    public float fAngleDifference = 0;
    public float fMaxOverShoulderAngleDifference = 175;

    public float fCheckDistance = 6;

    public LayerMask wallJumpLayerMask;

    public float fMaxLean = 20;
    public float fWallDistance;

    private void Start()
    {
        playerPhys = goPlayerObj.GetComponent<Phys>();
    }

    // Update is called once per frame
    private void Update()
    {
        float inputX = Input.GetAxis("Mouse Y");
        float inputY = Input.GetAxis("Mouse X");

        fYRotation += inputY * MouseSens.instance.fSens;
        fXRotation += -inputX * MouseSens.instance.fSens;

        fXRotation = Mathf.Clamp(fXRotation, -fClampAngle, fClampAngle);

        Quaternion localRotation = Quaternion.Euler(fXRotation, fYRotation, 0.0f);
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            bIsHoldingModifier = true;

            fAngleDifference = Mathf.Abs(localRotation.eulerAngles.y - goPlayerObj.transform.eulerAngles.y);

            if (fAngleDifference > fMaxOverShoulderAngleDifference && fAngleDifference < 360-fMaxOverShoulderAngleDifference)
            {
                localRotation = Quaternion.Euler(fXRotation, tCamera.localEulerAngles.y, 0.0f);
                fYRotation -= inputY * fSensitivity;
            }
            else
            {
                
            }
        }
        else
        {
            bIsHoldingModifier = false;
         
        }
        tCamera.localRotation = Quaternion.Lerp(tCamera.localRotation, localRotation, fLerp);
    }

    private void LateUpdate()
    {
        CameraUpdater();
    }

    public void CameraUpdater()
    {
        this.transform.position = goPlayerObj.transform.position + v3Offset;
    }

    private void CheckWallLean()
    {
        Vector3 v3Ret = Vector3.zero;
        float fMinDistance = 99999;
        fWallDistance = fCheckDistance;

        Vector3 v3Left = (goPlayerObj.transform.right * -1).normalized * fCheckDistance;
        Vector3 v3Right = (goPlayerObj.transform.right).normalized * fCheckDistance;
        Vector3 v3Forward = (goPlayerObj.transform.forward).normalized * fCheckDistance;
        Vector3 v3Backwards = (goPlayerObj.transform.forward * -1).normalized * fCheckDistance;

        Debug.DrawLine(this.transform.position, this.transform.position + v3Left, Color.red, 3);
        Debug.DrawLine(this.transform.position, this.transform.position + v3Right, Color.green, 3);
        Debug.DrawLine(this.transform.position, this.transform.position + v3Forward, Color.cyan, 3);
        RaycastHit hitinfo;
        if (Physics.Raycast(this.transform.position, Vector3.down, out hitinfo, fCheckDistance, wallJumpLayerMask, QueryTriggerInteraction.UseGlobal))
        {
            v3Ret = hitinfo.normal;
        }
        if (Physics.Raycast(this.transform.position, v3Left, out hitinfo, fCheckDistance, wallJumpLayerMask, QueryTriggerInteraction.UseGlobal))
        {
            float distance = Vector3.Distance(this.transform.position, hitinfo.point);
            if (distance < fMinDistance)
            {
                v3Ret = hitinfo.normal;
                fMinDistance = distance;
            }

        }
        if (Physics.Raycast(this.transform.position, v3Right, out hitinfo, fCheckDistance, wallJumpLayerMask, QueryTriggerInteraction.UseGlobal))
        {
            float distance = Vector3.Distance(this.transform.position, hitinfo.point);
            if (distance < fMinDistance)
            {
                v3Ret = hitinfo.normal;
                fMinDistance = distance;
            }
        }
        if (Physics.Raycast(this.transform.position, v3Forward, out hitinfo, fCheckDistance, wallJumpLayerMask, QueryTriggerInteraction.UseGlobal))
        {
            float distance = Vector3.Distance(this.transform.position, hitinfo.point);
            if (distance < fMinDistance)
            {
                v3Ret = hitinfo.normal;
                fMinDistance = distance;
            }
        }
        if (Physics.Raycast(this.transform.position, v3Backwards, out hitinfo, fCheckDistance, wallJumpLayerMask, QueryTriggerInteraction.UseGlobal))
        {
            float distance = Vector3.Distance(this.transform.position, hitinfo.point);
            if (distance < fMinDistance)
            {
                v3Ret = hitinfo.normal;
                fMinDistance = distance;
            }
        }


    }


}
