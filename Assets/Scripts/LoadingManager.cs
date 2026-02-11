using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    [Header("UI Components")]
    public Slider sliderLoading;
    public TextMeshProUGUI teksPersen;
    public TextMeshProUGUI teksStatus;
    public RectTransform ikonLoading;

    [Header("Settings")]
    public string namaSceneTujuan = "3_Cutscene";
    public float durasiTarget = 4.0f; // Target waktu sekitar 4 detik

    // Kata-kata bervariasi
    private string[] kataKataLoading = new string[]
    {
        "Memanaskan Mesin...", 
        "Kalibrasi Sensor...", 
        "Mengisi Bahan Bakar...", 
        "Menghubungi Markas...",
        "Memuat Peta..."
    };

    void Start()
    {
        StartCoroutine(ProsesLoadingOrganik());
        if(teksStatus != null) StartCoroutine(GantiTeksOtomatis());
    }

    void Update()
    {
        // Putar ikon
        if (ikonLoading != null) ikonLoading.Rotate(0, 0, -300 * Time.deltaTime);
    }

    IEnumerator ProsesLoadingOrganik()
    {
        AsyncOperation operasi = SceneManager.LoadSceneAsync(namaSceneTujuan);
        operasi.allowSceneActivation = false;

        float timer = 0f;
        float progressVisual = 0f;

        // Loop sampai visual menyentuh 100%
        while (progressVisual < 1f)
        {
            // TRIK 1: RANDOM SPEED (Variasi)
            // Kadang cepat (1.5x), kadang lambat (0.5x), seolah mikir
            float kecepatanAcak = Random.Range(0.5f, 1.5f); 
            timer += Time.deltaTime * kecepatanAcak;

            // Hitung persentase waktu (0 s.d 1)
            float persenWaktu = Mathf.Clamp01(timer / durasiTarget);

            // TRIK 2: SMOOTH STEP (Anti-Kaku)
            // Rumus matematika ini membuat gerakan melengkung (Awal lambat -> Tengah Cepat -> Akhir Ngerem)
            progressVisual = Mathf.SmoothStep(0f, 1f, persenWaktu);

            // Update Slider
            sliderLoading.value = progressVisual;
            
            if (teksPersen != null) 
                teksPersen.text = Mathf.RoundToInt(progressVisual * 100) + "%";

            yield return null;
        }

        // Finishing
        sliderLoading.value = 1f;
        if(teksPersen != null) teksPersen.text = "100%";
        
        // Jeda sedikit di 100% biar enak dilihat
        yield return new WaitForSeconds(0.5f);

        // Pastikan scene asli siap
        while (operasi.progress < 0.9f) yield return null;
        
        operasi.allowSceneActivation = true;
    }

    IEnumerator GantiTeksOtomatis()
    {
        int index = 0;
        while (true)
        {
            // Ganti teks secara berurutan atau acak
            teksStatus.text = kataKataLoading[index];
            index = (index + 1) % kataKataLoading.Length; // Loop balik ke 0 kalau habis

            // Ganti kata setiap 1.5 detik
            yield return new WaitForSeconds(1.5f);
        }
    }
}