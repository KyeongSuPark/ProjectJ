using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour {

    private Animator m_Animator = null;     ///< 애니메이터
    private PlayerState m_State = null;     ///< 현재 상태
    private eLane m_eLane = eLane.Middle;   ///< 현재 플레이어가 있는 Lane

    private Dictionary<ePlayerState, PlayerState> m_StateCache = null;  ///< 플레이어 상태 캐쉬

    public Transform[] m_JumpPos;              ///< 점프 할 위치 (반드시 세개여야 한다)
    public float m_MoveSpeed = 3.0f;            ///< 이동속도
    public float m_JumpPower = 200.0f;          ///< 점프 파워
    public float m_DoubleJumpPower = 200.0f;    ///< 더블 점프 파워
    public float m_SideJumpPower = 200.0f;      ///< 옆쪽 점프 파워

	// Use this for initialization
	void Start () {

        m_StateCache = new Dictionary<ePlayerState, PlayerState>();
        m_Animator = GetComponent<Animator>();

        //. 시작 부터 달리는 상태
        m_State = new RunState(this);
        m_StateCache.Add(m_State.GetCode(), m_State);

        DisableRagdoll();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_State != null)
            m_State.Update();
	}

    void FixedUpdate()
    {
        if (m_State != null)
            m_State.FixedUpdate();
    }

    /// <summary>
    /// 상태 변경
    /// </summary>
    public void ChangeState(ePlayerState _eUnitState, StateChangeEventArg _arg = null)
    {
        if (m_State == null)
            return;

        if (m_State.GetCode() == _eUnitState)
            return;

        //. 캐쉬 된게 있는지 검사
        PlayerState newUnitState = null;
        if(m_StateCache.ContainsKey(_eUnitState))
        {
            newUnitState = m_StateCache[_eUnitState];
        }
        else
        {
            //. 없으면 새로 만들고 캐쉬
            newUnitState = CreateUnitState(_eUnitState);
            m_StateCache.Add(newUnitState.GetCode(), newUnitState);
        }

        m_State.OnStateExit();
        m_State = newUnitState;
        m_State.OnStateEnter(_arg);

        Log.Print(eLogFilter.State, "change state to " + _eUnitState);
    }

    /// <summary>
    /// 상태 생성 코드 
    /// 자식 클래스에서 스테이트 추가될게 있다면 여기서 변경
    /// </summary>
    public virtual PlayerState CreateUnitState(ePlayerState _eUnitState)
    {
        switch (_eUnitState)
        {
            case ePlayerState.Run: return new RunState(this);
            case ePlayerState.JumpStart: return new JumpStartState(this);
            case ePlayerState.Jumpping: return new JumppingState(this);
            case ePlayerState.DoubleJump: return new DoubleJumpState(this);
            case ePlayerState.Land: return new LandState(this);
            case ePlayerState.RightJump: return new RightJumpState(this);
            case ePlayerState.LeftJump: return new LeftJumpState(this);
        }

        Log.PrintError(eLogFilter.Normal, "invalid parameter (CreateUnitState)");
        return null;
    }

    /// <summary>
    /// 애니메이션 이벤트 핸들러
    /// </summary>
    /// <param name="_eAnimEvent">이벤트 종류</param>
    public void OnAnimationEvent(eAnimationEvent _eAnimEvent)
    {
        if (m_State != null)
            m_State.OnAnimationEvent(_eAnimEvent);
    }

    void DisableRagdoll()
    {
        foreach (var ragdoll in gameObject.GetComponentsInChildren<Rigidbody>())
        {
            Collider col = ragdoll.GetComponent<Collider>();
            if (col && col != this.GetComponent<Collider>())
            {
                col.enabled = false;
                ragdoll.isKinematic = true;
                ragdoll.mass = 0.01f;
            }
        }
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawLine();
    }

    /// <summary>
    /// 타겟 위치로 주어진 duration 만큼 Jump!
    /// </summary>
    /// <param name="_target">점프 타겟 위치</param>
    /// <param name="_duration">점프할 시간</param>
    private IEnumerator InternalJump(Vector3 _target, float _duration, float _startVelocity)
    {
        Log.Print(eLogFilter.Jump, System.String.Format("start jump target:{0} position:{1} velocity:{2}", _target, _duration, _startVelocity));

        Vector3 startPos = transform.position;
        float time = 0.0f;
        float interpCoeff = 0.0f; //. 보간 계수
        float velocityY = _startVelocity;
        float height = startPos.y;

        while(IsJumping())
        {
            interpCoeff = time / _duration;
            if(interpCoeff > 1.0f)
            {
                transform.position = _target;
                Log.Print(eLogFilter.Jump, "end jump!!");
                yield break;
            }

            Vector3 curPos = Vector3.Lerp(startPos, _target, interpCoeff);
            curPos.y = height;
            transform.position = curPos;

            //. wait until next frame
            yield return null;

            height += velocityY * Time.deltaTime;
            velocityY += Time.deltaTime * Physics.gravity.y;
            time += Time.deltaTime;
        }

        transform.position = _target;
        Log.Print(eLogFilter.Jump, "end jump!!");
        yield break;
    }

    /// <summary>
    /// 점프 중인가?
    /// </summary>
    public bool IsJumping()
    {
        switch(m_State.GetCode())
        {
            case ePlayerState.DoubleJump:
            case ePlayerState.LeftJump:
            case ePlayerState.RightJump:
            case ePlayerState.JumpStart:
            case ePlayerState.Jumpping:
            case ePlayerState.Land:
                return true;

        }
        return false;   
    }

    /// <summary>
    ///  Jump 외부용 함수들
    /// </summary>
    public void Jump(float _duration)
    {
        Vector3 target = GetJumpPosition(m_eLane);
        StartCoroutine(InternalJump(target, _duration, m_JumpPower));

        Log.Print(eLogFilter.Jump, "normal jump lane(" + m_eLane + ")");
    }
    
    public void DoubleJump(float _duration)
    {
        Vector3 target = GetJumpPosition(m_eLane);
        StartCoroutine(InternalJump(target, _duration, m_DoubleJumpPower));

        Log.Print(eLogFilter.Jump, "double jump lane(" + m_eLane + ")");
    }

    public void LeftJump(float _duration)
    {
        //. 이미 좌측 레인에 있나?
        if (m_eLane == eLane.Left)
            return;

        //. 아니면 왼쪽 레인 타겟으로 점프
        Vector3 target = GetJumpPosition(--m_eLane);
        StartCoroutine(InternalJump(target, _duration, m_SideJumpPower));

        Log.Print(eLogFilter.Jump, "left jump lane(" + m_eLane + ")");
    }

    public void RightJump(float _duration)
    {
        //. 이미 우측 레인에 있나?
        if (m_eLane == eLane.Right)
            return;

        //. 아니면 우측 레인 타겟으로 점프
        Vector3 target = GetJumpPosition(++m_eLane);
        StartCoroutine(InternalJump(target, _duration, m_SideJumpPower));

        Log.Print(eLogFilter.Jump, "right jump lane(" + m_eLane + ")");
    }

    /// <summary>
    /// 점프 위치 반환
    /// </summary>
    public Vector3 GetJumpPosition(eLane _lane)
    {
        int idx = (int)_lane;
        if(idx < 0 && idx >= m_JumpPos.Length)
        {
            Log.PrintError(eLogFilter.Normal, "Player::GetJumpPosition() - invalid enum value(eLane) : " + _lane);
            return Vector3.zero;
        }

        return m_JumpPos[idx].position;
    }

    /// <summary>
    ///   애니메이션 Clip 길이를 반환
    /// </summary>
    public float GetAnimationLength(string _animName)
    {
        AnimationClip animClip = m_Animator.runtimeAnimatorController.animationClips.First(x => x.name.Contains(_animName));
        if (animClip == null)
            return 0;

        return animClip.length;        
    }

    /// 각종 getter
    public float MoveSpeed
    {
        get { return m_MoveSpeed; }
    }

    public float JumpPower
    {
        get { return m_JumpPower; }
    }

    public float DoubleJumpPower
    {
        get { return m_DoubleJumpPower; }
    }

    public float SideJumpPower
    {
        get { return m_SideJumpPower; }
    }

    public eLane Lane
    {
        get { return m_eLane; }
    }

}
