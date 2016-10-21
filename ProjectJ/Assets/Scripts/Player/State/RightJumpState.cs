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
        m_Animator.SetTrigger(R.String.ANIM_TRIGGER_RIGHT_JUMP);
        m_Rigidbody.AddForce(Vector3.right* m_Parent.SideJumpPower);
        m_Rigidbody.AddForce(Vector3.up * m_Parent.JumpPower);
    }

    public override void Update()
    {
        //. 정점을 찍고 내려올때가 되면 랜딩
        if (m_Rigidbody.velocity.y <= -0.5f)
        {
            m_Parent.ChangeState(ePlayerState.Land);
        }
    }

    
}
