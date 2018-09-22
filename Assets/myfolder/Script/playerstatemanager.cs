using UnityEngine;

//玩家的两种状态，休息状态和体力充沛状态
public enum PlayerState
{
    RELAXE, ENERGY
}
public class playerstatemanager : MonoBehaviour
{

    public static PlayerState m_state = PlayerState.ENERGY;

    [SerializeField]
    private float m_relaxtime = 4f;
    [SerializeField]
    private float m_energetictime = 20f;

    private float m_timer = 0;//用来计算时间

    private player m_player;//角色控制脚本的引用

    void Start()
    {
        m_player = gameObject.GetComponent<player>();

    }

    void Update()
    {
        m_timer += Time.deltaTime;

        if (m_state == PlayerState.ENERGY)
        {
            if (m_timer > m_energetictime)
            {
                m_player.Relax();
                m_state = PlayerState.RELAXE;
                m_timer = 0;
            }
        }
        if (m_state == PlayerState.RELAXE)
        {
            if (m_timer > m_relaxtime)
            {
                m_player.Resume();
                m_state = PlayerState.ENERGY;
                m_timer = 0;
            }
        }
    }
}
