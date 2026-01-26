using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float kecepatanGerak = 5f;
    public Rigidbody2D rb;
    public Animator anim; // Referensi ke Animator

    Vector2 inputGerakan;

    void Update()
    {
        // 1. Input
        inputGerakan.x = Input.GetAxisRaw("Horizontal");
        inputGerakan.y = Input.GetAxisRaw("Vertical");

        // 2. Logika Animasi Jalan
        // Jika x atau y tidak 0, berarti sedang bergerak
        if (inputGerakan.x != 0 || inputGerakan.y != 0)
        {
            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }

        // 3. Logika Hadap Kiri/Kanan (Flip)
        if (inputGerakan.x > 0) // Ke Kanan
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (inputGerakan.x < 0) // Ke Kiri
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void FixedUpdate()
    {
        // 4. Fisika Gerak
        rb.MovePosition(rb.position + inputGerakan.normalized * kecepatanGerak * Time.fixedDeltaTime);
    }
}