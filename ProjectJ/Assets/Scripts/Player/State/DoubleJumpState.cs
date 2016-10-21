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
        m_Animator.SetTrigger(R.String.ANIM_TRIGGER_DOUBLE_JUMP);
        m_Rigidbody.AddRelativeForce(Vector3.up * m_Parent.DoubleJumpPower);
    }

    public override void Update()
    {
        if(m_Rigidbody.velocity.y <= 0.1f)
        {
            m_Parent.ChangeState(ePlayerState.Land);
        }
    }
}
