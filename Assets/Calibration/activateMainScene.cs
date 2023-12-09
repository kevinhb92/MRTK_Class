using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;


[AddComponentMenu("Scripts/MRTK/Examples/SpawnOnPointerEvent")]
public class activateMainScene : MonoBehaviour
{
    public GameObject centerMarker;
    public GameObject mainScene;
    public GameObject buttonCanvas;
    public GameObject mainCamera;
    public GameObject allCalib;
    public GameObject resetButton;

    public void ActivateScene()
    {
        // activate main scene at position of the calibration X, but with the y-rotation of the main camera (aka whichever direction the user is facing)
        // y-rotation of head camera works better b/c rotation of the hands gets skewed if the selected point is more than a few feet away
        mainScene.transform.position = centerMarker.transform.position;
        mainScene.transform.eulerAngles = new Vector3 (0, mainCamera.transform.eulerAngles.y, 0);
        mainScene.SetActive(true);
        resetButton.SetActive(true);

        // deactivate the canvas, spatial awareness system,  and calibration objects
        buttonCanvas.SetActive(false);
        allCalib.SetActive(false);
        //CoreServices.SpatialAwarenessSystem.Disable();
    }
}
