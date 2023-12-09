using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;

public class angleRoation : MonoBehaviour
{
    private IMixedRealityGazeProvider gazeProvider;
    private Quaternion initialRotation;

    private void Start()
    {
        gazeProvider = CoreServices.InputSystem?.GazeProvider;
        initialRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        if (gazeProvider != null)
        {
            Vector3 gazeDirection = gazeProvider.GazeDirection;
            Quaternion desiredRotation = Quaternion.LookRotation(gazeDirection, Vector3.up) * initialRotation;

            transform.rotation = desiredRotation;
        }
    }
}