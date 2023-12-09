using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Examples.Demos
{
    [AddComponentMenu("Scripts/MRTK/Examples/centerOnPointerEvent")]
    public class centerOnPointerEvent : MonoBehaviour
    {
        public GameObject floorPoint;
        public GameObject centerMarker;
        public GameObject calibConfirm;
        
        private void Start() 
        {
            // deactivate these until needed
            centerMarker.SetActive(false);
            calibConfirm.SetActive(false);
        }

        public void Center()
        {
            // activate the orange X where the hand ray was clicked and show confirmation button
            centerMarker.transform.position = floorPoint.transform.position;
            centerMarker.SetActive(true);
            calibConfirm.SetActive(true);
        }
    }
}