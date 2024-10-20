using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    public TMP_Text keyCountText;
    public float rayDistance = 3.0f;  // Kapıya veya anahtara ne kadar yakın olunabileceği mesafe
    private int keyCount = 0; // Oyuncunun kaç tane anahtarı var?
    private Transform lastDoorOpened; // son açılan kapıyı takip etmek için

    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        UpdateKeyCountUI();
    }

    void Update()
    {
        // Oyuncunun önünden bir ray gönder
        // Ray ray = new Ray(transform.position + transform.forward , transform.forward);
        // RaycastHit hit;

        Vector3 rayStartPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);  // ekranın orta noktası
        // Ray oluşumu
        Ray ray = characterController.playerCamera.GetComponent<Camera>().ScreenPointToRay(rayStartPos);
        // hit değişkeninde depola
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

        // Ray'in çarptığı bir nesne varsa
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // E�er "Key" tag'ine sahip bir nesneye yaklaşıldıysa ve E tuşuna basıldıysa
            if (hit.transform.CompareTag("Key") && Input.GetKeyDown(KeyCode.E))
            {
                PickupKey(hit.transform); // Anahtarı al
            }

            // Eğer "Door" tag'ine sahip bir nesneye yaklaşıldıysa ve E tuşuna basıldıysa
            else if (hit.transform.CompareTag("Door") && Input.GetKeyDown(KeyCode.E) && keyCount > 0)
            {
                if (lastDoorOpened != hit.transform)
                {
                    OpenDoor(hit.transform);
                    lastDoorOpened = hit.transform;
                    keyCount -= 1;
                }
            }

            else if (hit.transform.CompareTag("Final_Door") && Input.GetKeyDown(KeyCode.E) && keyCount == 5)
            {
                if (lastDoorOpened != hit.transform)
                {
                    OpenDoor(hit.transform);
                    lastDoorOpened = hit.transform;
                    keyCount -= 1;
                }
            }

            else if (hit.transform.CompareTag("Unlocked") && Input.GetKeyDown(KeyCode.E))
            {
                if (lastDoorOpened != hit.transform)  // Aynı kapıyı tekrar açma kontrolü
                {
                    OpenDoor(hit.transform);
                    lastDoorOpened = hit.transform;  // Son açılan kapıyı güncelle
                }
            }
        }
    }

    void PickupKey(Transform Key)
    {
        Debug.Log("Anahtar alındı!");
        keyCount += 1;  // Elindeki anahtar sayısı 1 arttı
        UpdateKeyCountUI();
        Destroy(Key.gameObject);  // Anahtarı sahneden kaldır
    }

    void OpenDoor(Transform door)
    {
        Debug.Log("Kapı açıldı!");
        door.Rotate(0, 90, 0);  // Kapıyı 90 derece döndürerek aç
    }

    void UpdateKeyCountUI()
    {
        keyCountText.text = "You have " + keyCount.ToString() + "Keys"; // Anahtar sayısını Text'e yaz
    }
}

