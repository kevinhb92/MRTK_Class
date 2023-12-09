using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class ResizableRotatableObject : MonoBehaviour
{
    private PinchSlider pinchSlider;

    private Vector3 originalScale;
    private float originalRotationY;

    private void Start()
    {
        // Ensure the object has a PinchSlider component
        pinchSlider = GetComponent<PinchSlider>();
        if (pinchSlider == null)
        {
            Debug.LogError("PinchSlider component is missing on the object.");
            return;
        }

        // Store original scale and rotation for resizing and rotating
        originalScale = transform.localScale;
        originalRotationY = transform.rotation.eulerAngles.y;

        // Subscribe to PinchSlider events
        pinchSlider.OnValueUpdated.AddListener(OnSliderValueUpdated);
    }

    private void OnDestroy()
    {
        // Unsubscribe from PinchSlider events
        if (pinchSlider != null)
        {
            pinchSlider.OnValueUpdated.RemoveListener(OnSliderValueUpdated);
        }
    }

    private void OnSliderValueUpdated(SliderEventData eventData)
    {
        // Adjust the object's scale based on the pinch slider value
        transform.localScale = originalScale * pinchSlider.SliderValue;

        // Adjust the object's rotation based on the pinch slider value
        float newRotationY = originalRotationY + pinchSlider.SliderValue * 360f;
        transform.rotation = Quaternion.Euler(0f, newRotationY, 0f);
    }
}