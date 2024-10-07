using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float stamina = 100;
    public float sprintSpeed = 10f;
    public float moveSpeed = 5f; // Hareket hýzý
    public float mouseSensitivity = 100f; // Fare hassasiyeti
    public float jumpForce = 5f; // Zýplama kuvveti
    public Transform playerCamera; // Oyuncu kamerasý (karakterin baþýna ekleyeceðiz)
    public Animator animator; // Animator bileþeni
    public Rigidbody rb; // 3D Rigidbody (3D oyunlar için)
    public LayerMask groundMask; // Yerin hangi katmanda olduðunu belirlemek için
    public Transform groundCheck; // Yer kontrolü için referans nokta
    public float groundDistance = 0.4f; // Yerden ne kadar uzaklýkta olduðunu kontrol etmek için

    private Vector3 movement; // Hareket yönü
    private float xRotation = 0f; // Yukarý-aþaðý bakýþý kontrol etmek için
    private bool isGrounded; // Karakterin yerde olup olmadýðýný kontrol etmek için
    private bool isRunning = false; // Koþma durumu

    void Start()
    {
        // Fareyi kilitle (oyuncunun ekrandaki fareyi görmemesi için)
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        // Giriþ al (WASD ya da ok tuþlarý)
        movement.x = Input.GetAxisRaw("Horizontal"); // Yatay hareket (A, D veya Sol, Sað ok)
        movement.z = Input.GetAxisRaw("Vertical");   // Ýleri-geri hareket (W, S veya Yukarý, Aþaðý ok)

        // Animator parametrelerini ayarla
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.z);
        animator.SetFloat("Speed", movement.sqrMagnitude); // Hareket hýzýný al (0 ise duruyor)

        //koþma animasyonu
        animator.SetBool("isRunning", isRunning);

        // Fare hareketini al
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Yukarý-aþaðý bakýþ sýnýrlandýrýlýr (90 derece)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Kamerayý yukarý-aþaðý döndür
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Karakteri saða-sola döndür
        transform.Rotate(Vector3.up * mouseX);

        // Yer kontrolü
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Zýplama kontrolü (Space tuþuna basýlýrsa ve yerdeyse)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Animator parametresine zýplama durumu ekle
        animator.SetBool("isJumping", !isGrounded);
    }

    void FixedUpdate()
    {
        // Hareket yönünü dünya koordinatlarýna çevir
        Vector3 moveDirection = transform.right * movement.x + transform.forward * movement.z;

        // Hýz belirle
        float currentSpeed = isRunning ? sprintSpeed : moveSpeed;

        // Karakteri hareket ettir
        rb.MovePosition(rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime);
    }

    // Zýplama fonksiyonu
    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Zýplama kuvvetini uygula
    }

    // Görsel olarak yere temasýný göstermek için bir küre çiz
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }


}
