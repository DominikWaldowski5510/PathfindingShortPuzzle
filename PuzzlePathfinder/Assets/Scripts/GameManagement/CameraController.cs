using UnityEngine;
using UnityEngine.UI;

//Controls camera movement behaviour
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform startPosPoint = null, endPositionPoint = null;           //where camera starts and ends its position
    [Range(0.0f,1.0f)] private float cameraPosition = 0f;
    [SerializeField] private Slider cameraPositionSlider = null;

    //Controls our camera position
    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(startPosPoint.position, endPositionPoint.position, cameraPosition);
    }

    //When slider value is changed it changes camera position accordingly
    public void OnSliderUpdate()
    {
        cameraPosition = cameraPositionSlider.value;
    }
}
