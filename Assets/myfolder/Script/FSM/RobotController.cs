using System.Collections.Generic;
using UnityEngine;

public class RobotController : FSMBase
{
    public List<GameObject> m_patrolpostions = new List<GameObject>();
    [SerializeField]
    private float m_attackrange;
    [SerializeField]
    private float m_discoverrange;
    [SerializeField]
    private float m_chaserange;
    public float M_chaserange
    {
        set { m_chaserange = value; }
    }
    [SerializeField]
    private float m_patrolrange;
    [SerializeField]
    private float m_walkspeed;
    [SerializeField]
    private float m_runspeed;
    [SerializeField]
    private float m_rotatespeed;

    private GameObject m_player;

    void Start()
    {
        m_hpbefore = m_hp;

        FSMBase temp = this;

        m_player = GameObject.FindGameObjectWithTag("Player");

        //添加状态
        m_statmap.Add(State.PATROL, new Patrol
            (m_patrolpostions, m_walkspeed, m_rotatespeed, m_discoverrange, m_patrolrange, temp));
        m_statmap.Add(State.CHASE, new Chase
            (m_runspeed, m_rotatespeed, m_attackrange, m_chaserange, temp));
        m_statmap.Add(State.ATTACK, new Attack
            (m_attackrange, m_rotatespeed, temp));
        m_statmap.Add(State.DIE, new Die(temp));

        //初始化每一个状态的字典
        m_statmap[State.PATROL].m_map.Add(Event.FINDPLAYER, State.CHASE);
        m_statmap[State.PATROL].m_map.Add(Event.NOHP, State.DIE);
        m_statmap[State.CHASE].m_map.Add(Event.NOHP, State.DIE);
        m_statmap[State.CHASE].m_map.Add(Event.LOSEPLAYER, State.PATROL);
        m_statmap[State.CHASE].m_map.Add(Event.CANATTACKED, State.ATTACK);
        m_statmap[State.ATTACK].m_map.Add(Event.LOSEPLAYER, State.CHASE);
        m_statmap[State.ATTACK].m_map.Add(Event.NOHP, State.DIE);

        m_curstate = m_statmap[State.PATROL];
    }

    void FixedUpdate()
    {
        //设置机器人的音效的音量
        gameObject.GetComponent<AudioSource>().volume = datamanager.m_volume;

        m_curstate.JudgeEventHappen(m_player.transform);

        m_curstate.StateAction(m_player.transform);
    }
    //根据事件改变当前状态
    public override void ChangeState(Event eve)
    {
        State temp = m_curstate.m_map[eve];
        m_curstate = m_statmap[temp];
    }
}
