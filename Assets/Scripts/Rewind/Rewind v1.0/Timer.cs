using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour 
{
    private Text txt;
    private float timeLeft;
    private bool isRunning = true;
    
    public GameObject player;
    public float rewindTime = 8.0f;
    
    private void Start () 
    {
        txt = this.GetComponent<Text>();
        timeLeft = rewindTime;
    }
	

    
    private void RestartTime()
    {
        timeLeft = rewindTime;
        txt.color = Color.black;
        isRunning = true;
    }

    private void Update()
    {
        if (isRunning) 
        {
            timeLeft -= Time.deltaTime;
            txt.text = timeLeft.ToString ("0.00");
            if (timeLeft < 0) 
            {
                timeLeft = 0;
                txt.text = timeLeft.ToString ("0.00");
                txt.color = Color.red;
                isRunning = false;
                player.SendMessage ("StartRewind");
            }
        }
    }

}