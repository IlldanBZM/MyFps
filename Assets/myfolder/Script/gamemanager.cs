using UnityEngine;
using UnityEngine.SceneManagement;
public class gamemanager : MonoBehaviour
{

    [SerializeField]
    private float m_WaitTime = 50;
    public float M_WaitTime
    {
        get { return m_WaitTime; }
    }

    private GameObject m_player;
    private GameObject m_camera;
    private GameObject m_door;

    public GameObject m_secondcamera;//机器人血条所望向的相机
    public Transform bornpos;



    void Start()
    {
        m_door = GameObject.FindGameObjectWithTag("door");
        m_camera = GameObject.FindGameObjectWithTag("MainCamera");
        m_player = GameObject.FindGameObjectWithTag("Player");

        //摄像头（含武器系统）和角色都不会随场景切换而更改
        DontDestroyOnLoad(m_camera);
        DontDestroyOnLoad(m_player);

        m_door.SetActive(true);
    }


    void Update()
    {
        if (m_door.activeInHierarchy)
            m_WaitTime -= Time.deltaTime;

        if (m_WaitTime <= 0)
        {
            m_door.SetActive(false);
            m_WaitTime = 0;
        }

        datamanager.CalcuTime();//计时

        if (datamanager.m_isnewlevel)
        {
            //将委托清空
            m_player.GetComponent<playerhp>().diedelegate = null;


            int i = datamanager.M_gamelevel;
            if (i == datamanager.m_maxlevel)
            {
                datamanager.m_isnewlevel = false;
                return;
            }
            else
            {
                SceneManager.LoadScene(i);
                m_player.transform.position = bornpos.position;
                datamanager.m_isnewlevel = false;
            }

        }
        //将两个相机同步
        if (m_secondcamera)
        {
            m_secondcamera.transform.position = m_camera.transform.position;
            m_secondcamera.transform.rotation = m_camera.transform.rotation;
            m_secondcamera.GetComponent<Camera>().fieldOfView = m_camera.GetComponent<Camera>().fieldOfView;
        }
    }
    //当游戏返回主菜单时执行的函数
    public void OnReturn()
    {
        playerstatemanager.m_state = PlayerState.ENERGY;
        Destroy(m_player);
        Destroy(m_camera);
        datamanager.ReturnMenu();
        SceneManager.LoadScene(0);
    }
}
