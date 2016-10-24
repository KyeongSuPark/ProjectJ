﻿using UnityEngine;
using System.Collections;

public class LeftJumpState : PlayerState {
    private float m_JumpDuration = 0.0f;
    public LeftJumpState(Player _parent)
        : base(_parent)
    {
        m_JumpDuration += m_Parent.GetAnimationLength(R.String.ANIM_CLIP_FLIP);
    }

    public override ePlayerState GetCode()
    {
        return ePlayerState.LeftJump;
    }

    public override void OnStateEnter(StateChangeEventArg _arg)
    {
        m_Animator.SetTrigger(R.String.ANIM_TRIGGER_LEFT_JUMP);
        //m_Rigidbody.AddForce(Vector3.left * m_Parent.SideJumpPower);
        //m_Rigidbody.AddForce(Vector3.up * m_Parent.JumpPower);
        m_Parent.LeftJump(m_JumpDuration);
    }

    public override void Update()
    {
        //. 정점을 찍고 내려올때가 되면 랜딩
        if (m_Rigidbody.velocity.y <= -0.5f)
        {
            m_Parent.ChangeState(ePlayerState.Land);
        }
    }

    public override void OnAnimationEvent(eAnimationEvent _eAnimEvent)
    {
        
    }
}
