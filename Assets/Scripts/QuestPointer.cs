using UnityEngine;

public class QuestPointer : MonoBehaviour
{
    [Header("Target Navigasi")]
    public Transform targetSaatIni; 
    public Transform playerTransform; 

    [Header("Setting Orbit")]
    public float radiusOrbit = 2.0f; 
    public float offsetRotasi = -90f; 

    [Header("Setting Hilang (BARU)")]
    public float jarakHilang = 3.0f; // Jika jarak kurang dari ini, panah hilang

    private SpriteRenderer spritePanah;

    void Start()
    {
        spritePanah = GetComponent<SpriteRenderer>();
        if (playerTransform == null)
        {
            // Cari player otomatis jika lupa diisi
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
        }
    }

    void LateUpdate()
    {
        // 1. Cek Data
        if (targetSaatIni == null || playerTransform == null)
        {
            if (spritePanah != null) spritePanah.enabled = false;
            return;
        }

        // 2. HITUNG JARAK (Player ke Terminal)
        float jarakKeTarget = Vector3.Distance(playerTransform.position, targetSaatIni.position);

        // 3. LOGIKA SEMBUNYI / MUNCUL
        if (jarakKeTarget < jarakHilang)
        {
            // Jika sudah dekat, sembunyikan panah
            if (spritePanah != null) spritePanah.enabled = false;
        }
        else
        {
            // Jika masih jauh, munculkan panah & jalankan orbit
            if (spritePanah != null) spritePanah.enabled = true;
            
            UpdatePosisiDanRotasi();
        }
    }

    void UpdatePosisiDanRotasi()
    {
        // --- A. LOGIKA ORBIT (Melingkar) ---
        Vector3 arahKeTarget = targetSaatIni.position - playerTransform.position;
        Vector3 posisiOrbit = playerTransform.position + (arahKeTarget.normalized * radiusOrbit);
        
        transform.position = new Vector3(posisiOrbit.x, posisiOrbit.y, transform.position.z);

        // --- B. LOGIKA ROTASI (Menunjuk) ---
        float sudut = Mathf.Atan2(arahKeTarget.y, arahKeTarget.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, sudut + offsetRotasi);
    }

    // Fungsi Ganti Target (Tetap sama)
    public void GantiTarget(Transform targetBaru)
    {
        targetSaatIni = targetBaru;
    }

    public void SembunyikanPanah()
    {
        targetSaatIni = null;
    }
}