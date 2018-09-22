using System.Collections.Generic;
using UnityEngine;

public class Patrol : FSMStateBase {
    private FSMBase m_npcfsm;
    private float m_patrolrange;
    private float m_discoverrange;
    private List<GameObject> m_patrolpoionts;//存储巡逻点的数组

    //构造函数
   public Patrol
   (List<GameObject>partrolpoints, float movespeed,float rotatespeed,
       float discoverrange,float patrolrange,FSMBase aicontrollor)
    {
        m_patrolpoionts = partrolpoints;
        m_destination = m_patrolpoionts[0].transform.position;
        m_movespeed = movespeed;
        m_rotatespeed = rotatespeed;
        m_discoverrange = discoverrange;
        m_patrolrange = patrolrange;

        m_npcfsm = aicontrollor;
        m_npc = m_npcfsm.GetComponentInParent<CharacterController>();
    }

    public void FindNextPoint()//找下一个巡逻点
    {
        int i = Random.Range(0, m_patrolpoionts.Count);//随机一个索引
        m_destination = m_patrolpoionts[i].transform.position;
    }

    public override void JudgeEventHappen(Transform player)
    {
        //发现敌人
       if(Vector3.Distance(m_npc.transform.position, player.position)<m_discoverrange&&
              player.GetComponent<playerhp>().M_hp >0)//如果在发现范围内
        {
            m_npcfsm.ChangeState(Event.FINDPLAYER);
        }
       if(m_npcfsm.JudgeDamaged())//如果受到伤害
        {
            if (m_npcfsm.M_hp > 0)
            {
                m_npcfsm.ChangeState(Event.FINDPLAYER);
            }
            else m_npcfsm.ChangeState(Event.NOHP);
        }
    }

    public override void StateAction( Transform player)
    {
        TurnAndMove();

        //到达一个巡逻点，找下一个点
        if (Vector3.Distance(m_npc.transform.position, m_destination) < m_patrolrange )
        {
            FindNextPoint();
        }
        m_npc.GetComponentInParent<Animator>().SetBool("IsRun", false);
    }
}
