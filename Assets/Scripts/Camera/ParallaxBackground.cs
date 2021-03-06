using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform followingTarget;
    [SerializeField, Range(0f,1f)] private float parallaxStrenght = 0.1f;
    [SerializeField] private bool disableVerticalParallax;
    private Vector3 targetPreviousPosition;
    
    
    private void Start()
    {
        if (!followingTarget)
        {
            followingTarget = Camera.main.transform;
        }

        targetPreviousPosition = followingTarget.position;
    }
    
    private void FixedUpdate()
    {
        var delta = followingTarget.position - targetPreviousPosition;

        if (disableVerticalParallax)
        {
            delta.y = 0;
        }

        targetPreviousPosition = followingTarget.position;

        transform.position += delta * parallaxStrenght;
    }
}
