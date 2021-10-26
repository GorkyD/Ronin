using System.Collections.Generic;
using UnityEngine;

public class RewindBehavior : MonoBehaviour 
{

    private List<Vector2> positionList = new List<Vector2>();
    private GameObject Player;
    private Rigidbody2D rb;
    public GameObject Timer;
    
    private void Start () 
    {
        InvokeRepeating ("LogProgress", 0.1f, 0.1f);
        Player = this.gameObject;
        rb = GetComponent<Rigidbody2D> ();
    }
    private void LogProgress()
    {

        positionList.Add (Player.transform.position);
        Debug.Log (positionList[positionList.Count-1]);
    }

    private void StartRewind()
    {
        CancelInvoke ();
        rb.isKinematic = true;
        InvokeRepeating ("RewindTime", 0.02f, 0.02f);
    }

    private void RewindTime()
    {
        if (positionList.Count > 0) 
        {
            Player.transform.position = positionList [positionList.Count - 1];
            rb.velocity = new Vector2 (0, 0);
            positionList.RemoveAt (positionList.Count - 1);
        } 
        else 
        {
            CancelInvoke ();
            Timer.SendMessage ("RestartTime");
            rb.isKinematic = false;
            positionList.Clear ();
            InvokeRepeating ("LogProgress", 0.25f, 0.25f);
        }
			
    }
			
}