using UnityEngine;
public class Sensor_Player : MonoBehaviour 
{
    private int colCount = 0;
    private float disableTimer;
    private void OnEnable()
    {
        colCount = 0;
    }
    public bool State()
    {
        if (disableTimer > 0)
        {
            return false;
        }
        return colCount > 0;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        colCount++;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        colCount--;
    }
    private void Update()
    {
        disableTimer -= Time.deltaTime;
    }
    public void Disable(float duration)
    {
        disableTimer = duration;
    }
}
