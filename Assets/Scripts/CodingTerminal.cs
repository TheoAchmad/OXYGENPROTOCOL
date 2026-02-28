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
    public string[] daftarKunciJawaban; 
    
    public bool bisaDiulang = false; 
    public bool adalahBagianMisi = true;
    public string syaratTeksObjective;

    [Header("4. Event")]
    public UnityEvent onCodingSuccess; 

    private bool playerDekat = false;
    private bool sudahSelesai = false;
    private bool sedangProses = false; // Mencegah spam enter

    [Header("5. Animasi Pohon")]
public Animator animatorPohon; // Tarik object pohon ke sini di Inspector
public int idMisiIni; // Isi di inspector (misal: 1 untuk misi pertama)

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
            string teksSekarang = ObjectiveManager.Instance.objectiveText.text;

            if (!adalahBagianMisi) {
                if (sudahSelesai) onCodingSuccess.Invoke(); 
                else BukaPanel(); 
            }
            else if (adalahBagianMisi && teksSekarang.Contains(syaratTeksObjective) && !sudahSelesai)
            {
                BukaPanel();
            }
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
            inputField.readOnly = false; 
            inputField.textComponent.color = warnaTeks; 
            inputField.text = kodeRusak; 
            inputField.ActivateInputField(); 
            inputField.MoveTextEnd(false); 

            // Gunakan onEndEdit agar fungsi terpanggil saat Enter ditekan
            inputField.onEndEdit.RemoveAllListeners();
            inputField.onEndEdit.AddListener(ProsesSubmit);
        }
        Time.timeScale = 0; 
    }

    public void TutupPanel()
    {
        if (inputField != null) inputField.onEndEdit.RemoveAllListeners();
        panelUI.SetActive(false); 
        Time.timeScale = 1; 
        
        if (playerDekat && tombolPrompt != null && (!sudahSelesai || bisaDiulang)) 
            tombolPrompt.SetActive(true);     
    }

    public void ProsesSubmit(string kodePemain)
    {
        // Pengecekan hanya berjalan jika tombol Enter ditekan
        if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter)) return;
        
        if (sudahSelesai || sedangProses) return;

        // Bersihkan input dan kunci dari spasi/tag untuk dibandingkan
        string inputUser = BersihkanLengkap(kodePemain);
        bool semuaBenar = true;

        foreach (string kunci in daftarKunciJawaban)
        {
            string kunciBersih = BersihkanLengkap(kunci);
            if (!inputUser.Contains(kunciBersih))
            {
                semuaBenar = false; 
                break;
            }
        }

        if (semuaBenar)
        {
            StartCoroutine(ProsesSukses());
        }
        else
        {
            StartCoroutine(ProsesGagal());
        }
    }

    // Fungsi pembersih yang konsisten
    string BersihkanLengkap(string teks)
    {
        if (string.IsNullOrEmpty(teks)) return "";
        string tanpaTag = Regex.Replace(teks, @"<[/]?[a-zA-Z]+[^>]*>", string.Empty);
        return Regex.Replace(tanpaTag, @"\s+", "");
    }

    IEnumerator ProsesSukses()
    {
        sedangProses = true;
        sudahSelesai = true;
        inputField.readOnly = true;

        inputField.textComponent.color = Color.yellow; 
        inputField.text = ">>> VERIFYING DATA... <<<";
        if(teksInstruksi != null) teksInstruksi.text = "PLEASE WAIT...";
        
        yield return new WaitForSecondsRealtime(1.5f); 

        inputField.textComponent.color = Color.green; 
        inputField.text = ">>> ACCESS GRANTED <<<"; 
        if(teksInstruksi != null) teksInstruksi.text = "SYSTEM UNLOCKED";

        yield return new WaitForSecondsRealtime(1.0f); 

        if (animatorPohon != null)
    {
        // Mengirimkan ID Misi ke parameter Animator
        animatorPohon.SetInteger("TahapMisi", idMisiIni);
    }

        TutupPanel();
        onCodingSuccess.Invoke(); 

        if (adalahBagianMisi)
        {
            ObjectiveManager.Instance.AddMissionProgress();
            AIDAManager aidaScript = Object.FindFirstObjectByType<AIDAManager>();
            if (aidaScript != null && aidaScript.questPointer != null)
                aidaScript.questPointer.GantiTarget(aidaScript.locAIDA);
            
            if (tombolPrompt != null) tombolPrompt.SetActive(false);
            if (gambarTerminal != null) gambarTerminal.color = Color.gray; 
            this.enabled = false; 
        }
        sedangProses = false;
    }

    IEnumerator ProsesGagal()
    {
        sedangProses = true;
        inputField.readOnly = true;

        // Visual efek merah (Ditolak)
        inputField.textComponent.color = Color.red;
        if(teksInstruksi != null) teksInstruksi.text = "ACCESS DENIED - WRONG CODE";

        yield return new WaitForSecondsRealtime(1.0f);

        // Kembalikan ke warna awal agar pemain bisa memperbaiki
        inputField.textComponent.color = warnaTeks;
        if(teksInstruksi != null) teksInstruksi.text = "FIX THE CODE TO PROCEED";
        inputField.readOnly = false;
        inputField.ActivateInputField();
        
        sedangProses = false;
    }

    private void OnTriggerEnter2D(Collider2D col) 
    { 
        if (col.CompareTag("Player")) { 
            playerDekat = true; 
            if (tombolPrompt != null && (!sudahSelesai || bisaDiulang)) tombolPrompt.SetActive(true); 
            if (gambarTerminal != null) gambarTerminal.color = warnaHighlight; 
        } 
    }

    private void OnTriggerExit2D(Collider2D col) 
    { 
        if (col.CompareTag("Player")) { 
            playerDekat = false; 
            if (tombolPrompt != null) tombolPrompt.SetActive(false); 
            if (gambarTerminal != null) gambarTerminal.color = warnaNormal; 
            if(panelUI.activeSelf) TutupPanel(); 
        } 
    }
}