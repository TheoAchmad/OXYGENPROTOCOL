using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;

    [Header("UI Reference")]
    public TextMeshProUGUI objectiveText;

    private int completedMissions = 0;
    private int totalMissions = 5;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // --- TAMBAHKAN FUNGSI INI AGAR AIDAManager TIDAK ERROR ---
    public int GetProgress()
    {
        return completedMissions;
    }
    // ---------------------------------------------------------

    // Fungsi khusus untuk menambah progres misi
    public void AddMissionProgress()
    {
        completedMissions++;
        
        if (completedMissions > totalMissions) completedMissions = totalMissions;

        // Sesuai permintaanmu: "Misi 1/5 Kembali tanyakan ke AIDA..."
        objectiveText.text = "Misi " + completedMissions + "/" + totalMissions + 
                             "\n<size=80%>Kembali tanyakan ke AIDA</size>";
        
        Debug.Log("Progres: " + completedMissions);
    }

    public void UpdateObjective(string newInstruction)
    {
        if (objectiveText != null)
        {
            objectiveText.text = newInstruction;
        }
    }

    public void StartMissionCounter()
    {
        completedMissions = 0; 
        totalMissions = 5;
        objectiveText.text = "Kerjakan misi " + completedMissions + "/" + totalMissions;
    }

    public void ResetCounter()
    {
        completedMissions = 0; 
        totalMissions = 5;
        objectiveText.text = "Kerjakan misi 0/5"; 
        Debug.Log("Misi dimulai dari 0");
    }
}