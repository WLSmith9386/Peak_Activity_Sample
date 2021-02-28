using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class PlaneManager : MonoBehaviour
{
    public TextMeshProUGUI areaText;
    public GameObject planeSelectionButtons;
    public GameObject cubeGuiItems;
    public ARPlaneManager aRPlaneManager;
    public CubeManager cubeManager;
    private Transform selectedPlane;

    private void Awake()
    {
        areaText.text = "Select a plane to get area.";//set initial text for display
        planeSelectionButtons.SetActive(false);
        cubeManager.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                Ray cast = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(cast, out RaycastHit hit))
                {
                    GetPlaneMeasurements gpm = hit.collider.gameObject.GetComponent<GetPlaneMeasurements>();

                    if (gpm != null)
                    {
                        areaText.text = "Calculating area";
                        gpm.areaText = areaText;
                        gpm.CalculatePlaneArea(gpm.arPlane);//force it to do an intial calculations.
                        selectedPlane = gpm.transform;
                        planeSelectionButtons.SetActive(true);//turn on options
                        
                    }
                }
            }
        }
    }

    public void KeepPlane()
    {
        //after selecting this move onto cube management
        areaText.text = "Let's play with some cubes!";
        planeSelectionButtons.SetActive(false);
        aRPlaneManager.enabled = false;//stop detecting more planes
        ARPlane selectedArPlane = selectedPlane.GetComponent<ARPlane>();

        foreach(ARPlane a in aRPlaneManager.trackables)//remove planes that aren't going to be used to avoid screen clutter
        {
            if (a != selectedArPlane)
            {
                a.gameObject.SetActive(false);
            }
        }

        cubeManager.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void HidePlane()
    {
        planeSelectionButtons.SetActive(false);
        selectedPlane.gameObject.SetActive(false);//hide the currently selected plane
        selectedPlane = null;//remove from selected plane
        areaText.text = "Select a plane to get area.";
    }
}
