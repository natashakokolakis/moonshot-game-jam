using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PointToOffScreenTarget : MonoBehaviour
{
    // Apply this script to an image element on a canvas.


    public Transform targetTransform;
    public float iconMoveSpeed = 5;

    private RectTransform uiIconTransform;
    private Image thisImage;
    private Vector3 targetWorldPosition;

    private Camera mainCamera;
    private float maxCamWidth;
    private float maxCamHeight;


    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        uiIconTransform = GetComponent<RectTransform>();
        thisImage = GetComponent<Image>();

        maxCamHeight = mainCamera.pixelHeight;
        maxCamWidth = mainCamera.pixelWidth;
    }

    public void ChangeTargetTransform(Transform newTarget)
    {
        targetTransform = newTarget;
    }
    

    private void OnEnable()
    {
        uiIconTransform.localPosition = Vector3.zero;

    }

    private bool CheckIfCoordinateOnScreen(float x, float dimensionMax)
    {
        if (x >= 0 & x <= dimensionMax)
            return true;
        else
            return false;
    }

    private bool CheckIfTargetOnScreen()
    {
        if (CheckIfCoordinateOnScreen(targetWorldPosition.x, maxCamWidth) & CheckIfCoordinateOnScreen(targetWorldPosition.y, maxCamHeight))
            return true;
        else
            return false;
    }

    private void TurnImageOffifTargetOnScreen()
    {
        if (CheckIfTargetOnScreen())
            thisImage.enabled = false;
        else
            thisImage.enabled = true;
            
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveIndicator();
        TurnImageOffifTargetOnScreen();

    }

    private void MoveIndicator()
    {
        targetWorldPosition = mainCamera.WorldToScreenPoint(targetTransform.position);
        uiIconTransform.position = Vector3.Lerp(uiIconTransform.position, targetWorldPosition, iconMoveSpeed * Time.deltaTime);
        uiIconTransform.position = new Vector3(Mathf.Clamp(uiIconTransform.position.x, 0, maxCamWidth), Mathf.Clamp(uiIconTransform.position.y, 0, maxCamHeight), uiIconTransform.position.z);
    }
}
