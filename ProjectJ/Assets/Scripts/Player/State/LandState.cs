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
        Log.Print(eLogFilter.AnimTrigger, "set anim trigger " + R.String.ANIM_TRIGGER_LAND);
    }

    public override void OnFirstFrame()
    {
        m_Animator.SetTrigger(R.String.ANIM_TRIGGER_LAND);
    }

    public override void Update()
    {
        base.Update();
        //. 점프 중이 끝났으니, Run!
        if (m_Parent.IsJumping() == false)
        {
            m_Parent.ChangeState(ePlayerState.Run);
        }
    }
}
