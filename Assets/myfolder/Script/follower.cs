using UnityEngine;
using UnityEngine.UI;
public class follower : MonoBehaviour
{
    [SerializeField]
    private float m_offsety;
    public Transform m_robot;
    private float m_robotinithp;//机器人初始的hp

    private void Start()
    {
        m_robotinithp = m_robot.gameObject.GetComponent<RobotController>().M_hp;
    }
    private void Update()
    {
        //确定血条位置
        gameObject.transform.position = 
            new Vector3(m_robot.position.x, m_robot.position.y + m_offsety, m_robot.position.z);
        //根据实际情况扣血
        gameObject.GetComponent<Slider>().value=
            ((m_robot.gameObject.GetComponent<RobotController>().M_hp / m_robotinithp));
    }
}