using UnityEngine;

public class Spike : MonoBehaviour
{

    public AnimationClip animationClip;

    public void Start()
    {
        Destroy(gameObject, animationClip.length);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tail"))
        {
            Snake.DestroyTail(other.gameObject);
        }
    }
}
