using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Player : MonoBehaviour {

    private Rigidbody m_Rigidbody = null;
    private Animator m_Animator = null;     ///< 애니메이터
    private PlayerState m_State = null;     ///< 현재 상태
    private eLane m_eLane = eLane.Middle;   ///< 현재 플레이어가 있는 Lane

    private Dictionary<ePlayerState, PlayerState> m_StateCache = null;  ///< 플레이어 상태 캐쉬

    private Vector3[] m_InternalJumpPos;     ///< 점프 할 위치
    private bool m_Grounded = true;          ///< 지면에 있나?
    private Coroutine m_MoveSideStepRoutine; ///< 옆으로 점프 코루틴 변수 
    
    private float testJumpTime = 0.0f;

    public Transform[] m_JumpPos;            ///< 점프 할 위치 (반드시 세개여야 한다) - 이건 외부 셋팅용
    public float m_MoveSpeed = 3.0f;          ///< 이동속도

    public float m_JumpPower;                 ///< 점프 파워
    public float m_JumpDuration;              ///< 점프 시간

    public float m_SecondJumpPower;           ///< 두번째 점프 파워
    public float m_SecondJumpDuration;        ///< 두번째 점프 시간

    public float m_SideJumpCoeff = 1.25f;     ///< 사이드 점프 추가 계수

    public Stack<ePlayerState> m_JumpStack;  ///< 점프 스택
    public GUIText m_DebugInfo;              ///< 디버그 정보
    public Transform m_GroundChecker;        ///< 지면 체크 용 GameObject

	// Use this for initialization
	void Start () {

        m_MoveSideStepRoutine = null;
        m_JumpStack = new Stack<ePlayerState>();
        m_StateCache = new Dictionary<ePlayerState, PlayerState>();
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_InternalJumpPos = new Vector3[3];

        for (int i = 0; i < m_JumpPos.Length; ++i)
        {
            Transform posTransform = m_JumpPos[i];
            m_InternalJumpPos[i] = posTransform.position - transform.position;
        }

        //. 시작 부터 달리는 상태
        m_State = new RunState(this);
        m_StateCache.Add(m_State.GetCode(), m_State);

        DisableRagdoll();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_State != null && m_Animator.IsInTransition(0) == false)
        {
            if(m_State.FirstFrameFlag == false)
            {
                m_State.OnFirstFrame();
                m_State.FirstFrameFlag = true;
            }
            else
            {
                m_State.Update();
            }
        }

        //. 디버그 정보
        String debugText = "";
        AnimatorStateInfo state = m_Animator.GetCurrentAnimatorStateInfo(0);
        debugText += string.Format("Anim state :{0}\n", GetAnimStateName(state));
        debugText += string.Format("Player state :{0}\n", m_State.GetCode());
        debugText += string.Format("IsFullStack :{0}\n", IsFullJumpStack());
        debugText += string.Format("IsJumpping :{0}\n", IsJumping());
        m_DebugInfo.text = debugText;
	}

    string GetAnimStateName(AnimatorStateInfo _state)
    {
        if (_state.IsName(R.String.ANIM_TRIGGER_RUN))
            return R.String.ANIM_TRIGGER_RUN;
        else if (_state.IsName(R.String.ANIM_TRIGGER_JUMP_START))
            return R.String.ANIM_TRIGGER_JUMP_START;
        else if (_state.IsName(R.String.ANIM_TRIGGER_JUPPING))
            return R.String.ANIM_TRIGGER_JUPPING;
        else if (_state.IsName(R.String.ANIM_TRIGGER_LAND))
            return R.String.ANIM_TRIGGER_LAND;
        else if (_state.IsName(R.String.ANIM_TRIGGER_DOUBLE_JUMP))
            return R.String.ANIM_TRIGGER_DOUBLE_JUMP;
        else if (_state.IsName(R.String.ANIM_TRIGGER_LEFT_JUMP))
            return R.String.ANIM_TRIGGER_LEFT_JUMP;
        else if (_state.IsName(R.String.ANIM_TRIGGER_RIGHT_JUMP))
            return R.String.ANIM_TRIGGER_RIGHT_JUMP;

        return "None";
    }

    void FixedUpdate()
    {
        if (m_State != null)
            m_State.FixedUpdate();

        bool newGrounded = false;
        var colliders = Physics.OverlapSphere(m_GroundChecker.position, 0.1f);      //. Todo 지면 레이어 마스크 추가
        foreach (var collider in colliders)
        {
            if(collider.gameObject != gameObject)
            {
                newGrounded = true;
                break;
            }
        }

        if(m_Grounded != newGrounded)
            OnChangedGrounded(newGrounded);
    }

    void OnChangedGrounded(bool _newGrounded)
    {
        m_Grounded = _newGrounded;
        if(m_Grounded == true)
        {
            m_JumpStack.Clear();
        }
    }

    /// <summary>
    /// 점프 스택에 추가 
    /// </summary>
    void PushToJumpStack(ePlayerState _state)
    {
        //. 점프는 2단계 까지만
        if (IsFullJumpStack() == true)
            return;

        m_JumpStack.Push(_state);
        Log.Print(eLogFilter.JumpStack, string.Format("Push jump stack state[{0}], count[{1}]", _state, m_JumpStack.Count));
    }

    /// <summary>
    ///   점프 스택이 꽉 차있나?
    /// </summary>
    public bool IsFullJumpStack()
    {
        if (m_JumpStack.Count >= 2)
            return true;

        return false;
    }

    /// <summary>
    ///  최근 점프 상태 반환
    /// </summary>
    public ePlayerState PeekJumpStack()
    {
        return m_JumpStack.Peek();
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
    ///  taget 포지션으로 사이드 이동
    /// </summary>
    private IEnumerator MoveBySideStep(Vector3 _target, float _duration)
    {
        float time = 0.0f;
        Vector3 startPos = transform.position;
        float lerpCoeff = 0.0f;

        while(true)
        {
            if (lerpCoeff > 1.0f)
                break;

            Vector3 newPosition = transform.position;
            newPosition.x = Mathf.Lerp(startPos.x, _target.x, lerpCoeff);
            transform.position = newPosition;
            yield return null;

            time += Time.deltaTime;
            lerpCoeff = time / _duration;
        }

        Vector3 lastPosition = transform.position;
        lastPosition.x = _target.x;
        transform.position = lastPosition;
        Log.Print(eLogFilter.Jump, "end move side!!");
        yield break;
    }

    /// <summary>
    /// 점프 중인가?
    /// </summary>
    public bool IsJumping()
    {
        return !m_Grounded;
    }

    /// <summary>
    ///  Jump 외부용 함수들
    /// </summary>
    public void Jump()
    {
        if (IsFullJumpStack() == true)
            return;

        Vector3 target = GetJumpPosition(eLane.Middle);
        m_Rigidbody.AddForce(Vector3.up * GetJumpPower());
        PushToJumpStack(ePlayerState.JumpStart);
        Log.Print(eLogFilter.Jump, "normal jump lane(" + m_eLane + ")");
    }
    
    public void DoubleJump()
    {
        if (IsFullJumpStack() == true)
            return;

        Vector3 target = GetJumpPosition(eLane.Middle);
        m_Rigidbody.AddForce(Vector3.up * GetJumpPower());
        PushToJumpStack(ePlayerState.DoubleJump);
        Log.Print(eLogFilter.Jump, "double jump lane(" + m_eLane + ")");
    }

    public void LeftJump()
    {
        if (IsFullJumpStack() == true)
            return;

        //. 이미 좌측 레인에 있나?
        if (m_eLane == eLane.Left)
            return;

        //. 아니면 왼쪽 레인 타겟으로 점프
        Vector3 target = GetJumpPosition(--m_eLane);

        if (IsJumping() && m_MoveSideStepRoutine != null)
            StopCoroutine(m_MoveSideStepRoutine);

        m_Rigidbody.AddForce(Vector3.up * GetSideJumpPower());
        m_MoveSideStepRoutine = StartCoroutine(MoveBySideStep(target, GetJumpDuration()));
        PushToJumpStack(ePlayerState.LeftJump);
        Log.Print(eLogFilter.Jump, string.Format("left jump [lane:{0}], [power:{1}]", m_eLane, GetSideJumpPower()));
    }

    public void RightJump()
    {
        if (IsFullJumpStack() == true)
            return;

        //. 이미 우측 레인에 있나?
        if (m_eLane == eLane.Right)
            return;

        //. 아니면 우측 레인 타겟으로 점프
        Vector3 target = GetJumpPosition(++m_eLane);

        if (IsJumping() && m_MoveSideStepRoutine != null)
            StopCoroutine(m_MoveSideStepRoutine);

        m_Rigidbody.AddForce(Vector3.up * GetSideJumpPower());
        m_MoveSideStepRoutine = StartCoroutine(MoveBySideStep(target, GetJumpDuration()));
        PushToJumpStack(ePlayerState.RightJump);
        Log.Print(eLogFilter.Jump, string.Format("right jump [lane:{0}], [power:{1}]", m_eLane, GetSideJumpPower()));
    }

    /// <summary>
    /// 점프 위치 반환
    /// </summary>
    public Vector3 GetJumpPosition(eLane _lane)
    {
        int idx = (int)_lane;
        if (idx < 0 && idx >= m_InternalJumpPos.Length)
        {
            Log.PrintError(eLogFilter.Normal, "Player::GetJumpPosition() - invalid enum value(eLane) : " + _lane);
            return Vector3.zero;
        }

        return m_InternalJumpPos[idx];
    }

    /// <summary>
    /// 점프 파워 반환 (처음 점프와 두번째 점프를 구분해서 반환)
    /// </summary>
    private float GetJumpPower()
    {
        if (m_JumpStack.Count == 1)
            return m_SecondJumpPower;

        return m_JumpPower;
    }

    /// <summary>
    ///   옆으로 점프 할 때, 점프 파워 반환
    /// </summary>
    private float GetSideJumpPower()
    {
        if (m_JumpStack.Count == 1)
            return m_SecondJumpPower;

        return m_JumpPower * m_SideJumpCoeff;
    }

    /// <summary>
    ///   점프 시간 반환 (처음 점프와 두번째 점프를 구분해서 반환)
    /// </summary>
    private float GetJumpDuration()
    {
        if (m_JumpStack.Count == 1)
            return m_SecondJumpDuration;
        
        return m_JumpDuration;
    }
    
    /// 각종 getter
    public float MoveSpeed
    {
        get { return m_MoveSpeed; }
    }
    
    public eLane Lane
    {
        get { return m_eLane; }
    }

}
