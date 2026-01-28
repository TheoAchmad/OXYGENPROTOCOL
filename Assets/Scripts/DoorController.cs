using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal; // 1. WAJIB ADA: Biar bisa akses ShadowCaster2D

public class DoorController : MonoBehaviour
{
    [Header("Hubungkan di Inspector")]
    public Animator animPintu;      
    public BoxCollider2D colliderPintu; 
    public ShadowCaster2D bayanganPintu; // 2. BARU: Slot untuk Shadow Caster

    // Fungsi ini akan dipanggil oleh script Terminal nanti
    public void BukaPintu()
    {
        StartCoroutine(ProsesBukaTutup());
    }

    IEnumerator ProsesBukaTutup()
    {
        // === FASE MEMBUKA ===
        animPintu.SetBool("IsOpen", true); // Animasi TETAP SAMA (Tidak diubah)
        
        yield return new WaitForSeconds(0.5f); // Tunggu animasi gerak dulu
        
        colliderPintu.enabled = false; // Fisik pintu hilang
        
        // 3. BARU: Matikan bayangan bersamaan dengan fisik pintu
        if (bayanganPintu != null) bayanganPintu.enabled = false; 

        // === FASE MENUNGGU ===
        yield return new WaitForSeconds(5f); // Tunggu 5 detik

        // === FASE MENUTUP ===
        animPintu.SetBool("IsOpen", false); // Animasi TETAP SAMA (Tidak diubah)
        
        colliderPintu.enabled = true; // Fisik pintu kembali
        
        // 4. BARU: Nyalakan bayangan lagi saat pintu terkunci
        if (bayanganPintu != null) bayanganPintu.enabled = true; 
    }
}