using System.Collections.Generic;
using UnityEngine;

public abstract class FSMStateBase  {

    protected CharacterController m_npc;//机器人的碰撞器
    public Dictionary<Event,State>m_map=new Dictionary<Event, State>();//每个状态下发生某件事时都会转换至唯一其他状态

    public Vector3 m_destination;//目标点

    protected float m_movespeed;//移动速度
    protected float m_rotatespeed;//转身速度

    public abstract void  JudgeEventHappen(Transform player);//判断是否有事件发生
    public abstract void StateAction( Transform player);//保持状态的动作

    public void TurnAndMove()//转身并向目标点移动
    {
        //行走时有一定的偏差，并y轴与本身平行
        Vector3 des = new Vector3(m_destination.x+Random.Range(0,0.5f), m_npc.transform.position.y, m_destination.z+Random.Range(0, 0.5f));

        //身体平滑转向目标点
        Quaternion targetdirection = Quaternion.LookRotation(des- m_npc.transform.position);
        m_npc.transform.rotation = Quaternion.Slerp(m_npc.transform.rotation, targetdirection, Time.deltaTime * m_rotatespeed);
        //向目标点移动
        m_npc.Move(m_npc.gameObject.transform.forward*m_movespeed*Time.deltaTime);
    }
}
