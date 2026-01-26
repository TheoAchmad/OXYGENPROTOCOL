using UnityEngine;
using TMPro;

public class CodingTerminal : MonoBehaviour
{
    [Header("Setting UI")]
    public GameObject panelUI;       
    public TMP_InputField inputField; 

    [Header("Setting Game")]
    public DoorController scriptPintu; 
    public string passwordBenar = "admin123"; 

    // Variabel private
    private bool playerDekat = false;

    // 1. FITUR: Pastikan semua bersih saat Game Mulai
    void Start()
    {
        // Matikan panel secara paksa biar tidak menghalangi
        if (panelUI != null && panelUI.activeSelf)
        {
            panelUI.SetActive(false);
        }
        
        // Bersihkan sisa-sisa kabel listener yang nyangkut
        if (inputField != null)
        {
            inputField.onEndEdit.RemoveAllListeners();
        }
    }

    void Update()
    {
        // LOGIKA BUKA (Hanya jika Player Dekat DAN Panel Mati)
        if (playerDekat && Input.GetKeyDown(KeyCode.E) && !panelUI.activeSelf)
        {
            BukaPanel();
        }
        
        // LOGIKA TUTUP DENGAN 'E' (PERBAIKAN UTAMA DI SINI)
        // Syarat ditambah: "&& playerDekat"
        // Artinya: Hanya terminal yang sedang diinjak player yang boleh menutup panel.
        // Terminal yang jauh tidak akan ikut campur.
        else if (playerDekat && panelUI.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            TutupPanel();
        }

        // LOGIKA TUTUP DENGAN 'ESC' (Global Cancel)
        // Tombol Esc boleh ditekan kapan saja untuk keluar
        if (panelUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            TutupPanel();
        }
    }

    void BukaPanel()
    {
        Debug.Log($"Membuka Terminal: {gameObject.name}");
        panelUI.SetActive(true);      
        inputField.text = "";         
        inputField.ActivateInputField(); 
        Time.timeScale = 0; 

        // Sambungkan kabel input field ke terminal INI SAJA
        inputField.onEndEdit.RemoveAllListeners(); // Putus kabel lama dulu
        inputField.onEndEdit.AddListener(CekPassword); // Sambung kabel baru
    }

    public void TutupPanel()
    {
        Debug.Log("Menutup Terminal");
        inputField.onEndEdit.RemoveAllListeners(); // Putus kabel
        panelUI.SetActive(false); 
        Time.timeScale = 1;       
    }

    public void CekPassword(string apaYangDiketik)
    {
        // Hanya proses jika panel ini yang sedang aktif
        if (!panelUI.activeSelf) return;

        string inputBersih = apaYangDiketik.Trim().Replace("\n", "").Replace("\r", "").ToUpper();
        string passwordBersih = passwordBenar.Trim().ToUpper();

        if (inputBersih == passwordBersih)
        {
            Debug.Log(">>> AKSES DITERIMA! <<<");
            TutupPanel(); 
            if(scriptPintu != null) scriptPintu.BukaPintu();
        }
        else
        {
            // Abaikan input kosong
            if (inputBersih == "") return;

            Debug.Log(">>> PASSWORD SALAH! <<<");
            inputField.text = ""; 
            inputField.ActivateInputField(); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            playerDekat = true;
            Debug.Log($"Player masuk area: {gameObject.name}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            playerDekat = false;
            // Tutup otomatis kalau player kabur
            if(panelUI.activeSelf) TutupPanel();
        }
    }
}