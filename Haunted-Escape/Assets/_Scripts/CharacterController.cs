using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float stamina = 100;
    public float sprintSpeed = 10f;
    public float moveSpeed = 5f; // Hareket h�z�
    public float mouseSensitivity = 100f; // Fare hassasiyeti
    public float jumpForce = 5f; // Z�plama kuvveti
    public Transform playerCamera; // Oyuncu kameras� (karakterin ba��na ekleyece�iz)
    public Animator animator; // Animator bile�eni
    public Rigidbody rb; // 3D Rigidbody (3D oyunlar i�in)
    public LayerMask groundMask; // Yerin hangi katmanda oldu�unu belirlemek i�in
    public Transform groundCheck; // Yer kontrol� i�in referans nokta
    public float groundDistance = 0.4f; // Yerden ne kadar uzakl�kta oldu�unu kontrol etmek i�in

    private Vector3 movement; // Hareket y�n�
    private float xRotation = 0f; // Yukar�-a�a�� bak��� kontrol etmek i�in
    private bool isGrounded; // Karakterin yerde olup olmad���n� kontrol etmek i�in
    private bool isRunning = false; // Ko�ma durumu

    void Start()
    {
        // Fareyi kilitle (oyuncunun ekrandaki fareyi g�rmemesi i�in)
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

        // Giri� al (WASD ya da ok tu�lar�)
        movement.x = Input.GetAxisRaw("Horizontal"); // Yatay hareket (A, D veya Sol, Sa� ok)
        movement.z = Input.GetAxisRaw("Vertical");   // �leri-geri hareket (W, S veya Yukar�, A�a�� ok)

        // Animator parametrelerini ayarla
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.z);
        animator.SetFloat("Speed", movement.sqrMagnitude); // Hareket h�z�n� al (0 ise duruyor)

        //ko�ma animasyonu
        animator.SetBool("isRunning", isRunning);

        // Fare hareketini al
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Yukar�-a�a�� bak�� s�n�rland�r�l�r (90 derece)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Kameray� yukar�-a�a�� d�nd�r
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Karakteri sa�a-sola d�nd�r
        transform.Rotate(Vector3.up * mouseX);

        // Yer kontrol�
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Z�plama kontrol� (Space tu�una bas�l�rsa ve yerdeyse)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Animator parametresine z�plama durumu ekle
        animator.SetBool("isJumping", !isGrounded);
    }

    void FixedUpdate()
    {
        // Hareket y�n�n� d�nya koordinatlar�na �evir
        Vector3 moveDirection = transform.right * movement.x + transform.forward * movement.z;

        // H�z belirle
        float currentSpeed = isRunning ? sprintSpeed : moveSpeed;

        // Karakteri hareket ettir
        rb.MovePosition(rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime);
    }

    // Z�plama fonksiyonu
    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Z�plama kuvvetini uygula
    }

    // G�rsel olarak yere temas�n� g�stermek i�in bir k�re �iz
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }


}
