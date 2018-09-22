using UnityEngine;

public class Die : FSMStateBase
{
    FSMBase m_npcfsm;
    public Die(FSMBase aicontrollor)
    {
        m_npcfsm = aicontrollor;
        m_npc = m_npcfsm.GetComponentInParent<CharacterController>();
    }
    public override void JudgeEventHappen(Transform player)
    {
        
    }

    public override void StateAction(Transform player)
    {
        datamanager.AddGameScore();
        GameObject.Destroy( m_npc.gameObject);
    }
}
