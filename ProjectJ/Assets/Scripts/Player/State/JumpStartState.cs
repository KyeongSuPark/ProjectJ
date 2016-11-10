using UnityEngine;
using System.Collections;

public class JumpStartState : PlayerState
{
    private PlayerState m_PreState = null;      ///< 이전 상태
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
        Log.Print(eLogFilter.AnimTrigger, "set anim trigger " + R.String.ANIM_TRIGGER_JUMP_START);
        m_PreState = _arg.PreState;
    }

    public override void OnFirstFrame()
    {
        m_Animator.SetTrigger(R.String.ANIM_TRIGGER_JUMP_START);

        //. 이전 상태가 측면 점프라면 바로 점프하자.
        if (GetPreStateCode() == ePlayerState.LeftJump || GetPreStateCode() == ePlayerState.RightJump)
            m_Parent.Jump();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        m_PreState = null;
    }

    public override void OnAnimationEvent(eAnimationEvent _eAnimEvent)
    {
        if(_eAnimEvent == eAnimationEvent.JumpStart)
        {
            //. 이전 상태가 Run 일때만 여기서 점프
            if(GetPreStateCode() == ePlayerState.Run)
                m_Parent.Jump();
        }
        else if (_eAnimEvent == eAnimationEvent.AnimationEnd)
        {
            m_Parent.ChangeState(ePlayerState.Jumpping);
        }
    }

    public ePlayerState GetPreStateCode()
    {
        if (m_PreState == null)
            return ePlayerState.None;

        return m_PreState.GetCode();
    }
}
