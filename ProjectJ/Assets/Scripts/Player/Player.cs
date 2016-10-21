using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    
    private PlayerState m_State = null;     ///< 현재 상태
    private Dictionary<ePlayerState, PlayerState> m_StateCache = null;  ///< 플레이어 상태 캐쉬
    
    public float m_MoveSpeed = 3.0f;            ///< 이동속도
    public float m_JumpPower = 200.0f;          ///< 점프 파워
    public float m_DoubleJumpPower = 200.0f;    ///< 더블 점프 파워
    public float m_SideJumpPower = 200.0f;      ///< 옆쪽 점프 파워

	// Use this for initialization
	void Start () {

        m_StateCache = new Dictionary<ePlayerState, PlayerState>();

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

}
