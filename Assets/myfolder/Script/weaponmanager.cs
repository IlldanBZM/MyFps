using UnityEngine;

public class weaponmanager : MonoBehaviour
{
    private int m_currentweapon=1;//现在的武器

    [HideInInspector]
    public bool m_canchangeweapon= true;//是否可以更换武器

    public GameObject m_weapon1;//weapon1 prefab
    public GameObject m_weapon2;//weapon2 prefab
    public GameObject m_weapon3;//weapon3 prefab

    public gun m_gun, m_subgun, m_sniperrifle;

    private playerhp m_playerhp;

    void Start()
    {
        m_playerhp = FindObjectOfType<playerhp>();

        m_playerhp.diedelegate += OnDie;//添加委托

        NullWeapons();

        m_weapon1.SetActive(true);//默认将武器1设置为可用
        m_weapon1.GetComponent<Animation>().Play("Gun_On");
    }

    void Update()
    {
        if (m_weapon3.activeSelf)//当使用狙击枪时
        {
            if (!sniperscope.m_isaiming)//当狙击枪没有瞄准时可以更换武器
                m_canchangeweapon = true;
            else m_canchangeweapon = false;

            if (m_sniperrifle.Curbulletnum == 0)//当狙击无弹时不可瞄准
                sniperscope.m_canaiming = false;
            else sniperscope.m_canaiming = true;
        }

        #region 更换武器
        //通过滑轮换武器
        if ((Input.GetAxis("Mouse ScrollWheel") < 0) & m_canchangeweapon == true)
        {
            Debug.Log("h");
            m_currentweapon -= 1;
            if (m_currentweapon > 3) m_currentweapon = 1;
            if (m_currentweapon < 1) m_currentweapon = 3;
            Switch();
        }
        if ((Input.GetAxis("Mouse ScrollWheel") > 0) & m_canchangeweapon == true)
        {
            m_currentweapon += 1;
            if (m_currentweapon > 3) m_currentweapon = 1;
            if (m_currentweapon < 1) m_currentweapon = 3;
            Switch();
        }

        //通过按键换武器
        if (Input.GetKeyDown("1") & m_canchangeweapon == true)
        {
            if (!m_gun.isActiveAndEnabled)
            {
                m_currentweapon = 1;
                Switch();
            }
        }
        if (Input.GetKeyDown("2") & m_canchangeweapon == true)
        {
            if (!m_subgun.isActiveAndEnabled)
            {
                m_currentweapon = 2;
                Switch();
            }
        }
        if (Input.GetKeyDown("3") & m_canchangeweapon == true)
        {
            if (!m_sniperrifle.isActiveAndEnabled)
            {
                m_currentweapon = 3;
                Switch();
            }
        }

        #endregion
    }

    public void NullWeapons()//隐藏所有武器
    {
        m_weapon1.SetActive(false);
        m_weapon2.SetActive(false);
        m_weapon3.SetActive(false);
    }

    public void Switch()// 转换武器
    {

        if (m_currentweapon == 1)
        {
            NullWeapons();
            m_weapon1.SetActive(true);//将武器1设置为可用
            m_weapon1.GetComponent<Animation>().Play("Gun_On");
        }

        if (m_currentweapon == 2)
        {
            NullWeapons();
            m_weapon2.SetActive(true);//将武器2设置为可用
            m_weapon2.GetComponent<Animation>().Play("SubGun_On");
        }

        if (m_currentweapon == 3)
        {
            NullWeapons();
            m_weapon3.SetActive(true);//将武器3设置为可用
            m_weapon3.GetComponent<Animation>().Play("SniperRifle_On");
        }
    }

    //死亡时所执行的函数
    public void OnDie()
    {
        GetComponent<Animation>().Play("Die");

        m_playerhp.diedelegate -= OnDie;//释放委托
    }
}
