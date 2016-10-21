using UnityEngine;
using System.Collections;

public class JumppingState : PlayerState {
    public JumppingState(Player _parent)
        : base(_parent)
    {
    }

    public override ePlayerState GetCode()
    {
        return ePlayerState.Jumpping;
    }

    public override void OnStateEnter(StateChangeEventArg _arg)
    {
        m_Animator.SetTrigger(R.String.ANIM_TRIGGER_JUPPING);
    }

    public override void Update()
    {
        //. 점프 중 한번 더 점프!! 이단 점프!
        if(Input.GetButtonDown(R.String.INPUT_JUMP))
        {
            m_Parent.ChangeState(ePlayerState.DoubleJump);
            return;
        }
        else if (Input.GetButtonDown(R.String.INPUT_LEFT_JUMP))
        {
            m_Parent.ChangeState(ePlayerState.LeftJump);
            return;
        }
        else if (Input.GetButtonDown(R.String.INPUT_RIGHT_JUMP))
        {
            m_Parent.ChangeState(ePlayerState.RightJump);
            return;
        }

        //. 정점을 찍고 내려올때가 되면 랜딩
        if(m_Rigidbody.velocity.y <= -0.5f)
        {
            m_Parent.ChangeState(ePlayerState.Land);
        }
    }
}
