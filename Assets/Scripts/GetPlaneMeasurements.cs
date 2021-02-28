using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class GetPlaneMeasurements : MonoBehaviour
{
    public TextMeshProUGUI areaText;
    public ARPlane arPlane;

    private void OnEnable()
    {
        arPlane.boundaryChanged += BoundaryChanged;
    }

    private void OnDisable()
    {
        arPlane.boundaryChanged -= BoundaryChanged;
    }
    private void BoundaryChanged(ARPlaneBoundaryChangedEventArgs obj)
    {
        if (areaText != null)
        {
            areaText.text = string.Format("{0} m2", CalculatePlaneArea(arPlane).ToString());
        }
    }

    public float CalculatePlaneArea(ARPlane plane)
    {
        return plane.size.x * plane.size.y;
    }
}
