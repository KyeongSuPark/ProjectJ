using UnityEngine;
using System.Collections;

public class JumppingState : PlayerState {
    private string m_CurrentInput;
    public JumppingState(Player _parent)
        : base(_parent)
    {
    }

    public override ePlayerState GetCode()
    {
        return ePlayerState.Jumpping;
    }

    public override void OnStateEnter(StateChangeEventArg _arg)
    {
        Log.Print(eLogFilter.AnimTrigger, "set anim trigger " + R.String.ANIM_TRIGGER_JUPPING);
        m_Animator.SetTrigger(R.String.ANIM_TRIGGER_JUPPING);
    }

    public override void Update()
    {
        //. 점프 중 한번 더 점프!! 이단 점프!
        if(m_Animator.IsInTransition(0) == false && m_Parent.IsFullJumpStack() == false)
        {
            if (Input.GetButtonDown(R.String.INPUT_JUMP))
            {
                m_Parent.ChangeState(ePlayerState.DoubleJump);
                return;
            }
            else if (Input.GetButtonDown(R.String.INPUT_LEFT_JUMP) && m_Parent.Lane != eLane.Left)
            {
                m_Parent.ChangeState(ePlayerState.LeftJump);
                return;
            }
            else if (Input.GetButtonDown(R.String.INPUT_RIGHT_JUMP) && m_Parent.Lane != eLane.Right)
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
}
