using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cannon : MonoBehaviour
{
    public GameObject cannonBall;
    public GameObject missileTargetWarningPrefab;
    
    public Transform shootTransform;

    public AudioSource shootSound;
    public Animator shootAnimator;
    
    private Rigidbody cannonballInstance;

    public Transform cannonBodyTransform;

    public int cannonBallCount;
    public float reloadDelay;
    private float _reloadTimer;
    
    [SerializeField]
    [Range(10f, 80f)]
    private float angle = 45f;

    private bool _touched;
    
    private void Start()
    {
        _touched = false;
        
        _reloadTimer = 0;
    }

    private void Update()
    {
        if (GameManager.gameState != GameState.Playing) return;
        _reloadTimer += Time.deltaTime;
        if (_reloadTimer >= reloadDelay)
        {
            var targetPointOnGround = Spawner.GetRandomPointOnGround();
            FireCannonAtPoint(targetPointOnGround);
            
            _reloadTimer = 0;
        }

    }
    
    private void FireCannonAtPoint(Vector3 point)
    {
        shootSound.Play();
        shootAnimator.Play("CannonShootAnimation", -1, 0f);
        
        var rotation = cannonBodyTransform.rotation;
        cannonBodyTransform.LookAt(point);
        rotation.eulerAngles = new Vector3(rotation.x, cannonBodyTransform.rotation.eulerAngles.y , rotation.z);
        cannonBodyTransform.rotation = rotation;

        var cannonBallObject = Instantiate(cannonBall);
        cannonballInstance = cannonBallObject.GetComponent<Rigidbody>();
        
        var targetWarningObj = Instantiate(missileTargetWarningPrefab, Spawner.GetWorldTransform());
        targetWarningObj.transform.localPosition = point + new Vector3(0, 15.75f, 0);    //offsetting to level the ground top

        cannonBallObject.GetComponent<CanonBall>().targetWarningObj = targetWarningObj;
        
        var velocity = BallisticVelocity(point, angle);
        
        cannonballInstance.transform.position = shootTransform.position;
        cannonballInstance.velocity = velocity;
        // Apply the rotation to the rigid body
    }

    private Vector3 BallisticVelocity(Vector3 destination, float angle)
    {
        Vector3 dir = destination - shootTransform.position; // get Target Direction
        float height = dir.y; // get height difference
        dir.y = 0; // retain only the horizontal difference
        float dist = dir.magnitude; // get horizontal currentDirection
        float a = angle * Mathf.Deg2Rad; // Convert angle to radians
        dir.y = dist * Mathf.Tan(a); // set dir to the elevation angle.
        dist += height / Mathf.Tan(a) - 0.7f; // Correction for small height differences

        // Calculate the velocity magnitude
        float velocity = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return velocity * dir.normalized; // Return a normalized vector.
    }

    public void AddExtraCannonBall(int count)
    {
        cannonBallCount += count;
        PlayerPrefs.SetInt("CannonBallCount", cannonBallCount);
    }

    public static void Reset()
    {
        var cannonBalls = GameObject.FindGameObjectsWithTag("CannonBallHolder");
        foreach (var ball in cannonBalls)
        {
            Destroy(ball);
        }
    }

    private bool IsPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
