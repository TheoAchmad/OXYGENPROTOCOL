using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class CodingTerminal : MonoBehaviour
{
    [Header("1. Setting UI Panel")]
    public GameObject panelUI;       
    public TMP_InputField inputField; 
    public TextMeshProUGUI teksInstruksi; 

    [Header("2. Setting Visual Interaksi (BARU)")]
    public GameObject tombolPrompt; // Masukkan objek 'Prompt_E' ke sini
    public SpriteRenderer gambarTerminal; // Masukkan gambar terminal itu sendiri
    public Color warnaHighlight = Color.white; // Warna saat dekat (Terang)
    private Color warnaNormal; // Menyimpan warna asli (Gelap/Normal)

    [Header("3. Setting Soal")]
    [TextArea(5, 20)] public string kodeRusak; 
    [TextArea(5, 20)] public string kodeBenar; 
    public bool bisaDiulang = false; 

    [Header("4. Aksi Sukses")]
    public UnityEvent onCodingSuccess; 

    // Status
    private bool playerDekat = false;
    private bool sudahSelesai = false;

    void Start()
    {
        // Simpan warna asli terminal saat game mulai
        if (gambarTerminal != null) warnaNormal = gambarTerminal.color;

        // Pastikan UI mati duluan
        if (panelUI != null && panelUI.activeSelf) panelUI.SetActive(false);
        if (tombolPrompt != null) tombolPrompt.SetActive(false);
    }

    void Update()
    {
        // Logika Buka Panel
        if (playerDekat && Input.GetKeyDown(KeyCode.E) && !panelUI.activeSelf)
        {
            if (!sudahSelesai || bisaDiulang) 
            {
                BukaPanel();
            }
        }
        // Logika Tutup Panel
        else if (panelUI.activeSelf && (Input.GetKeyDown(KeyCode.Escape) || (playerDekat && Input.GetKeyDown(KeyCode.E))))
        {
            TutupPanel();
        }
    }

    // --- VISUAL INTERAKSI (Highlight & Tombol) ---

    private void OnTriggerEnter2D(Collider2D col) 
    { 
        if (col.CompareTag("Player")) 
        { 
            playerDekat = true; 
            
            // 1. Munculkan Tombol E
            if (tombolPrompt != null && (!sudahSelesai || bisaDiulang)) 
                tombolPrompt.SetActive(true);

            // 2. Ubah Warna jadi Terang (Highlight)
            if (gambarTerminal != null) 
                gambarTerminal.color = warnaHighlight;
        } 
    }

    private void OnTriggerExit2D(Collider2D col) 
    { 
        if (col.CompareTag("Player")) 
        { 
            playerDekat = false; 
            
            // 1. Matikan Tombol E
            if (tombolPrompt != null) 
                tombolPrompt.SetActive(false);

            // 2. Kembalikan Warna Asli
            if (gambarTerminal != null) 
                gambarTerminal.color = warnaNormal;

            // Tutup panel jika pemain kabur kejauhan
            if(panelUI.activeSelf) TutupPanel(); 
        } 
    }

    // --- LOGIKA PANEL ---

    void BukaPanel()
    {
        // Sembunyikan prompt E saat panel terbuka biar tidak menghalangi
        if (tombolPrompt != null) tombolPrompt.SetActive(false);

        panelUI.SetActive(true);
        inputField.text = kodeRusak; 
        if (teksInstruksi != null) teksInstruksi.text = "/// AUTHENTICATION REQUIRED ///";
        inputField.ActivateInputField(); 
        inputField.MoveTextEnd(false); 
        Time.timeScale = 0; 
        
        inputField.onValueChanged.RemoveAllListeners();
        inputField.onValueChanged.AddListener(CekKodeRealtime);
    }

    public void TutupPanel()
    {
        inputField.onValueChanged.RemoveAllListeners();
        panelUI.SetActive(false); 
        Time.timeScale = 1;  
        
        // Munculkan lagi prompt E jika pemain masih di dekat situ
        if (playerDekat && tombolPrompt != null && (!sudahSelesai || bisaDiulang))
            tombolPrompt.SetActive(true);     
    }

    public void CekKodeRealtime(string kodePemain)
    {
        if (sudahSelesai && !bisaDiulang) return;

        string pemainBersih = HapusSpasi(kodePemain);
        string kunciBersih = HapusSpasi(kodeBenar);

        if (pemainBersih == kunciBersih)
        {
            sudahSelesai = true;
            inputField.text = ">>> ACCESS GRANTED <<<";
            StartCoroutine(ProsesSukses());
        }
    }

    IEnumerator ProsesSukses()
    {
        yield return new WaitForSecondsRealtime(1.0f); 
        TutupPanel();
        onCodingSuccess.Invoke(); 

        if (bisaDiulang)
        {
            sudahSelesai = false; // Reset
        }
        else
        {
            // Jika terminal sekali pakai selesai, matikan highlight selamanya
            if (tombolPrompt != null) tombolPrompt.SetActive(false);
            if (gambarTerminal != null) gambarTerminal.color = Color.gray; // Jadi gelap/mati
            this.enabled = false; 
        }
    }

    string HapusSpasi(string teks)
    {
        return teks.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
    }
}