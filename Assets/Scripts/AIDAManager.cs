using UnityEngine;
using TMPro;
using System.Collections;

public class AIDAManager : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI teksInformasiAIDA; 
    public GameObject panelDialog; 
    public GameObject layarSentuhLanjut; 

    [Header("Player Reference")]
    public PlayerController player; 

    [Header("Settings")]
    public float kecepatanKetik = 0.05f;

    private Coroutine typingCoroutine;
    private bool sedangMengetik = false;

    [Header("Petunjuk Arah (Pointer)")]
public QuestPointer questPointer; // Drag objek Panah ke sini
public Transform locAIDA; 
public Transform locM1, locM2, locM3, locM4, locM5;

    public void TanyaAIDA()
    {
        if (panelDialog == null || teksInformasiAIDA == null) return;
        if (player != null) player.enabled = false;
        panelDialog.SetActive(true);

        int progress = ObjectiveManager.Instance.GetProgress(); 
        string dialogFull = "";

        // --- LOGIKA MISI BERTAHAP (0 sampai 5) ---

        if (progress == 0) {
            dialogFull = "AIDA: Hallo Hewri, Kapten dan Crew telah meninggalkan pesawat ini. Karena seluruh system Pesawat mengalami EROR dan oksigen pun menipis.\nCepat perbaiki penyiraman di Hidroponik!";
            ObjectiveManager.Instance.UpdateObjective("Misi 0/5\n>Pergi ke Sektor Hidroponik");
            if(questPointer != null) questPointer.GantiTarget(locM1);
        } 
        else if (progress == 1) {
            dialogFull = "AIDA: Bagus, air mengalir. Sekarang bersihkan filter udara di Sektor Penjernihan!";
            ObjectiveManager.Instance.UpdateObjective("Misi 1/5\n>Pergi ke Sektor Penjernihan");
            if(questPointer != null) questPointer.GantiTarget(locM2);
        }
        else if (progress == 2) {
            dialogFull = "AIDA: Udara mulai stabil. Namun, tekanan bahan bakar menurun. Cek Sektor Generator!";
            ObjectiveManager.Instance.UpdateObjective("Misi 2/5\n>Pergi ke Sektor Generator");
            if(questPointer != null) questPointer.GantiTarget(locM3);
        }
        else if (progress == 3) {
            dialogFull = "AIDA: Generator stabil, tapi radar kita terganggu magnetik. Kalibrasi ulang di Sektor Navigasi!";
            ObjectiveManager.Instance.UpdateObjective("Misi 3/5\n>Pergi ke Sektor Navigasi");
            if(questPointer != null) questPointer.GantiTarget(locM4);
        }
        else if (progress == 4) {
            dialogFull = "AIDA: Semua sistem hampir pulih! Terakhir, aktifkan protokol transmisi di Ruang Komunikasi!";
            ObjectiveManager.Instance.UpdateObjective("Misi 4/5\n>Pergi ke Ruang Komunikasi");
            if(questPointer != null) questPointer.GantiTarget(locM5);
        }
        else if (progress == 5) {
            dialogFull = "AIDA: Kerja bagus, Hewri! Semua sistem aktif. Kita siap untuk melompat ke kecepatan cahaya!";
            ObjectiveManager.Instance.UpdateObjective("Misi 5/5\n>Semua Sistem Aktif");
        }

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeWriterEffect(dialogFull));
    }

    IEnumerator TypeWriterEffect(string teksLengkap)
    {
        teksInformasiAIDA.text = "";
        sedangMengetik = true;
        
        foreach (char karakter in teksLengkap.ToCharArray())
        {
            teksInformasiAIDA.text += karakter;
            yield return new WaitForSecondsRealtime(kecepatanKetik); 
        }

        sedangMengetik = false;
        if(layarSentuhLanjut != null) layarSentuhLanjut.SetActive(true);
    }

    public void LanjutMisi()
    {
        if (sedangMengetik) return; 

        panelDialog.SetActive(false);
        if(layarSentuhLanjut != null) layarSentuhLanjut.SetActive(false);
        if (player != null) player.enabled = true;
    }
}