using UnityEngine;
using System.Collections;

public class LandState : PlayerState {
    public LandState(Player _parent)
        : base(_parent)
    {
    }

    public override ePlayerState GetCode()
    {
        return ePlayerState.Land;
    }

    public override void OnStateEnter(StateChangeEventArg _arg)
    {
        m_Animator.SetTrigger(R.String.ANIM_TRIGGER_LAND);
    }

    public override void Update()
    {
        
    }

    public override void OnAnimationEvent(eAnimationEvent _eAnimEvent)
    {
        if(_eAnimEvent == eAnimationEvent.AnimationEnd)
        {
            m_Parent.ChangeState(ePlayerState.Run);
        }
    }
}
