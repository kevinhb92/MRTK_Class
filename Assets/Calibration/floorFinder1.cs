using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Physics;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;
using TMPro;

public class floorFinder1 : MonoBehaviour
{
    private Vector3? foundPosition = null;
    private float maxDistance = 3.0f;
    private float _delayMoment;
    private Quaternion anchorRotation;

    public GameObject floorPoint;
    public GameObject anchorObject; // The object you want to align with the rotation

    private IMixedRealityGazeProvider gazeProvider;

    // Stabilization variables
    private int rayCount = 5;
    private Vector3[] rayDirections;
    private Vector3 averageRayDirection;

    [SerializeField]
    private bool isSpatialAwarenessActive = false; // Flag to track spatial awareness activation

    // Start is called before the first frame update
    void Start()
    {
        CoreServices.SpatialAwarenessSystem.Reset();
        gazeProvider = CoreServices.InputSystem.GazeProvider as IMixedRealityGazeProvider;

        // Store the initial gaze direction
        averageRayDirection = gazeProvider.GazePointer.Rays[0].Direction;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpatialAwarenessActive)
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
            if (anchorObject == null)
            {
                // Create a new anchor object and set its rotation to align with rotData
                anchorObject = new GameObject("AnchorObject");
                anchorObject.transform.position = foundPosition.Value;
                anchorRotation = Quaternion.Euler(-90f, QuantizeRotation(rotData.eulerAngles.y), 0f);
            }
            else
            {
                // Update the floorPoint's position based on the foundPosition
                floorPoint.transform.position = foundPosition.Value;

                // Update the anchor object's y-angle
                Quaternion desiredRotation = Quaternion.Euler(-90f, QuantizeRotation(rotData.eulerAngles.y), 0f);
                anchorRotation = Quaternion.Euler(-90f, desiredRotation.eulerAngles.y, 0f);
            }

            // Apply the anchor object's rotation
            anchorObject.transform.rotation = anchorRotation;

            // Quantize gaze angle
            float gazeAngle = QuantizeRotation(GetGazeAngle());
            // Use the quantized gaze angle as needed
            // ...
        }
    }

    private float QuantizeRotation(float angle)
    {
        const float angleStep = 10f;
        float quantizedAngle = Mathf.Round(angle / angleStep) * angleStep;
        return quantizedAngle;
    }

    private float GetGazeAngle()
    {
        // Calculate the average gaze direction based on multiple raycasts
        averageRayDirection = Vector3.zero;
        for (int i = 0; i < rayCount; i++)
        {
            averageRayDirection += rayDirections[i];
        }
        averageRayDirection /= rayCount;

        // Calculate the gaze angle based on the average gaze direction
        float gazeAngle = Mathf.Atan2(averageRayDirection.x, averageRayDirection.z) * Mathf.Rad2Deg;

        return gazeAngle;
    }

    public static Vector3? GetPositionOnSpatialMap(Vector3 posData, Quaternion rotData, float maxDistance = 2)
    {
        // Make hand ray using position & rotation data
        var handRay = new Ray(posData, rotData * Vector3.forward);

        // Get point where hand ray intersects with spatial mesh, make sure it's not too far away
        if (Physics.Raycast(handRay, out var hitInfo, maxDistance, GetSpatialMeshMask()))
        {
            return hitInfo.point;
        }
        return null;
    }

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

    public void ToggleSpatialAwareness()
    {
        isSpatialAwarenessActive = !isSpatialAwarenessActive; // Toggle spatial awareness activation

        if (isSpatialAwarenessActive)
        {
            SetSpatialAwarenessActive(true);
        }
        else
        {
            SetSpatialAwarenessActive(false);
        }
    }

    private void SetSpatialAwarenessActive(bool isActive)
    {
        var spatialAwarenessSystem = CoreServices.SpatialAwarenessSystem;
        if (spatialAwarenessSystem != null)
        {
            if (isActive)
            {
                spatialAwarenessSystem.ResumeObservers();
                Debug.Log("Spatial awareness activated.");
            }
            else
            {
                spatialAwarenessSystem.SuspendObservers();
                Debug.Log("Spatial awareness deactivated.");
            }
        }
    }
}