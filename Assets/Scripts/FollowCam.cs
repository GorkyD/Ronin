using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] private GameObject player;
    
        private Vector3 offset;
    
        private void Start()
        { 
            offset = transform.position - player.transform.position;
        }
    
        private void LateUpdate()
        {      
            if (player != null)
            {
                transform.position = player.transform.position + offset;
            }
        }
}
