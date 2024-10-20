using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public float rayDistance = 3.0f;  // Anahtar� ne kadar uzakl�ktan alabilece�in mesafe
    private bool hasKey = false;

    void Update()
    {
        // Oyuncunun �n�nden bir ray g�nder
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // E�er ray bir nesneye �arparsa
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // E�er "Key" tag'ine sahip bir nesneye yakla��ld�ysa ve E tu�una bas�l�rsa
            if (hit.transform.CompareTag("Key") && Input.GetKeyDown(KeyCode.E))
            {
                PickupKey(hit.transform);  // Anahtar� al
            }
        }
    }

    void PickupKey(Transform key)
    {
        Debug.Log("Anahtar al�nd�!");
        hasKey = true;  // Anahtar al�nd� olarak i�aretle
        Destroy(key.gameObject);  // Anahtar� sahneden kald�r
    }

    // Anahtar�n al�n�p al�nmad���n� kontrol eden fonksiyon
    public bool GetKeyStatus()
    {
        return hasKey;
    }
}
