using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocPhys : MonoBehaviour
{
    public GameObject m_goTarget;
    public Phys m_PlayerPhys;
    public DocAudioController m_Audio;

    private CharacterController m_Controller;
    private DocMove m_Move;
    private Animator m_Anim;

    public Vector3 m_v3MoveDirection;
    private Vector3 v3MoveDirFlattened;
    public bool bIsGrounded = false;
    private bool bWasGrounded = false;
    private Vector3 v3UserInput;

    private Vector3 m_v3GroundNormal;
    private Vector3 v3PerpendicularVector;
    private Vector3 m_v3WallNormal;

    public float fWalkSpeed = 50;
    public float fAirSpeed = 50;

    public float fSpeedDampening = 0.1f;

    protected float fJumpTimer = 0;
    protected float fJumpMax = 0.15f;

    public float fMaxMoveSpeed = 20;
    public float fMaxAirSpeed = 25;

    public float fGroundCheckDistance = 1;
    public float fNormalCheckDistance = 1;
    public Vector3 v3GroundCheckOffset = new Vector3(0, -1.2f, 0);
    public LayerMask m_GroundCheckMask;

    public float fGravity = -50f;
    public float fFriction = 0.008f;

    public float fJumpSpeed = 80;
    public bool bJumped = false;

    public float fLerp = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        m_Move = GetComponent<DocMove>();
        m_Anim = GetComponentInChildren<Animator>();
        m_Audio = GetComponent<DocAudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        bIsGrounded = IsGrounded();
        if (bWasGrounded && !bJumped)
            m_v3MoveDirection.y = -4;

        if (bIsGrounded)
        {
            m_Anim.SetBool("IsGrounded", true);
        }
        else
        {
            m_Anim.SetBool("IsGrounded", false);
        }

        m_Move.CheckControlsUpdate();
        v3UserInput = m_Move.GetUserInput();

        if (bIsGrounded)
        {
            Vector3 v3NewIn = Vector3.zero;

            Vector3 tangent = Vector3.Cross(m_v3GroundNormal, Vector3.forward);

            if (tangent.magnitude == 0)
                tangent = Vector3.Cross(m_v3GroundNormal, Vector3.up);


            if (tangent.y != 0)
            {
                //convert user input to new plane
                if (Vector3.Dot(v3UserInput, tangent) > 0)
                {
                    //v3UserInput.y += tangent.y;
                }
                else
                {
                    //v3UserInput.y -= tangent.y;
                }
            }

            Vector3 v3Result = Vector3.zero;
            float fComponent = Vector3.Dot(v3UserInput, m_v3GroundNormal);
            Vector3 v3ReverseNormal = m_v3GroundNormal * -1 * fComponent;
            v3Result = v3UserInput + v3ReverseNormal;


            //Debug.DrawLine(this.transform.position, this.transform.position + v3Result, Color.cyan, 1);

            v3UserInput = v3Result.normalized;
            v3UserInput *= (fWalkSpeed - (fSpeedDampening * GetMoveDirFlattened().magnitude)) * Time.deltaTime;
            if ((GetMoveDirFlattened() + v3UserInput).magnitude > fMaxMoveSpeed)
            {
                v3NewIn = (1 - Vector3.Dot(v3UserInput.normalized, v3MoveDirFlattened.normalized)) * v3UserInput;
                Debug.DrawLine(this.transform.position + Vector3.up, this.transform.position + Vector3.up + v3NewIn, Color.blue, 0.4f);
                v3UserInput = v3NewIn;
                //v3MoveDirFlattened = v3MoveDirFlattened.normalized * fMaxMoveSpeed;
                //m_v3MoveDirection.x = v3MoveDirFlattened.x;
                //m_v3MoveDirection.z = v3MoveDirFlattened.z;
            }

            ApplyFriction(1);
            ApplyGravity();

        }
        else
        {
            v3UserInput *= (fAirSpeed - (fSpeedDampening * GetMoveDirFlattened().magnitude)) * Time.deltaTime;
            ApplyGravity();

            if ((GetMoveDirFlattened() + v3UserInput).magnitude > fMaxAirSpeed)
            {
                Vector3 v3NewIn;
                v3NewIn = (1 - Vector3.Dot(v3UserInput.normalized, v3MoveDirFlattened.normalized)) * v3UserInput;
                Debug.DrawLine(this.transform.position + Vector3.up, this.transform.position + Vector3.up + v3NewIn, Color.blue, 0.4f);
                v3UserInput = v3NewIn;
            }
            //ApplyAirDrag(1);
        }
        m_v3MoveDirection += v3UserInput;


        m_Controller.Move(m_v3MoveDirection * Time.deltaTime);
        bWasGrounded = m_Controller.isGrounded;
        RotateToDirection();
        CheckClearAnimationStates();
    }

    Vector3 GetMoveDirFlattened()
    {
        v3MoveDirFlattened = m_v3MoveDirection;
        v3MoveDirFlattened.y = 0;
        return v3MoveDirFlattened;
    }

    protected virtual void ApplyFriction(float fDampening)
    {
        if (m_v3MoveDirection.magnitude < 0.5f)
            return;

        m_v3MoveDirection.z *= Mathf.Pow(fFriction * fDampening, Time.deltaTime);
        m_v3MoveDirection.x *= Mathf.Pow(fFriction * fDampening, Time.deltaTime);
    }

    protected virtual void ApplyAirDrag(float fDampening)
    {
        m_v3MoveDirection.z *= Mathf.Pow(fFriction * fDampening, Time.deltaTime);
        m_v3MoveDirection.x *= Mathf.Pow(fFriction * fDampening, Time.deltaTime);
    }

    protected void ApplyGravity()
    {
        m_v3MoveDirection.y -= fGravity * Time.deltaTime;
    }

    protected void ApplyVerticalSlowOnGround()
    {
        if (m_v3MoveDirection.y < -4.0f)
            m_v3MoveDirection.y *= 0.5f;

        if (m_v3MoveDirection.y < -4.0f)
            m_v3MoveDirection.y = -4.0f;
    }

    public bool IsGrounded()
    {
        if (m_Controller.isGrounded)
        {
            //if (!bSliding)
            //    ApplyVerticalSlowOnGround();
            return true;
        }


        RaycastHit hit;
        if (Physics.Linecast(this.transform.position, this.transform.position + Vector3.up * -fGroundCheckDistance, out hit, m_GroundCheckMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(this.transform.position, this.transform.position + Vector3.up * -fGroundCheckDistance, Color.red, 4);
            //ApplyVerticalSlowOnGround();
            m_v3GroundNormal = hit.normal;
            return true;
        }

        return false;
    }

    Vector3 GetActiveCollisionNormals()
    {
        Vector3 v3Ret = Vector3.zero;

        Vector3 v3LeftDiagonal = (Vector3.down + transform.right * -1).normalized * fNormalCheckDistance;
        Vector3 v3RightDiagonal = (Vector3.down + transform.right).normalized * fNormalCheckDistance;
        Vector3 v3ForwardDiagonal = (Vector3.down + transform.forward).normalized * fNormalCheckDistance;
        Vector3 v3BackwardsDiagonal = (Vector3.down + transform.forward * -1).normalized * fNormalCheckDistance;

        Debug.DrawLine(this.transform.position + v3GroundCheckOffset, this.transform.position + v3GroundCheckOffset + Vector3.down * fNormalCheckDistance, Color.red, 3);
        Debug.DrawLine(this.transform.position + v3GroundCheckOffset, this.transform.position + v3GroundCheckOffset + v3LeftDiagonal, Color.green, 3);
        Debug.DrawLine(this.transform.position + v3GroundCheckOffset, this.transform.position + v3GroundCheckOffset + v3RightDiagonal, Color.cyan, 3);
        RaycastHit hitinfo;
        if (Physics.Raycast(this.transform.position + v3GroundCheckOffset, Vector3.down, out hitinfo, fNormalCheckDistance, 9999, QueryTriggerInteraction.UseGlobal))
        {
            v3Ret += hitinfo.normal;
        }
        if (Physics.Raycast(this.transform.position + v3GroundCheckOffset, v3LeftDiagonal, out hitinfo, fNormalCheckDistance, 9999, QueryTriggerInteraction.UseGlobal))
        {
            Debug.DrawLine(hitinfo.point, hitinfo.point + hitinfo.normal, Color.blue, 3);
            v3Ret += hitinfo.normal;
        }
        if (Physics.Raycast(this.transform.position + v3GroundCheckOffset, v3RightDiagonal, out hitinfo, fNormalCheckDistance, 9999, QueryTriggerInteraction.UseGlobal))
        {
            Debug.DrawLine(hitinfo.point, hitinfo.point + hitinfo.normal, Color.blue, 3);
            v3Ret += hitinfo.normal;
        }
        if (Physics.Raycast(this.transform.position + v3GroundCheckOffset, v3ForwardDiagonal, out hitinfo, fNormalCheckDistance, 9999, QueryTriggerInteraction.UseGlobal))
        {
            Debug.DrawLine(hitinfo.point, hitinfo.point + hitinfo.normal, Color.blue, 3);
            v3Ret += hitinfo.normal;
        }
        if (Physics.Raycast(this.transform.position + v3GroundCheckOffset, v3BackwardsDiagonal, out hitinfo, fNormalCheckDistance, 9999, QueryTriggerInteraction.UseGlobal))
        {
            Debug.DrawLine(hitinfo.point, hitinfo.point + hitinfo.normal, Color.blue, 3);
            v3Ret += hitinfo.normal;
        }

        return v3Ret.normalized;
    }

    public bool Jump()
    {
        if (IsGrounded())
        {
            m_v3MoveDirection.y = fJumpSpeed;
            m_Anim.SetBool("Jump", true);
            bJumped = true;
            fJumpTimer = 0;
            return true;
        }
        else
        {
            Vector3 v3WallNormal = GetActiveCollisionNormals();

            if (v3WallNormal == Vector3.zero)
            {
                return false;
            }
            else
            {
                m_v3MoveDirection += v3WallNormal * fJumpSpeed;
                m_v3MoveDirection.y += fJumpSpeed;
                return true;
            }
        }

    }

    public bool Leap()
    {
        float fJumpSpeedFull = (0.5f + (GameData.instance.iShungitecollected / 5f)) * fJumpSpeed;
        if (IsGrounded())
        {
            m_v3MoveDirection = transform.forward * fJumpSpeedFull;
            m_v3MoveDirection.y += Random.Range(0, fJumpSpeedFull / 1.5f);
            float fHeightDiff = m_goTarget.transform.position.y - this.transform.position.y;

            m_v3MoveDirection.y += Random.Range(-fHeightDiff / 2, fHeightDiff / 2);
            m_Anim.SetBool("Leap", true);
            bJumped = true;
            fJumpTimer = 0;
            m_Audio.PlayAttackAudioClip(true);
            return true;
        }
        else
        {
            Vector3 v3WallNormal = GetActiveCollisionNormals();

            if (v3WallNormal != Vector3.zero)
            {
                m_v3MoveDirection += v3WallNormal * fJumpSpeedFull;
                Vector3 v3DiffToTarget = m_goTarget.transform.position - this.transform.position;
                v3DiffToTarget.y = 0;
                v3DiffToTarget.Normalize();
                m_v3MoveDirection += v3DiffToTarget * (m_v3MoveDirection.magnitude / 2);
                m_v3MoveDirection.y = Random.Range(-fJumpSpeedFull / 2, fJumpSpeedFull / 2);
                m_Anim.SetBool("Leap", true);
                bJumped = true;
                fJumpTimer = 0;
                m_Audio.PlayAttackAudioClip(false);
                return true;
            }

            return false;
        }
        
    }

    void RotateToDirection()
    {

        Vector3 target = new Vector3(m_goTarget.transform.position.x, this.transform.position.y, m_goTarget.transform.position.z);
        transform.LookAt(target);

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Slope Handling
        if (bIsGrounded)
            HandleSlopeVertical(hit.normal);
        else
        {
            Vector3 v3Norm = GetActiveCollisionNormals();
            if (Mathf.Abs(v3Norm.y) < 1.1f)
            {
                m_v3WallNormal = v3Norm;
            }
            HandleBump(hit.normal);
            m_v3MoveDirection = v3PerpendicularVector;
        }
    }

    public void HandleSlopeVertical(Vector3 v3Normal)
    {
        //Get component along normal
        if (v3Normal.y > 0.1f)
        {
            m_v3GroundNormal = v3Normal;

            v3PerpendicularVector = Vector3.Dot(m_v3MoveDirection, v3Normal) * v3Normal;

            m_v3MoveDirection.y -= v3PerpendicularVector.y;
            m_v3MoveDirection.y -= 4f;

            Vector3 tangent = Vector3.Cross(v3Normal, Vector3.forward);

            if (tangent.magnitude == 0)
            {
                tangent = Vector3.Cross(v3Normal, Vector3.up);
            }
        }
    }

    void HandleBump(Vector3 v3Normal)
    {
        v3PerpendicularVector = m_v3MoveDirection - (Vector3.Dot(m_v3MoveDirection, v3Normal) * v3Normal);
    }

    void CheckClearAnimationStates()
    {
        if (fJumpTimer > fJumpMax && bJumped)
        {
            bJumped = false;
            m_Anim.SetBool("Jump", false);
            m_Anim.SetBool("Leap", false);

        }
        else if (bJumped)
        {
            fJumpTimer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Phys>())
        {
            m_Audio.PlayKillPlayerClip();
            m_Move.bActive = false;
            GameData.instance.ShowGameOverScreen();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
