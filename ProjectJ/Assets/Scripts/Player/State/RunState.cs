using UnityEngine;
using System.Collections;

public class RunState : PlayerState
{
    public RunState(Player _parent)
        : base(_parent)
    {
    }

    public override ePlayerState GetCode()
    {
        return ePlayerState.Run;
    }

    public override void OnStateEnter(StateChangeEventArg _arg) 
    {
        Log.Print(eLogFilter.AnimTrigger, "set anim trigger " + R.String.ANIM_TRIGGER_RUN);
        //. Todo : Movespeed에 따라 Anim 재생 속도 바꿔야 할듯..
    }

    public override void OnFirstFrame()
    {
        m_Animator.SetTrigger(R.String.ANIM_TRIGGER_RUN);
    }

    public override void Update()
    {
        if (m_Animator.IsInTransition(0) == false && m_Parent.IsJumping() == false) 
        {
            if (Input.GetButtonDown(R.String.INPUT_JUMP))
            {
                StateChangeEventArg arg = new StateChangeEventArg();
                arg.PreState = this;

                m_Parent.ChangeState(ePlayerState.JumpStart, arg);
            }
            else if (Input.GetButtonDown(R.String.INPUT_LEFT_JUMP) && m_Parent.Lane != eLane.Left)
            {
                m_Parent.ChangeState(ePlayerState.LeftJump);
            }
            else if (Input.GetButtonDown(R.String.INPUT_RIGHT_JUMP) && m_Parent.Lane != eLane.Right)
            {
                m_Parent.ChangeState(ePlayerState.RightJump);
            }
        }
    }
}
