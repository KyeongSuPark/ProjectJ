using UnityEngine;
using System.Collections;

public class PlayerState{
   
    protected Player m_Parent = null;
    protected Rigidbody m_Rigidbody = null;
    protected Animator m_Animator = null;

    public PlayerState(Player _parent)
    {
        m_Parent = _parent;
        m_Animator = _parent.GetComponent<Animator>();
        m_Rigidbody = _parent.GetComponent<Rigidbody>();
        if (m_Animator == null || m_Rigidbody == null)
        {
            Log.PrintError(eLogFilter.Normal, "Animator or Rigidbody is null");
            Debug.Break();
        }
    }

    public virtual ePlayerState GetCode() 
    {
        return ePlayerState.None; 
    }

    public virtual void OnStateEnter(StateChangeEventArg _arg) { }
    public virtual void OnStateExit() { }
    //public virtual void PreUpdate() { }
    public virtual void Update() { }
    public virtual void PostUpdate() { }

    public virtual void FixedUpdate() { }

    public virtual void OnAnimationEvent(eAnimationEvent _eAnimEvent) { }
}
