using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    [Header("Hubungkan di Inspector")]
    public Animator animPintu;      // Tarik objek Visual_Pintu ke sini
    public BoxCollider2D colliderPintu; // Tarik BoxCollider Pintu_Parent ke sini

    // Fungsi ini akan dipanggil oleh script Terminal nanti
    public void BukaPintu()
    {
        StartCoroutine(ProsesBukaTutup());
    }

    IEnumerator ProsesBukaTutup()
    {
        // 1. Membuka
        animPintu.SetBool("IsOpen", true); // Mainkan animasi buka
        
        yield return new WaitForSeconds(0.5f); // Tunggu setengah detik biar animasinya kelihatan gerak dulu
        colliderPintu.enabled = false; // Matikan collider biar player bisa lewat

        // 2. Menunggu
        yield return new WaitForSeconds(10f); // Tunggu 10 detik

        // 3. Menutup
        animPintu.SetBool("IsOpen", false); // Mainkan animasi tutup
        colliderPintu.enabled = true; // Nyalakan collider lagi (dikunci)
    }
}