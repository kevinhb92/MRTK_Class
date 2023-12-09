using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Physics;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;
using TMPro;

/*
lots of code taken from Joost's "Finding the Floor with HoloLens 2" project
https://localjoost.github.io/Finding-the-Floor-with-HoloLens-2-and-MRTK-273/
*/

public class floorFinder : MonoBehaviour
{
    
    private Vector3? foundPosition = null;
    private float maxDistance = 3.0f;
    private float _delayMoment;

    public GameObject floorPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        CoreServices.SpatialAwarenessSystem.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        // Every frame, get data on position & rotation of hand pointers
        foreach (IMixedRealityController controller in CoreServices.InputSystem.DetectedControllers)
        {
            foreach (MixedRealityInteractionMapping interactionMapping in controller.Interactions)
            {
                if (interactionMapping.InputType == DeviceInputType.SpatialPointer)
                {
                    Reset();
                    CheckLocationOnSpatialMap(interactionMapping.PositionData, interactionMapping.RotationData);
                }
            }
        }
    }


    public void Reset()
    {
        _delayMoment = Time.time + 2;
        foundPosition = null;
    }

    private void CheckLocationOnSpatialMap(Vector3 posData, Quaternion rotData)
    {
        foundPosition = GetPositionOnSpatialMap(posData, rotData, maxDistance);
        if (foundPosition != null)
        {
            // empty GameObject floorPoint tracks the hand ray's intersection with the floor
            floorPoint.transform.position = foundPosition.Value;
        }
    }
    
    public static Vector3? GetPositionOnSpatialMap(Vector3 pos_data, Quaternion rot_data, float maxDistance = 2,
        BaseRayStabilizer stabilizer = null)
    {
        // make hand ray using positon & rotation data
        var handRay = stabilizer?.StableRay ?? new Ray(pos_data, rot_data * Vector3.forward);

        // get point where hand ray intersects with spatialmesh, make sure it's not too far away
        if (Physics.Raycast(handRay, out var hitInfo, maxDistance, GetSpatialMeshMask()))
        {
            return hitInfo.point;
        }
        return null;
    }


    // this wasn't altered from Joost's code at all
    private static int _meshPhysicsLayer = 0;

    private static int GetSpatialMeshMask()
    {
        if (_meshPhysicsLayer == 0)
        {
            var spatialMappingConfig = Microsoft.MixedReality.Toolkit.CoreServices.SpatialAwarenessSystem.ConfigurationProfile as
                MixedRealitySpatialAwarenessSystemProfile;
            if (spatialMappingConfig != null)
            {
                foreach (var config in spatialMappingConfig.ObserverConfigurations)
                {
                    var observerProfile = config.ObserverProfile
                        as MixedRealitySpatialAwarenessMeshObserverProfile;
                    if (observerProfile != null)
                    {
                        _meshPhysicsLayer |= (1 << observerProfile.MeshPhysicsLayer);
                    }
                }
            }
        }

        return _meshPhysicsLayer;
    }
}
