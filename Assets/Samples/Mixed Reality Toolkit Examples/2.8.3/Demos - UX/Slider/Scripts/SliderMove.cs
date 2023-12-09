// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Examples.Demos
{
    [AddComponentMenu("Scripts/MRTK/Examples/SliderLunarLander")]
    public class SliderMove : MonoBehaviour
    {
        [SerializeField]
        private Transform transformLandingGear = null;

        public void OnSliderUpdated(SliderEventData eventData)
        {
            if (transformLandingGear != null)
            {
                // Rotate the target object using Slider's eventData.NewValue
                transformLandingGear.localPosition = new Vector3(eventData.NewValue - 0.5f, transformLandingGear.localPosition.y, transformLandingGear.localPosition.z);
            }
        }
    }
}
