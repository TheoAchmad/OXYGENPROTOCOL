using UnityEngine;
using UnityEngine.Rendering.Universal; // Wajib untuk Light 2D

public class SirenController : MonoBehaviour
{
    [Header("Komponen")]
    public Light2D lampuPlayer;     // Masukkan 'Spot Light 2D' milik Player di sini
    public AudioSource suaraSirine; // Masukkan Audio Source suara ngiung-ngiung

    [Header("Setting")]
    public float kecepatanKedip = 3.0f; 
    
    // Kita simpan warna asli (Putih) biar bisa balik normal nanti
    private Color warnaAsli = Color.white; 
    private Color warnaAlarm = Color.red;
    private bool alarmNyala = true;

    void Start()
    {
        // Ambil warna saat ini sebagai warna normal (biasanya putih)
        if (lampuPlayer != null) warnaAsli = lampuPlayer.color;

        if(suaraSirine != null) suaraSirine.Play();
    }

    void Update()
    {
        if (alarmNyala && lampuPlayer != null)
        {
            // LOGIKA WARNA: Berubah halus dari Putih ke Merah dan sebaliknya
            // Mathf.PingPong membuat angka naik turun 0 ke 1
            float t = Mathf.PingPong(Time.time * kecepatanKedip, 1);
            
            // Color.Lerp mencampur dua warna berdasarkan angka t
            lampuPlayer.color = Color.Lerp(warnaAsli, warnaAlarm, t);
        }
    }

    // Dipanggil Terminal saat sukses
    public void MatikanAlarm()
    {
        alarmNyala = false;

        // Matikan suara
        if(suaraSirine != null) suaraSirine.Stop();

        // KEMBALIKAN WARNA SENTER JADI PUTIH BERSIH
        if(lampuPlayer != null) 
        {
            lampuPlayer.color = warnaAsli;
        }
        
        Debug.Log("ALARM MATI - SENTER KEMBALI PUTIH");
    }
}