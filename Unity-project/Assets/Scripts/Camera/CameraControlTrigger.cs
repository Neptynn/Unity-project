using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CameraControlTrigger : MonoBehaviour
{
    public CustomInspectorObject customInspectorObject;
    private PlayerModel _player;
    private Collider2D _coll;
    private bool _isTrigered;
    Collider2D _collision;
    private float _speed = 0.01f;

    [SerializeField] PlayerMovement playerMovement;

    private void Start()
    {
        _coll = GetComponent<Collider2D>();
        _player = PlayerModel.Instance;
    }
    private void FixedUpdate()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {

            if (customInspectorObject.panCameraOnContact)
            {
                //CheckPosition();
                CameraManager.instance.PanCameraOnContact(customInspectorObject.panDistance, customInspectorObject.panTime, customInspectorObject.panDirection, false);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 exitDirection = (collision.transform.position - _coll.bounds.center).normalized;
            if(customInspectorObject.swapCameras && customInspectorObject.cameraOnLeft != null && customInspectorObject.cameraOnRight != null)
            {
                CameraManager.instance.SwapCamera(customInspectorObject.cameraOnLeft, customInspectorObject.cameraOnRight, exitDirection);
            }
            if (customInspectorObject.panCameraOnContact)
            {
                //CheckPosition();
                CameraManager.instance.PanCameraOnContact(customInspectorObject.panDistance, customInspectorObject.panTime, customInspectorObject.panDirection, true);      
            }
        }

    }
    //private void CheckPosition()
    //{
    //    if (playerMovement.GetRbVelocityY() > _speed && customInspectorObject.panDirection.Equals(PanDirection.Down))
    //    {
    //        customInspectorObject.panDirection = PanDirection.Up;
    //    }
    //    else if (playerMovement.GetRbVelocityY() < -_speed && customInspectorObject.panDirection.Equals(PanDirection.Up))
    //    {
    //        customInspectorObject.panDirection = PanDirection.Down;
    //    }
    //}
}


[System.Serializable]
public class CustomInspectorObject
{
    public bool swapCameras = false;
    public bool panCameraOnContact = false;

    [HideInInspector] public CinemachineVirtualCamera cameraOnLeft;
    [HideInInspector] public CinemachineVirtualCamera cameraOnRight;

    [HideInInspector] public PanDirection panDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.5f;
}
public enum PanDirection
{
    Up,
    Down,
    Left,
    Right
}

[CustomEditor(typeof(CameraControlTrigger))]
public class MyScriptEditor : Editor
{
    CameraControlTrigger cameraControlTrigger;

    private void OnEnable()
    {
        cameraControlTrigger = (CameraControlTrigger)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (cameraControlTrigger.customInspectorObject.swapCameras)
        {
            cameraControlTrigger.customInspectorObject.cameraOnLeft = EditorGUILayout.ObjectField("Camera on Left", cameraControlTrigger.customInspectorObject.cameraOnLeft,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;

            cameraControlTrigger.customInspectorObject.cameraOnRight = EditorGUILayout.ObjectField("Camera on Right", cameraControlTrigger.customInspectorObject.cameraOnRight,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
        }
        if (cameraControlTrigger.customInspectorObject.panCameraOnContact)
        {
            cameraControlTrigger.customInspectorObject.panDirection = (PanDirection)EditorGUILayout.EnumPopup("Camera Pan Direction",
                cameraControlTrigger.customInspectorObject.panDirection);

            cameraControlTrigger.customInspectorObject.panDistance = EditorGUILayout.FloatField("Pan Distance", cameraControlTrigger.customInspectorObject.panDistance);
            cameraControlTrigger.customInspectorObject.panTime = EditorGUILayout.FloatField("Pan Time", cameraControlTrigger.customInspectorObject.panTime);
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(cameraControlTrigger);
        }
    }
}