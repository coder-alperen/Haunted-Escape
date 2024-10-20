using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public float rayDistance = 3.0f;  // Anahtarý ne kadar uzaklýktan alabileceðin mesafe
    private bool hasKey = false;

    void Update()
    {
        // Oyuncunun önünden bir ray gönder
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Eðer ray bir nesneye çarparsa
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // Eðer "Key" tag'ine sahip bir nesneye yaklaþýldýysa ve E tuþuna basýlýrsa
            if (hit.transform.CompareTag("Key") && Input.GetKeyDown(KeyCode.E))
            {
                PickupKey(hit.transform);  // Anahtarý al
            }
        }
    }

    void PickupKey(Transform key)
    {
        Debug.Log("Anahtar alýndý!");
        hasKey = true;  // Anahtar alýndý olarak iþaretle
        Destroy(key.gameObject);  // Anahtarý sahneden kaldýr
    }

    // Anahtarýn alýnýp alýnmadýðýný kontrol eden fonksiyon
    public bool GetKeyStatus()
    {
        return hasKey;
    }
}
