using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class CodingTerminal : MonoBehaviour
{
    [Header("Setting UI")]
    public GameObject panelUI;       
    public TMP_InputField inputField; 
    public TextMeshProUGUI teksInstruksi; 

    [Header("Setting Kode")]
    [TextArea(5, 20)] public string kodeRusak; 
    [TextArea(5, 20)] public string kodeBenar; 

    [Header("Setting Tipe Terminal")]
    public bool bisaDiulang = false; // BARU: Centang ini jika untuk Pintu

    [Header("AKSI SETELAH BERHASIL")]
    public UnityEvent onCodingSuccess; 

    private bool playerDekat = false;
    private bool sudahSelesai = false;

    void Start()
    {
        if (panelUI != null && panelUI.activeSelf) panelUI.SetActive(false);
        if (inputField != null) inputField.onValueChanged.RemoveAllListeners();
    }

    void Update()
    {
        // Logika Buka: Hanya jika belum selesai ATAU bisa diulang
        if (playerDekat && Input.GetKeyDown(KeyCode.E) && !panelUI.activeSelf)
        {
            if (!sudahSelesai || bisaDiulang) 
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
        panelUI.SetActive(true);
        
        // Reset input field setiap kali dibuka jika bisa diulang
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
    }

    public void CekKodeRealtime(string kodePemain)
    {
        // Jika sudah selesai DAN tidak bisa diulang, berhenti.
        if (sudahSelesai && !bisaDiulang) return;

        string pemainBersih = HapusSpasi(kodePemain);
        string kunciBersih = HapusSpasi(kodeBenar);

        if (pemainBersih == kunciBersih)
        {
            // Tandai selesai sementara
            sudahSelesai = true;
            
            inputField.text = ">>> KODE BERHASIL <<<";
            StartCoroutine(ProsesSukses());
        }
    }

    IEnumerator ProsesSukses()
    {
        yield return new WaitForSecondsRealtime(1.0f); 
        TutupPanel();
        
        onCodingSuccess.Invoke(); 

        // LOGIKA BARU:
        if (bisaDiulang)
        {
            // Jika terminal pintu, reset statusnya biar bisa dipakai lagi nanti
            sudahSelesai = false; 
        }
        else
        {
            // Jika terminal soal, matikan selamanya
            this.enabled = false; 
        }
    }

    string HapusSpasi(string teks)
    {
        return teks.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
    }

    private void OnTriggerEnter2D(Collider2D col) { if (col.CompareTag("Player")) playerDekat = true; }
    private void OnTriggerExit2D(Collider2D col) { if (col.CompareTag("Player")) { playerDekat = false; if(panelUI.activeSelf) TutupPanel(); } }
}