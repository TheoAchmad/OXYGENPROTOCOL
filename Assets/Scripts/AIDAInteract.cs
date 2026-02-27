using UnityEngine;

public class AIDAInteract : MonoBehaviour
{
    private bool playerDekat = false;
    public GameObject tombolPrompt; // Drag UI "Press E" ke sini
    public AIDAManager aidaManager; // Drag script AIDAManager ke sini

    void Update()
    {
        // Hanya bisa interaksi jika player dekat DAN panel dialog sedang tidak terbuka
        if (playerDekat && Input.GetKeyDown(KeyCode.E) && !aidaManager.panelDialog.activeSelf)
        {
            aidaManager.TanyaAIDA();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerDekat = true;
            if (tombolPrompt != null) tombolPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerDekat = false;
            if (tombolPrompt != null) tombolPrompt.SetActive(false);
        }
    }
}