using UnityEngine;
using System.Collections;

public class RightJumpState : PlayerState {
    public RightJumpState(Player _parent)
        : base(_parent)
    {
    }

    public override ePlayerState GetCode()
    {
        return ePlayerState.RightJump;
    }

    public override void OnStateEnter(StateChangeEventArg _arg)
    {
        Log.Print(eLogFilter.AnimTrigger, "set anim trigger " + R.String.ANIM_TRIGGER_RIGHT_JUMP);
    }

    public override void OnFirstFrame()
    {
        m_Animator.SetTrigger(R.String.ANIM_TRIGGER_RIGHT_JUMP);
        m_Parent.RightJump();
    }

    public override void Update()
    {
        base.Update();

        //. 점프는 이단 점프 까지만 지원
        if(m_Animator.IsInTransition(0) == false && m_Parent.IsFullJumpStack() == false)
        {
            if (Input.GetButtonDown(R.String.INPUT_JUMP))
            {
                StateChangeEventArg arg = new StateChangeEventArg();
                arg.PreState = this;

                m_Parent.Jump();
                m_Parent.ChangeState(ePlayerState.Jumpping, arg);
                return;
            }
            else if (Input.GetButtonDown(R.String.INPUT_LEFT_JUMP) && m_Parent.Lane != eLane.Left)
            {
                m_Parent.ChangeState(ePlayerState.LeftJump);
                return;
            }
        }

        //. 정점을 찍고 내려올때가 되면 랜딩
        if (m_Parent.IsJumping() == false || m_Rigidbody.velocity.y <= -0.001f)
        {
            m_Parent.ChangeState(ePlayerState.Land);
        }
    }

    
}
