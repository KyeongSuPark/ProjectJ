using UnityEngine;
using System.Collections;

public class JumppingState : PlayerState {
    private string m_ReservedInput;
    public JumppingState(Player _parent)
        : base(_parent)
    {
        m_ReservedInput = string.Empty;
    }

    public override ePlayerState GetCode()
    {
        return ePlayerState.Jumpping;
    }

    public override void OnStateEnter(StateChangeEventArg _arg)
    {
        Log.Print(eLogFilter.AnimTrigger, "set anim trigger " + R.String.ANIM_TRIGGER_JUPPING);
        m_ReservedInput = string.Empty;
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        m_ReservedInput = string.Empty;
    }

    public override void OnFirstFrame()
    {
        m_Animator.SetTrigger(R.String.ANIM_TRIGGER_JUPPING);
    }

    public override void ReserveInput(string _input)
    {
        m_ReservedInput = _input;
    }

    public override void Update()
    {
        //. 점프 중 한번 더 점프!! 이단 점프!
        if(m_Animator.IsInTransition(0) == false && m_Parent.IsFullJumpStack() == false)
        {
            if (GetButtonDown(R.String.INPUT_JUMP))
            {
                m_Parent.ChangeState(ePlayerState.DoubleJump);
                return;
            }
            else if (GetButtonDown(R.String.INPUT_LEFT_JUMP) && m_Parent.Lane != eLane.Left)
            {
                m_Parent.ChangeState(ePlayerState.LeftJump);
                return;
            }
            else if (GetButtonDown(R.String.INPUT_RIGHT_JUMP) && m_Parent.Lane != eLane.Right)
            {
                m_Parent.ChangeState(ePlayerState.RightJump);
                return;
            }
        }
        
        //. 정점을 찍고 내려올때가 되면 랜딩
        if(m_Parent.IsJumping() == false || m_Rigidbody.velocity.y <= -0.001f)
        {
            m_Parent.ChangeState(ePlayerState.Land);
        }
    }

    public bool IsReservedInput(string _input)
    {
        return m_ReservedInput.Equals(_input, System.StringComparison.OrdinalIgnoreCase);
    }

    public bool GetButtonDown(string _input)
    {
        return (IsReservedInput(_input) || Input.GetButtonDown(_input));
    }
}
