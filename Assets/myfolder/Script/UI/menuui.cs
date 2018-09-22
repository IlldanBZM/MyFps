using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class menuui : MonoBehaviour {

    public GameObject m_setcanvas;
    public GameObject m_Authorcanvas;
    public GameObject m_volumeslider;

    private void Start()
    {
        m_setcanvas.SetActive(false);
        m_Authorcanvas.SetActive(false);

        m_volumeslider.GetComponent<Slider>().value = datamanager.m_volume;
    }
    private void Update()
    {
        //音量调节
        datamanager.m_volume = m_volumeslider.GetComponent<Slider>().value;
    }

    //下面的方法全都挂在不同的Button上
    public void OnStart()
    {
        SceneManager.LoadScene(1);
    }
   public void OnExit()
    {
        Application.Quit();
    }

    public void OnSetOpen()
    {
        m_setcanvas.SetActive(true);
    }
    public void OnSetClose()
    {
        m_setcanvas.SetActive(false);
    }
    public void OnAuthorOpen()
    {
        m_Authorcanvas.SetActive(true);
    }
    public void OnAuthorClose()
    {
        m_Authorcanvas.SetActive(false);
    }
}
