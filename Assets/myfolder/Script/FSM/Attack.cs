using UnityEngine;

public class Attack : FSMStateBase
{
    FSMBase m_npcfsm;
    private float m_attackrange;

    public Attack(float attackrange,float rotatespeed,FSMBase aicontroller)
    {
        m_attackrange = attackrange;
        m_movespeed = 0;


        m_npcfsm = aicontroller;
        m_npc = m_npcfsm.GetComponentInParent<CharacterController>();

    }
    public override void JudgeEventHappen(Transform player)
    {
        //受到伤害
        if(m_npcfsm.JudgeDamaged())
        {
            if(m_npcfsm.M_hp<=0)
               m_npcfsm.ChangeState(Event.NOHP);
        }
        //丢失玩家
        if(Vector3.Distance(m_npc.transform.position, player.position) > m_attackrange||
             player.GetComponent<playerhp>().M_hp <= 0)
        {
            m_npcfsm.ChangeState(Event.LOSEPLAYER);
        }
    }

    public override void StateAction(Transform player)
    {
        m_destination = player.position;
        TurnAndMove();

        m_npc.GetComponentInParent<Animator>().SetBool("IsAttack", true);
    }
}
