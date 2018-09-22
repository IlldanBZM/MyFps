using UnityEngine;

public class Chase : FSMStateBase
{
    private FSMBase m_npcfsm;
    private float m_attackrange;
    private float m_chaserange;

    public Chase(float movespeed, float rotatespeed, float attackrange, float chaserange, FSMBase aicontrollor)
    {
        m_movespeed = movespeed;
        m_rotatespeed = rotatespeed;
        m_attackrange = attackrange;
        m_chaserange = chaserange;


        m_npcfsm = aicontrollor;
        m_npc = m_npcfsm.GetComponentInParent<CharacterController>();
    }



    public override void JudgeEventHappen(Transform player)
    {
        //攻击敌人
        if (Vector3.Distance(m_npc.transform.position, player.position) < m_attackrange&&
            player.GetComponent<playerhp>().M_hp > 0)
        {
            m_npcfsm.ChangeState(Event.CANATTACKED);
        }
        //受到伤害
        if (m_npcfsm.JudgeDamaged())
        {
            if (m_npcfsm.M_hp <= 0)
            {
                m_npcfsm.ChangeState(Event.NOHP);
            }

        }

        //丢失敌人
        if (Vector3.Distance(m_npc.transform.position, player.position) > m_chaserange||
            player.GetComponent<playerhp>().M_hp<=0)
        {
            m_npcfsm.ChangeState(Event.LOSEPLAYER);
        }

    }

    public override void StateAction(Transform player)
    {

        m_destination = player.transform.position;
        TurnAndMove();

        //动画逻辑
        AnimatorStateInfo info = m_npc.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (info.IsName("walk"))
        {
            m_npc.GetComponentInParent<Animator>().SetBool("IsRun", true);
        }
        if (info.IsName("attack"))
            m_npc.GetComponentInParent<Animator>().SetBool("IsAttack", false);
    }

}
