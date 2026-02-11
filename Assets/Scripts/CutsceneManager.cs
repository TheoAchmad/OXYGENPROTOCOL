using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    // --- DATA GAMBAR & CERITA ---
    [System.Serializable]
    public class SceneData
    {
        public Sprite gambarBackground; 
        [TextArea(3, 10)] 
        public string kalimatCerita;    
    }

    [Header("1. Komponen Intro (WAJIB DIISI)")]
    public GameObject panelIntro;        // Panel Hitam (Induk)
    public TextMeshProUGUI teksIntro;    // Teks "Singkat cerita..."
    public Image backgroundHitam;        // Gambar Panel Hitam (Untuk efek pudar)

    [Header("2. Komponen Cutscene (WAJIB DIISI)")]
    public Image layarVisual;            // Tempat Gambar Cutscene
    public TextMeshProUGUI teksDialog;   // Tempat Teks Cerita

    [Header("3. Pengaturan Data")]
    public SceneData[] daftarScene;      // Isi Jumlah Gambar di sini (misal: 5)
    public float kecepatanKetik = 0.05f; 
    public string namaSceneGame = "4_Gameplay"; 

    private int indexSekarang = 0;
    private bool sedangMengetik = false;
    private bool introSelesai = false;   // Pengunci agar tidak bisa klik saat intro

    void Start()
    {
        // Saat mulai, jangan langsung tampilkan gambar 1.
        // Tapi mainkan INTRO dulu.
        StartCoroutine(MainkanIntro());
    }

    // --- BAGIAN 1: ANIMASI INTRO ---
    IEnumerator MainkanIntro()
    {
        // 1. Siapkan Layar Hitam
        panelIntro.SetActive(true);
        
        // Pastikan background hitam pekat & teks transparan
        Color warnaBG = backgroundHitam.color; warnaBG.a = 1; backgroundHitam.color = warnaBG;
        SetAlphaTeks(teksIntro, 0); 

        // 2. Teks Pudar Masuk (Muncul)
        yield return FadeTeks(teksIntro, 0, 1, 2.0f); // Durasi 2 detik
        
        // 3. Diam sebentar (Baca teks)
        yield return new WaitForSeconds(1.5f);

        // 4. Teks Pudar Keluar (Hilang)
        yield return FadeTeks(teksIntro, 1, 0, 1.5f);

        // 5. Layar Hitam Memudar (Membuka Tirai)
        yield return FadeBackground(backgroundHitam, 1, 0, 2.0f);

        // 6. Selesai Intro -> Matikan Panel -> Mulai Cutscene
        panelIntro.SetActive(false);
        introSelesai = true;
        TampilkanScene(0); // Tampilkan Gambar 1
    }

    // --- BAGIAN 2: LOGIKA CUTSCENE ---
    public void KlikLanjut()
    {
        // Kalau Intro belum kelar, tombol gak berfungsi
        if (!introSelesai) return; 

        if (sedangMengetik)
        {
            // Kalau diklik saat ngetik -> Langsung selesai ngetik
            StopAllCoroutines();
            teksDialog.text = daftarScene[indexSekarang].kalimatCerita;
            sedangMengetik = false;
        }
        else
        {
            // Kalau diklik saat teks sudah diam -> Pindah Gambar
            if (indexSekarang < daftarScene.Length - 1)
            {
                indexSekarang++;
                TampilkanScene(indexSekarang);
            }
            else
            {
                // Kalau gambar habis -> Pindah ke Game
                Debug.Log("Masuk Game...");
                SceneManager.LoadScene(namaSceneGame);
            }
        }
    }

    void TampilkanScene(int index)
    {
        layarVisual.sprite = daftarScene[index].gambarBackground;
        StartCoroutine(EfekMengetik(daftarScene[index].kalimatCerita));
    }

    IEnumerator EfekMengetik(string kalimatLengkap)
    {
        sedangMengetik = true;
        teksDialog.text = ""; 
        foreach (char huruf in kalimatLengkap.ToCharArray())
        {
            teksDialog.text += huruf;
            yield return new WaitForSeconds(kecepatanKetik);
        }
        sedangMengetik = false;
    }

    // --- FUNGSI BANTUAN FADE (JANGAN DIUBAH) ---
    void SetAlphaTeks(TextMeshProUGUI teks, float alpha)
    {
        Color c = teks.color; c.a = alpha; teks.color = c;
    }

    IEnumerator FadeTeks(TextMeshProUGUI teks, float start, float end, float durasi)
    {
        float timer = 0;
        while (timer < durasi)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, timer / durasi);
            SetAlphaTeks(teks, alpha);
            yield return null;
        }
    }

    IEnumerator FadeBackground(Image bg, float start, float end, float durasi)
    {
        float timer = 0;
        Color c = bg.color;
        while (timer < durasi)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(start, end, timer / durasi);
            bg.color = c;
            yield return null;
        }
    }
}