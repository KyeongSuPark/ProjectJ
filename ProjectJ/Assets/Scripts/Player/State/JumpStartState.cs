using UnityEngine;
using System.Collections;

public class JumpStartState : PlayerState
{
    private float m_jumpDuration = 0.0f;       ///< Jump 시간

    public JumpStartState(Player _parent)
        : base(_parent)
    {
        //. 점프 시간은 세개의 애니메이션의 총합
        m_jumpDuration += m_Parent.GetAnimationLength(R.String.ANIM_CLIP_JUMP_START);
        m_jumpDuration += m_Parent.GetAnimationLength(R.String.ANIM_CLIP_JUMP_START);
        m_jumpDuration += m_Parent.GetAnimationLength(R.String.ANIM_CLIP_JUMP_START);
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
            m_Parent.Jump(m_jumpDuration);
        }
        else if(_eAnimEvent == eAnimationEvent.AnimationEnd)
        {
            m_Parent.ChangeState(ePlayerState.Jumpping);
        }
    }
}
