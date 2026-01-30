using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class DoorController : MonoBehaviour
{
    [Header("Hubungkan di Inspector")]
    public Animator animPintu;      
    public BoxCollider2D colliderPintu; 
    public ShadowCaster2D bayanganPintu; 

    // --- LOGIKA UNTUK TERMINAL (PAKAI TIMER) ---
    public void BukaPintu()
    {
        StopAllCoroutines(); // Reset jika ada perintah lain
        StartCoroutine(ProsesBukaTutup());
    }

    IEnumerator ProsesBukaTutup()
    {
        GerakkanPintu(true); // Buka
        yield return new WaitForSeconds(5f); // Tunggu 5 detik
        GerakkanPintu(false); // Tutup
    }

    // --- LOGIKA UNTUK SENSOR OTOMATIS (TANPA TIMER) ---
    // Fungsi ini akan dipanggil oleh script sensor baru nanti
    public void SetPintuOtomatis(bool buka)
    {
        StopAllCoroutines(); // Batalkan timer terminal jika ada
        GerakkanPintu(buka);
    }

    // --- FUNGSI UTAMA PENGGERAK ---
    void GerakkanPintu(bool isOpening)
    {
        // 1. Mainkan Animasi
        if(animPintu != null) animPintu.SetBool("IsOpen", isOpening);

        // 2. Atur Fisik (Collider)
        // Kalau buka, collider MATI (bisa lewat). Kalau tutup, NYALA.
        if(colliderPintu != null) colliderPintu.enabled = !isOpening;

        // 3. Atur Bayangan
        if (bayanganPintu != null) bayanganPintu.enabled = !isOpening;
    }
}