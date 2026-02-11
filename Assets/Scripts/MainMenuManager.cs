using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void TekanPlay()
    {
        // Pesan ini akan muncul di Console bawah kalau tombol berhasil dipencet
        Debug.Log("TOMBOL DITEKAN! MENCOBA LOAD SCENE..."); 
        
        SceneManager.LoadScene("2_Loading");
    }

    public void TekanKeluar()
    {
        Debug.Log("Keluar Game");
        Application.Quit();
    }
}