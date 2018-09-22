using UnityEngine;

public class sniperscope : MonoBehaviour
{
    [HideInInspector]
    public static bool m_isaiming;
    [HideInInspector]
    public static bool m_canaiming;

    [SerializeField]
    private float m_zoomlevel;
    [SerializeField]
    private float m_zoominspeed;
    [SerializeField]
    private float m_zoomoutspeed;

    private float m_initfov;//初始值

    void Start()
    {
        m_canaiming = true;
        m_initfov = Camera.main.fieldOfView;
    }
    void Update()
    {
        //如果条件允许
        if (gameObject && m_canaiming)
        {
            if (Input.GetMouseButton(1))
                ZoomView();
            else ZoomOut();
        }
        //如果正在瞄准，但当前条件不允许，回到正常状态
        if (m_isaiming && !m_canaiming)
        {
            Camera.main.fieldOfView = m_initfov;
            m_isaiming = false;
        }

    }

    private void ZoomView()
    {
        if (Camera.main.fieldOfView - (Time.deltaTime * m_zoominspeed) >= m_initfov / m_zoomlevel)
        {
            Camera.main.fieldOfView -= Time.deltaTime * m_zoominspeed;
        }
        m_isaiming = true;
    }
    private void ZoomOut()
    {
        if (Camera.main.fieldOfView + (Time.deltaTime * m_zoomoutspeed) <= m_initfov)
        {
            Camera.main.fieldOfView += Time.deltaTime * m_zoomoutspeed;
        }
        m_isaiming = false;
    }
}
