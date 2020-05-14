using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CanonBall : MonoBehaviour
{
    public float destructionDuration;

    public AudioSource cannonHitSound;

    private float _destroyCheckerTimer;

    private Rigidbody rb;

    private bool isCannonFalling = true;

    public Transform blastTransform;

    public GameObject targetWarningObj;

    public float blastTriggerRange;
    
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    public void Update()
    {
        _destroyCheckerTimer += Time.deltaTime;
        if (_destroyCheckerTimer >= 10 && isCannonFalling)
        {
            Destroy(gameObject);
        }
        
        if (isCannonFalling)
        {
            transform.LookAt(transform.position + rb.velocity);
        }
        
        if (Vector3.Distance(transform.position, targetWarningObj.transform.position) <= blastTriggerRange)
        {
            Hit();
        }
    }

    private void Hit()
    {
        isCannonFalling = false;
        
        rb.isKinematic = true;
        cannonHitSound.Play();
        
        blastTransform.gameObject.SetActive(true);
        blastTransform.eulerAngles = new Vector3(90, 0, 0);
        
        Destroy(targetWarningObj);
        
        StartCoroutine(TriggerCannonDeactivation());
    }

    private IEnumerator TriggerCannonDeactivation()
    {
        yield return new WaitForSeconds(destructionDuration);
        
        DeactivateCannon();
    }
    
    private void DeactivateCannon()
    {
        Destroy(gameObject);
    }

}
