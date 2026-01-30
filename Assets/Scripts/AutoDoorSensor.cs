using UnityEngine;

public class AutoDoorSensor : MonoBehaviour
{
    [Header("Hubungkan Pintu")]
    public DoorController scriptPintu; // Pintu mana yang mau dibuka?

    private int jumlahOrang = 0; // Menghitung jika ada 2 orang (misal Player + Musuh)

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Deteksi Player (Pastikan Player Anda punya Tag "Player")
        if (col.CompareTag("Player"))
        {
            jumlahOrang++;
            // Suruh pintu membuka selamanya (sampai diperintah tutup)
            if(scriptPintu != null) scriptPintu.SetPintuOtomatis(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            jumlahOrang--;
            
            // Hanya tutup jika TIDAK ADA orang lagi di area sensor
            if (jumlahOrang <= 0)
            {
                jumlahOrang = 0; // Reset biar aman
                if(scriptPintu != null) scriptPintu.SetPintuOtomatis(false);
            }
        }
    }
}