using UnityEngine;

public  delegate  void  onedelegate();

public class playerhp : MonoBehaviour
{
    private int m_hp = 100;
    public int M_hp { get { return m_hp; } }

    public onedelegate diedelegate;

    void Start()
    {
    }
    void Update()
    {
        if (m_hp <= 0)
        {
            if(diedelegate!=null)
                diedelegate();
        }
    }
    public void bedammaged(int dam)
    {
        FindObjectOfType<playui>().HurtEffect();
        m_hp -= dam;
    }
}
