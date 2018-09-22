using UnityEngine;

public class bgm : MonoBehaviour {	

	void Update ()
    {
        gameObject.GetComponent<AudioSource>().volume = datamanager.m_volume;
    }
}
