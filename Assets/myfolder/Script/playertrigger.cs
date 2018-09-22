using UnityEngine;

public class playertrigger : MonoBehaviour
{

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.CompareTag("Exit"))
        {
            Destroy(collider.gameObject);

            datamanager.AddGameLevel();
            datamanager.m_isnewlevel = true;

        }
        if (collider.gameObject.CompareTag("bullet"))
        {
            Destroy(collider.gameObject);

            gun one = FindObjectOfType<gun>();
            one.AddBullets();

        }
    }
}
