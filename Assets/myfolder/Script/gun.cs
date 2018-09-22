using UnityEngine;

public class gun : MonoBehaviour
{
    //资源
    public Transform textruehit;//击中墙壁的图片
    public Transform spark;//击中物体的特效
    public Transform flashfire;//开火特效

    private RaycastHit m_hit; // 命中的对象
    private Transform m_gunmouth;//枪口的位置
    private Light m_light;//为了增强开火特效的点光源
    private Camera m_camera;//主相机
  
    [SerializeField]
    private int curbulletnum;
    public int Curbulletnum { get { return curbulletnum; } }
    [SerializeField]
    private int maxbulletnum;
    [SerializeField]
    private int storebulletnum;
    public int Storebulletnum { get { return storebulletnum; } }
    private float timer;//为了记录开火的时间间隔
    [SerializeField]
    private float AccuracyRange;//子弹的准度范围
    private float reloadtimer;//记录换弹的时间间隔
    [SerializeField]
    private int damage;
    [SerializeField]
    private int shootrange;
    [SerializeField]
    private float rate;

    public AudioClip ShootAudio;
    public AudioClip ReloadedAudio;
    //标记当前的动画状态
    private int m_animation;
    //动画
    public AnimationClip IdleAnimation;
    public AnimationClip ReloadAnimation;
    public AnimationClip ShootAnimation;
    //用来保存动画名
    string IdleS;
    string ReloadS;
    string ShootS;

    void Start()
    {
        m_gunmouth = GameObject.FindGameObjectWithTag("gunmouth").transform;
        m_light= GameObject.FindGameObjectWithTag("light").GetComponent<Light>();
        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        IdleS = IdleAnimation.name;
        ReloadS = ReloadAnimation.name;
        ShootS = ShootAnimation.name;
        m_light.transform.position = m_gunmouth.position;
        m_light.range = 0;

        m_animation = 0;
        reloadtimer = 0;
        timer = 0;
    }

    void Update()
    {
        SwicthAimation();

        gameObject.GetComponent<AudioSource>().volume = datamanager.m_volume;

        #region 换子弹、开火
        //主动换弹
        if (Input.GetKeyDown("r") && storebulletnum > 0 && curbulletnum != maxbulletnum && m_animation != 1)
        {
            m_animation = 1;
        }
        if (Input.GetButton("Fire1") && m_animation == 0)
        {
            if (curbulletnum == 0 && storebulletnum > 0) //自动换弹
            {
                m_animation = 1;
            }
            else if (curbulletnum > 0)
            {
                ShootFire();
                m_animation = 2;
            }
        }
        #endregion
    }
    //生成枪打到障碍物的特效
    void PlayHitEffect(Vector3 hitpos, Vector3 hitdirection)
    {
        //获得一个垂直于墙壁向上的方向
        Quaternion HitRotation = Quaternion.FromToRotation(Vector3.up, m_hit.normal);

        GameObject s = Instantiate(spark.gameObject, hitpos, HitRotation);
        if (!m_hit.collider.gameObject.CompareTag("Robot"))
        {
            GameObject h = Instantiate(textruehit.gameObject, hitpos, HitRotation);
            Destroy(h, 3);
        }
        Destroy(s, 1);
       
    }

    void ShootFire()
    {
        // 播放枪发射子弹时枪口的火焰
        GetComponent<AudioSource>().PlayOneShot(ShootAudio);
        //确定弹道方向
        Vector3 direction = m_camera.gameObject.transform.TransformDirection
            (Vector3.forward + new Vector3
            (Random.Range(-AccuracyRange, AccuracyRange), Random.Range(-AccuracyRange, AccuracyRange)));
        // 播放枪发射子弹时枪口的火焰
        GameObject f = Instantiate(flashfire.gameObject, m_gunmouth.transform.position, gameObject.transform.rotation);

        if (Physics.Raycast(m_camera.transform.position, direction, out m_hit, shootrange))
        {
            Debug.Log(m_hit.collider.gameObject.name);
            if (m_hit.collider.gameObject.CompareTag( "Robot"))
                m_hit.collider.GetComponentInParent<RobotController>().BeDamaged(damage);
            //RobotController会在后面创建，这里调用其中的BeDamaged方法
            PlayHitEffect(m_hit.point, direction);
        }
        Destroy(f, 0.04f);

        curbulletnum--;
    }

    void Reload()
    {
        if (storebulletnum > maxbulletnum - curbulletnum)
        {
            storebulletnum -= maxbulletnum - curbulletnum;
            curbulletnum = maxbulletnum;
        }
        else
        {
            curbulletnum += storebulletnum;
            storebulletnum = 0;
        }
    }//装弹计算


    void SwicthAimation()//转换动画
    {
        if (m_animation == 0)
        {
            GetComponent<Animation>().CrossFade(IdleS);
        }
        if (m_animation == 1)
        {
            GetComponent<Animation>().CrossFade(ReloadS);
            reloadtimer += Time.deltaTime;
        }
        if (m_animation == 2)
        {
            GetComponent<Animation>().Play(ShootS);
            m_light.range = 3;
            timer += Time.deltaTime;
        }

        if (reloadtimer >= GetComponent<Animation>()[ReloadS].length && m_animation == 1)// 当装弹动画结束时
        {
            Reload();
            m_animation = 0;
            reloadtimer = 0;
        }

        if ((timer > rate|| timer >= GetComponent<Animation>()[ShootS].length) && m_animation == 2)// 当射击动画结束时
        {
            m_light.range = 0;
            timer = 0;
            m_animation = 0;
        }
    }

   public void AddBullets()
    {
        storebulletnum += 2*maxbulletnum;
    }
};


