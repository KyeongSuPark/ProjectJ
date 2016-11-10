using UnityEngine;
using System.Collections;

public class DoubleJumpState : PlayerState {
    public DoubleJumpState(Player _parent)
        : base(_parent)
    {
    }

    public override ePlayerState GetCode()
    {
        return ePlayerState.DoubleJump;
    }

    public override void OnStateEnter(StateChangeEventArg _arg)
    {
        Log.Print(eLogFilter.AnimTrigger, "set anim trigger " + R.String.ANIM_TRIGGER_DOUBLE_JUMP);
        //m_Rigidbody.AddRelativeForce(Vector3.up * m_Parent.DoubleJumpPower);
    }

    public override void OnFirstFrame()
    {
        m_Animator.SetTrigger(R.String.ANIM_TRIGGER_DOUBLE_JUMP);
        m_Parent.DoubleJump();
    }

    public override void Update()
    {
        if(m_Parent.IsJumping() == false || m_Rigidbody.velocity.y <= -0.01f)
        {
            m_Parent.ChangeState(ePlayerState.Land);
        }
    }
}
