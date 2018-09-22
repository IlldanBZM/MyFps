using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//游戏界面状态
enum PlayUiState
{
    BATTLE,
    END,
    SET
}

public class playui : MonoBehaviour
{

    private PlayUiState m_state;//当前界面状态

    private gun m_gun;
    private playerhp m_playerhp;
    private player m_player;
    private gamemanager m_manager;

    public GameObject m_hptext;//获取画布中的一个text
    public GameObject m_bulletText;//子弹数文本
    public GameObject m_snipertexture;//瞄准时加载的图片
    public GameObject m_aimtexture;
    public GameObject m_time;
    public GameObject m_endcanvas;
    public GameObject m_endtext;
    public GameObject m_playerstatetext;
    public GameObject m_hurtimage;
    public GameObject m_warningtext;
    public GameObject m_scoretext;
    public GameObject m_setcanvas;
    public GameObject m_bgm;
    public GameObject m_volumeslider;
    public GameObject m_mouseslider;

    private Color m_flashcolor; //记录受伤图片的色彩值
    public float m_flashspeed = 2;

    private float m_musictime = 0;//记录音乐播放位置
    private bool m_dead = false;
    private float m_finaltime = 0;

    void Start()
    {
        m_state = PlayUiState.BATTLE;

        m_player = FindObjectOfType<player>();
        m_playerhp = FindObjectOfType<playerhp>();
        m_manager = FindObjectOfType<gamemanager>();


        m_playerhp.diedelegate += OnDie;//添加委托事件

        
        m_volumeslider.GetComponent<Slider>().value = datamanager.m_volume;
        m_mouseslider.GetComponent<Slider>().value = datamanager.m_mosen;

        //记录当前设置好的受伤图片的颜色值
        m_flashcolor = m_hurtimage.GetComponent<Image>().color;
        //将受伤图片先设置为透明
        m_hurtimage.GetComponent<Image>().color = Color.clear;

        m_setcanvas.SetActive(false);
        m_endcanvas.SetActive(false);
        m_hurtimage.SetActive(true);
        m_playerstatetext.SetActive(false);
        m_warningtext.SetActive(false);

        //若是第一关 为玩家显示提示
        if (datamanager.M_gamelevel == 1)
        {
            StartCoroutine(FirstWarning());
        }

        Cursor.visible = false;//隐藏鼠标
    }

    void Update()
    {
        #region 战斗界面
        if (m_state == PlayUiState.BATTLE)
        {
            m_gun = FindObjectOfType<gun>();

            FreshHpText();
            FreshBulletText();
            ShowAimImage();
            ShowTheTime();

            //当玩家处于休息状态 显示提示
            if (playerstatemanager.m_state == PlayerState.RELAXE)
            {
                m_playerstatetext.GetComponent<Text>().text = "Relaxing...";
                m_playerstatetext.SetActive(true);
            }
            else m_playerstatetext.SetActive(false);

            //将受伤图片的颜色渐变为透明
            m_hurtimage.GetComponent<Image>().color =
                Color.Lerp
                (
                 m_hurtimage.GetComponent<Image>().color,
                 Color.clear,
                 m_flashspeed * Time.deltaTime
                );
            //设置鼠标灵敏度
            datamanager.m_mosen = m_mouseslider.GetComponent<Slider>().value;
            m_player.m_mouserate = datamanager.m_mosen * 5;
            //状态转换
            if (datamanager.M_gamelevel == datamanager.m_maxlevel || m_dead) m_state = PlayUiState.END;
            if (Input.GetKeyUp(KeyCode.Y)) m_state = PlayUiState.SET;

        }
        #endregion

        #region 结束界面
        if (m_state == PlayUiState.END)
        {
            Cursor.visible = true;//显示鼠标

            m_gun.enabled = false;

            m_player.enabled = false;

            m_endcanvas.SetActive(true);

            string score = "";

            if (datamanager.M_gamelevel == datamanager.m_maxlevel)//胜利
            {
                m_endtext.GetComponent<Text>().text = "Victory!";

                //记录游戏时间
                if (m_finaltime == 0)
                    m_finaltime = (int)datamanager.M_playtime;

                score =
                  "杀敌数：" + datamanager.M_gamescore +
                  "\n当前关卡:" + "final" +
                  "\n用时:" + m_finaltime + "s";
            }
            else if (m_dead)//死亡
            {
                m_endtext.GetComponent<Text>().text = "Game Over";

                m_playerhp.diedelegate -= OnDie;//释放委托事件
               
                //记录游戏时间
                if (m_finaltime == 0)
                    m_finaltime = (int)datamanager.M_playtime;

                score =
                    "杀敌数：" + datamanager.M_gamescore +
                    "\n当前关卡:" + datamanager.M_gamelevel +
                    "\n用时:" + m_finaltime + "s";
            }

            m_scoretext.GetComponent<Text>().text = score.ToString();
        }
        #endregion

        #region 设置界面
        if (m_state == PlayUiState.SET)
        {
            m_gun.enabled = false;
            //音乐暂停
            m_musictime = m_bgm.GetComponent<AudioSource>().time;
            m_bgm.GetComponent<AudioSource>().Pause();

            Cursor.visible = true;//显示鼠标

            Time.timeScale = 0;//时间暂停

            m_setcanvas.SetActive(true);
            //声音设置
            datamanager.m_volume = m_volumeslider.GetComponent<Slider>().value;
        }
        #endregion

    }


    void FreshHpText()
    {
        m_hptext.GetComponent<Text>().text = "HP:" + m_playerhp.M_hp.ToString();
    }

    void FreshBulletText()
    {

        m_bulletText.GetComponent<Text>().text = m_gun.Curbulletnum.ToString() + "/"
            + m_gun.Storebulletnum.ToString();
    }

    void ShowAimImage()
    {
        if (sniperscope.m_isaiming)
        {
            m_snipertexture.SetActive(true);
            m_aimtexture.SetActive(false);
        }
        else
        {
            m_snipertexture.SetActive(false);
            m_aimtexture.SetActive(true);
        }
    }

    void OnDie()
    {
        m_dead = true;
    }

    void ShowTheTime()
    {
        int time = (int)m_manager.M_WaitTime;
        if (time == 0)
            m_time.GetComponent<Text>().text = "The door is open!";
        else
            m_time.GetComponent<Text>().text = time.ToString();
    }

    public void HurtEffect()
    {
        m_hurtimage.GetComponent<Image>().color = m_flashcolor;
    }

    public void OnSetClose()
    {
        Cursor.visible = false;

        // 继续播放音乐
        m_bgm.GetComponent<AudioSource>().Play();
        m_bgm.GetComponent<AudioSource>().time = m_musictime;

        Time.timeScale = 1;//时间恢复正常

        m_setcanvas.SetActive(false);
        m_gun.enabled = true;
        
        m_state = PlayUiState.BATTLE;//转换状态

    }
    public void OnExit()
    {
        Application.Quit();
    }
    
    //显示警告，延迟消失
    IEnumerator FirstWarning()
    {
        m_warningtext.GetComponent<Text>().text = "Live and get out";
        m_warningtext.SetActive(true);

        yield return new WaitForSeconds(5f);

        m_warningtext.GetComponent<Text>().text = " ";
        m_warningtext.SetActive(false);
    }
}
