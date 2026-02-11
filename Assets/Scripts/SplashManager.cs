using UnityEngine;
using UnityEngine.SceneManagement; // Wajib untuk pindah scene

public class SplashManager : MonoBehaviour
{
    public float durasi = 10f; // 10 Detik sesuai request

    void Start()
    {
        // Perintahkan pindah scene setelah 'durasi' selesai
        Invoke("PindahKeMenu", durasi);
    }

    void PindahKeMenu()
    {
        SceneManager.LoadScene("1_MainMenu");
    }
}