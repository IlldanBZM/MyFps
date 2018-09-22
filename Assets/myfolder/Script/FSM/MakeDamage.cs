using UnityEngine;

public class MakeDamage : MonoBehaviour
{
    private GameObject m_player;

    [SerializeField]
    private int m_damage;
    private bool m_hasdamaged=false;
    private Animator m_animator;
    private AudioSource m_audio;
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_animator = gameObject.GetComponent<Animator>();
        m_audio = gameObject.GetComponent<AudioSource>();
    }
    void Update()
    {
        //下载的进攻音效不是很完美 进行一个截取
        if (m_audio.time > 1) m_audio.Stop();

        //获取动画层 0 指Base Layer.
        AnimatorStateInfo info = m_animator.GetCurrentAnimatorStateInfo(0);

        //normalizedTime为规范化时间，当动画循环播放时该值不断累加（播放一次累加1）
        //下面这句表示每一次当攻击动画播放了十分之八时
        if (info.IsName("attack") && info.normalizedTime % 1 > 0.8f && !m_hasdamaged)
        {
            m_player.GetComponent<playerhp>().bedammaged(m_damage);
            m_audio.Play();
            m_hasdamaged = true;
        }
        if (info.IsName("attack") && info.normalizedTime % 1 < 0.8f && m_hasdamaged)
        {
            m_hasdamaged = false;
        }
    }
}
