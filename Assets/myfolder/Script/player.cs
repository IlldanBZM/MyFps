using System;
using UnityEngine;
public class player : MonoBehaviour
{
    [HideInInspector]
    public float m_mouserate;//鼠标灵敏度

    [SerializeField]
    private float m_gravity;
    [SerializeField]
    private float m_steptime;
    [SerializeField]
    private float m_jumpspeed;
    [SerializeField]
    private float m_movespeed;
    [SerializeField]
    private float m_crouchmovespeed;

    public Camera m_camera;
    private CharacterController m_player;

    public Transform m_eyes;//眼睛
    public AudioClip[] m_stepsound = new AudioClip[4];
    public AudioClip m_jumpsound;
    public AudioSource m_audio;

    private float m_y = 0;
    private float m_timer = 0;//用来计算播放走路音效的时间间隔

    void Start()
    {
        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        m_player = gameObject.GetComponent<CharacterController>();
        m_audio = gameObject.GetComponent<AudioSource>();

    }


    private void FixedUpdate()
    {
        //声音设置
        gameObject.GetComponent<AudioSource>().volume = datamanager.m_volume;

        playermove();

        float deltay = Input.GetAxis("Mouse X");
        float deltax = Input.GetAxis("Mouse Y");
        Cameramove(m_mouserate * deltax, m_mouserate * deltay);

        if (!GroundCheck())
        {
            m_y -= m_gravity * Time.deltaTime;
        }
    }

    #region 主角移动和脚步声音
    public void playermove()
    {

        m_timer += Time.deltaTime;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (GroundCheck())
        {
            if (Input.GetKeyDown(KeyCode.Space))//起跳
            {
                Jump();
                m_timer = -m_steptime / 2;//这个值是随意设置的，只是为了起跳时不播放走路音效
            }
        }

            if (m_player.velocity.magnitude != 0 && m_timer > m_steptime)
            {
                m_audio.PlayOneShot(m_stepsound[UnityEngine.Random.Range(0, 4)]);//PlayOneShot可以直接播放一次指定AudioClip
                m_timer = 0;
            }

        //移动
        m_player.Move(gameObject.transform.TransformDirection(new Vector3(x, m_y, z) * m_movespeed * Time.deltaTime));
    }

    public void Jump()
    {
        m_y = m_jumpspeed;
        m_audio.PlayOneShot(m_jumpsound);
    }
    #endregion

    #region 摄像头角度移动
    public void Cameramove(float deltax, float deltay)
    {
        //鼠标控制摄像头角度
        Vector3 angle = m_camera.transform.eulerAngles;
        angle.x -= deltax;
        angle.y += deltay;
        if (angle.x > 35 && angle.x < 90) angle.x = 35;
        if (angle.x < 325 && angle.x > 270) angle.x = 325;//控制上下视野

        m_camera.transform.eulerAngles = angle;
        //主角角度与摄像头角度同步
        angle.x = angle.z = 0;
        transform.eulerAngles = angle;
        //摄像机跟随眼睛
        m_camera.transform.position = m_eyes.transform.position;
    }
    #endregion

    //检测是否在地上
    private bool GroundCheck()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast
            (transform.position, m_player.radius, Vector3.down, out hitInfo,
             0.3f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            return true;
        else return false;
    }

    public void Relax()
    {
        m_movespeed /= 100;
    }
    public void Resume()
    {
        m_movespeed *= 100;
    }
}

