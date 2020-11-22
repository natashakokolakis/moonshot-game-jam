using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PointToOffScreenTarget : MonoBehaviour
{

    public Transform targetTransform;
    public float iconMoveSpeed = 1;

    private RectTransform arrowIconTransform;
    private Image thisImage;
    private Vector3 targetWorldPosition;

    private Camera mainCamera;
    private float maxCamWidth;
    private float maxCamHeight;


    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        arrowIconTransform = GetComponent<RectTransform>();
        thisImage = GetComponent<Image>();

        maxCamHeight = mainCamera.pixelHeight;
        maxCamWidth = mainCamera.pixelWidth;
    }

    

    private void OnEnable()
    {
        arrowIconTransform.localPosition = Vector3.zero;

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
        arrowIconTransform.position = Vector3.Lerp(arrowIconTransform.position, targetWorldPosition, iconMoveSpeed * Time.deltaTime);
        arrowIconTransform.position = new Vector3(Mathf.Clamp(arrowIconTransform.position.x, 0, maxCamWidth), Mathf.Clamp(arrowIconTransform.position.y, 0, maxCamHeight), arrowIconTransform.position.z);
    }
}
