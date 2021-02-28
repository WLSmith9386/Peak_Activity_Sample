using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CubeManager : MonoBehaviour
{
    public GameObject interactiveCube;
    public Animation cubeAnimation;
    public GameObject pathGuiItems;
    public TextMeshProUGUI instruText;

    private bool cubePlaced = false;
    private List<Vector3> pathNodes;

    AnimationClip animClip;//used to create animations for paths

    void Start()
    {
        instruText.text = "Tap on the space to put a cube there.";
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))//stops GUI from hiding when pressing button over surface space
            {
                return;
            }

            var touch = Input.GetTouch(0);
            Ray cast = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(cast, out RaycastHit hit))
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:

                        if(cubePlaced)
                        {
                            pathNodes = new List<Vector3>();
                            pathNodes.Add(hit.point);
                        }

                        break;

                    case TouchPhase.Moved:

                        if (cubePlaced)
                        {
                            interactiveCube.transform.position = hit.point;
                            pathNodes.Add(hit.point);
                        }

                        break;

                    case TouchPhase.Ended:

                        PlaceCube(hit);//place the cube in it's initial starting point.

                        if (pathNodes.Count <= 1)
                        {
                            interactiveCube.transform.position = hit.point;
                            pathGuiItems.SetActive(false);
                        }
                        else
                        {
                            pathGuiItems.SetActive(true);
                            CreateAnimationClip();
                        }

                        break;
                }
            }
        }
    }

    private void CreateAnimationClip()
    {        
        AnimationCurve TranslateCubeX = new AnimationCurve();
        AnimationCurve TranslateCubeY = new AnimationCurve();
        AnimationCurve TranslateCubeZ = new AnimationCurve();

        animClip = new AnimationClip();

        for (int i = 0; i < pathNodes.Count; i++)
        {
            TranslateCubeX.AddKey(i, pathNodes[i].x);
            TranslateCubeY.AddKey(i, pathNodes[i].y);
            TranslateCubeZ.AddKey(i, pathNodes[i].z);
        }

        animClip.legacy = true;

        animClip.SetCurve("", typeof(Transform), "localPosition.x", TranslateCubeX);
        animClip.SetCurve("", typeof(Transform), "localPosition.y", TranslateCubeY);
        animClip.SetCurve("", typeof(Transform), "localPosition.z", TranslateCubeZ);

        cubeAnimation.AddClip(animClip, "path");
    }

    private void PlaceCube(RaycastHit hit)
    {
        if (!cubePlaced)
        {
            interactiveCube.transform.position = hit.point;
            interactiveCube.SetActive(true);
            cubePlaced = true;
        }
    }

    public void StartPath()
    {
        cubeAnimation["path"].speed = 1.5f;
        cubeAnimation.Play("path");    
    }

    public void ReversePath()
    {
        cubeAnimation["path"].speed = -1.5f;
        cubeAnimation.Play("path");
    }
}
