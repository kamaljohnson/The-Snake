using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CannonManager : MonoBehaviour
{
    public GameObject cannonPrefab;
    
    public List<Transform> cannonBaseTransforms;

    private readonly List<bool> _cannonFilledAtLocation = new List<bool>();
    
    private List<GameObject> _listOfCannons = new List<GameObject>();
    
    public void Start()
    {
        for (var i = 0; i < cannonBaseTransforms.Count; i++)
        {
            _cannonFilledAtLocation.Add(false);
        }
        InitializeCannon();
    }

    private void InitializeCannon()
    {
        var flag = _cannonFilledAtLocation.Any(filled => !filled);

        if (!flag) return;
        
        while (true)
        {
            var randomPlacementIndex = Mathf.RoundToInt(Random.Range(0, cannonBaseTransforms.Count));
            if (_cannonFilledAtLocation[randomPlacementIndex]) continue;
            _cannonFilledAtLocation[randomPlacementIndex] = true;
            var cannonObj = Instantiate(cannonPrefab, cannonBaseTransforms[randomPlacementIndex]);
            _listOfCannons.Add(cannonObj);
            break;
        }
    }
}
