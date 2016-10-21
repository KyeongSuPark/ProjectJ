using UnityEngine;
using System.Collections;

public class JumpStartState : PlayerState
{
    public JumpStartState(Player _parent)
        : base(_parent)
    {
    }

    public override ePlayerState GetCode()
    {
        return ePlayerState.JumpStart;
    }

    public override void OnStateEnter(StateChangeEventArg _arg)
    {
        m_Animator.SetTrigger(R.String.ANIM_TRIGGER_JUMP_START);
    }

    public override void OnAnimationEvent(eAnimationEvent _eAnimEvent)
    {
        if(_eAnimEvent == eAnimationEvent.JumpStart)
        {
            m_Rigidbody.AddForce(m_Parent.transform.up * m_Parent.JumpPower);
        }
        else if(_eAnimEvent == eAnimationEvent.AnimationEnd)
        {
            m_Parent.ChangeState(ePlayerState.Jumpping);
        }
    }
}
