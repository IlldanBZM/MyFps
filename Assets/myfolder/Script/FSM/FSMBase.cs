using System.Collections.Generic;
using UnityEngine;
public enum State
{
    PATROL,
    CHASE,
    ATTACK,
    DIE
}
public enum Event
{
    FINDPLAYER,
    NOHP,
    CANATTACKED,
    LOSEPLAYER,
}
public class FSMBase :MonoBehaviour {

    protected int m_hp=100;
    public float M_hp
    {
        get { return m_hp; }
    }

    protected int m_hpbefore;//记录之前的血量

    protected FSMStateBase m_curstate;//现在的状态
    public FSMStateBase M_curstate { get { return m_curstate; } }

    protected Dictionary<State,FSMStateBase>m_statmap=new Dictionary<State, FSMStateBase>();

    virtual public void ChangeState(Event eve) { }

    public void  BeDamaged(int damage)
    {
        m_hpbefore = m_hp;
        m_hp -= damage;
    }

    public bool JudgeDamaged()//判断是否有掉血
    {
        if (m_hp != m_hpbefore)
        {
            m_hpbefore = m_hp;
            return true;
        }
        else return false;
    }
}
