using UnityEngine;
using UnityEngine.UI; // Wajib untuk mengakses Gambar UI
using UnityEngine.SceneManagement; // Wajib untuk Pindah Scene
using System.Collections;

public class SplashAnimator : MonoBehaviour
{
    [Header("Komponen UI")]
    public Image gambarLogo; // Masukkan Logo di sini

    [Header("Pengaturan Waktu (Total 10 Detik)")]
    public float durasiMuncul = 3.0f;  // Waktu fade-in (3 detik)
    public float durasiDiam = 4.0f;    // Waktu logo diam (4 detik)
    public float durasiHilang = 3.0f;  // Waktu fade-out (3 detik)
    
    [Header("Scene Tujuan")]
    public string namaSceneBerikutnya = "1_MainMenu";

    void Start()
    {
        // Mulai animasi segera setelah game jalan
        StartCoroutine(JalankanAnimasi());
    }

    IEnumerator JalankanAnimasi()
    {
        // 1. FADE IN (Muncul Perlahan)
        // Kita ubah Alpha dari 0 ke 1
        float timer = 0;
        while (timer < durasiMuncul)
        {
            timer += Time.deltaTime;
            float alpha = timer / durasiMuncul; // Menghitung persen (0.0 sampai 1.0)
            SetAlpha(alpha);
            yield return null; // Tunggu frame berikutnya
        }
        SetAlpha(1); // Pastikan alpha mentok di 1 (jelas)

        // 2. STAY (Diam menikmati logo)
        yield return new WaitForSeconds(durasiDiam);

        // 3. FADE OUT (Menghilang Perlahan)
        // Kita ubah Alpha dari 1 ke 0
        timer = 0;
        while (timer < durasiHilang)
        {
            timer += Time.deltaTime;
            float alpha = 1.0f - (timer / durasiHilang); // Kebalikan (1.0 sampai 0.0)
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(0); // Pastikan alpha mentok di 0 (hilang)

        // 4. PINDAH SCENE
        SceneManager.LoadScene(namaSceneBerikutnya);
    }

    // Fungsi bantuan untuk mengubah transparansi gambar
    void SetAlpha(float nilaiAlpha)
    {
        if (gambarLogo != null)
        {
            Color warnaSementara = gambarLogo.color;
            warnaSementara.a = nilaiAlpha;
            gambarLogo.color = warnaSementara;
        }
    }
}