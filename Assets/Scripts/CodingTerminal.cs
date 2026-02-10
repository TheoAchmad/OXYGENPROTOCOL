using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;
using System.Text.RegularExpressions;

public class CodingTerminal : MonoBehaviour
{
    [Header("1. UI Setting")]
    public GameObject panelUI;       
    public TMP_InputField inputField; 
    public TextMeshProUGUI teksInstruksi; 

    [Header("2. Tampilan")]
    public Color warnaTeks = Color.green; 
    public GameObject tombolPrompt; 
    public SpriteRenderer gambarTerminal; 
    public Color warnaHighlight = Color.white; 
    private Color warnaNormal; 

    [Header("3. Soal & Jawaban")]
    [TextArea(5, 20)] public string kodeRusak; 
    [TextArea(5, 20)] public string kodeBenar; // Isi: <20
    public bool bisaDiulang = false; 

    [Header("4. Event")]
    public UnityEvent onCodingSuccess; 

    private bool playerDekat = false;
    private bool sudahSelesai = false;

    void Start()
    {
        if (gambarTerminal != null) warnaNormal = gambarTerminal.color;
        if (panelUI != null && panelUI.activeSelf) panelUI.SetActive(false);
        if (tombolPrompt != null) tombolPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerDekat && Input.GetKeyDown(KeyCode.E) && !panelUI.activeSelf)
        {
            if (!sudahSelesai || bisaDiulang) BukaPanel();
        }
        else if (panelUI.activeSelf && (Input.GetKeyDown(KeyCode.Escape) || (playerDekat && Input.GetKeyDown(KeyCode.E))))
        {
            TutupPanel();
        }
    }

    void BukaPanel()
    {
        if (tombolPrompt != null) tombolPrompt.SetActive(false);
        panelUI.SetActive(true);
        
        if (inputField != null) 
        {
            inputField.readOnly = false; // Izinkan ketik lagi
            inputField.textComponent.color = warnaTeks; 
            inputField.text = kodeRusak; 
            inputField.ActivateInputField(); 
            inputField.MoveTextEnd(false); 

            inputField.onValueChanged.RemoveAllListeners();
            inputField.onValueChanged.AddListener(CekKodeRealtime);
        }
        Time.timeScale = 0; 
    }

    public void TutupPanel()
    {
        inputField.onValueChanged.RemoveAllListeners();
        panelUI.SetActive(false); 
        Time.timeScale = 1; 
        
        if (playerDekat && tombolPrompt != null && (!sudahSelesai || bisaDiulang)) 
            tombolPrompt.SetActive(true);     
    }

    public void CekKodeRealtime(string kodePemain)
    {
        if (sudahSelesai && !bisaDiulang) return;

        string inputBersih = BersihkanKode(kodePemain);
        string kunciBersih = BersihkanKode(kodeBenar);

        // Jika Benar...
        if (inputBersih.Contains(kunciBersih))
        {
            sudahSelesai = true;
            inputField.readOnly = true; // Kunci biar gak bisa diketik lagi saat loading
            
            // Jalankan Urutan Animasi Loading
            StartCoroutine(ProsesSukses());
        }
    }

    string BersihkanKode(string teks)
    {
        if (string.IsNullOrEmpty(teks)) return "";
        // Hapus Tag HTML tapi BIARKAN angka (biar <20 terbaca benar)
        string tanpaTag = Regex.Replace(teks, @"<[/]?[a-zA-Z]+[^>]*>", string.Empty);
        return Regex.Replace(tanpaTag, @"\s+", "");
    }

    // --- INI BAGIAN KERENNYA (LOADING -> SUKSES) ---
    IEnumerator ProsesSukses()
    {
        // TAHAP 1: PROCESSING / LOADING
        // Teks berubah jadi Kuning
        inputField.textComponent.color = Color.yellow; 
        inputField.text = ">>> VERIFYING DATA... <<<";
        if(teksInstruksi != null) teksInstruksi.text = "PLEASE WAIT...";
        
        // Tunggu 1.5 detik (Efek mikir)
        yield return new WaitForSecondsRealtime(1.5f); 

        // TAHAP 2: SUCCESS
        // Teks berubah jadi Hijau Neon
        inputField.textComponent.color = Color.green; 
        inputField.text = ">>> ACCESS GRANTED <<<"; 
        if(teksInstruksi != null) teksInstruksi.text = "SYSTEM UNLOCKED";

        // Tunggu 1 detik lagi biar pemain sempat baca "Success"
        yield return new WaitForSecondsRealtime(1.0f); 

        // TAHAP 3: SELESAI
        TutupPanel();
        onCodingSuccess.Invoke(); 

        if (bisaDiulang) sudahSelesai = false; 
        else
        {
            if (tombolPrompt != null) tombolPrompt.SetActive(false);
            if (gambarTerminal != null) gambarTerminal.color = Color.gray; 
            this.enabled = false; 
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col) 
    { 
        if (col.CompareTag("Player")) { playerDekat = true; if (tombolPrompt != null && (!sudahSelesai || bisaDiulang)) tombolPrompt.SetActive(true); if (gambarTerminal != null) gambarTerminal.color = warnaHighlight; } 
    }
    private void OnTriggerExit2D(Collider2D col) 
    { 
        if (col.CompareTag("Player")) { playerDekat = false; if (tombolPrompt != null) tombolPrompt.SetActive(false); if (gambarTerminal != null) gambarTerminal.color = warnaNormal; if(panelUI.activeSelf) TutupPanel(); } 
    }
}