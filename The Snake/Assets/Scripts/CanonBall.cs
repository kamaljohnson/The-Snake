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

    private Rigidbody _rb;

    private bool _isCannonFalling = true;

    public Transform blastTransform;

    public GameObject targetWarningObj;

    public float blastTriggerRange;
    
    public void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    public void Update()
    {

        if (!_isCannonFalling) return;
        
        transform.LookAt(transform.position + _rb.velocity);
            
        if (Vector3.Distance(transform.position, targetWarningObj.transform.position) <= blastTriggerRange)
        {
            Hit();
        }
    }

    private void Hit()
    {
        _isCannonFalling = false;
        
        _rb.isKinematic = true;
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
