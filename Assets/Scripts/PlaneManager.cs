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
        //set initial text for display
        areaText.text = "Select a plane to get area.";
        planeSelectionButtons.SetActive(false);
        cubeManager.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.touchCount < 1)
        {
            return;
        }

        var touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Ended)
        {
            Ray cast = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            bool castHitObject = (Physics.Raycast(cast, out hit));

            if (!castHitObject)
            {
                return;
            }
            
            GetPlaneMeasurements gpm = hit.collider.gameObject.GetComponent<GetPlaneMeasurements>();

            if (gpm == null)
            {
                return;
            }

            areaText.text = "Calculating area";
            gpm.areaText = areaText;
            //force it to do an intial calculations.
            gpm.CalculatePlaneArea(gpm.arPlane);
            selectedPlane = gpm.transform;
            //turn on options
            planeSelectionButtons.SetActive(true);
        }
    }

    public void KeepPlane()
    {
        //after selecting this move onto cube management
        areaText.text = "Let's play with some cubes!";
        planeSelectionButtons.SetActive(false);

        //stop detecting more planes
        aRPlaneManager.enabled = false;
        ARPlane selectedArPlane = selectedPlane.GetComponent<ARPlane>();

        //remove planes that aren't going to be used to avoid screen clutter
        foreach (ARPlane a in aRPlaneManager.trackables)
        {
            if (a != selectedArPlane)
            {
                a.gameObject.SetActive(false);
            }
        }

        cubeManager.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    //Used to hide unwanted planes
    public void HidePlane()
    {
        planeSelectionButtons.SetActive(false);
        //hide the currently selected plane
        selectedPlane.gameObject.SetActive(false);
        //remove from selected plane
        selectedPlane = null;
        areaText.text = "Select a plane to get area.";
    }
}
