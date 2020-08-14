using Unity.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine.XR.ARKit;
#endif

[RequireComponent(typeof(ARFace))]
public class BlinkDetector : MonoBehaviour
{
    #pragma warning disable 0649
    [SerializeField]
    private GameEvent blinkEvent;
    #pragma warning restore 0649
    ARFace m_Face;

    #if UNITY_IOS && !UNITY_EDITOR
        ARKitFaceSubsystem m_ARKitFaceSubsystem;
    #endif
    bool activate;

    void Awake()
    {
        m_Face = GetComponent<ARFace>();
    }

    void OnEnable()
    {
        Debug.Log("Here");
#if UNITY_IOS && !UNITY_EDITOR
        var faceManager = FindObjectOfType<ARFaceManager>();
        if (faceManager != null)
        {
            m_ARKitFaceSubsystem = (ARKitFaceSubsystem)faceManager.subsystem;
        }
#endif
        UpdateActivity();
        m_Face.updated += OnUpdated;
        ARSession.stateChanged += OnSystemStateChanged;
    }

    void OnDisable()
    {
        m_Face.updated -= OnUpdated;
        ARSession.stateChanged -= OnSystemStateChanged;
    }

    void OnSystemStateChanged(ARSessionStateChangedEventArgs eventArgs)
    {
        UpdateActivity();
    }

    void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
    {
        UpdateActivity();
        Detect();
    }

    void UpdateActivity()
    {
        var activate =
            enabled &&
            (m_Face.trackingState == TrackingState.Tracking) &&
            (ARSession.state > ARSessionState.Ready);

        SetActivity(activate);
    }

    void SetActivity(bool activate)
    {
        this.activate = activate;
    }

    void Detect() {
        if(!activate) {
            return;
        }
    #if UNITY_IOS && !UNITY_EDITOR
        using (var blendShapes = m_ARKitFaceSubsystem.GetBlendShapeCoefficients(m_Face.trackableId, Allocator.Temp))
        {
            foreach (var featureCoefficient in blendShapes)
            {
                if(featureCoefficient.blendShapeLocation == ARKitBlendShapeLocation.EyeBlinkLeft || featureCoefficient.blendShapeLocation == ARKitBlendShapeLocation.EyeBlinkRight) {
                    Debug.Log(featureCoefficient.coefficient);
                    if(featureCoefficient.coefficient > 0.9) {
                        blinkEvent.Raise();
                        break;
                    }
                }
                
            }
        }
    #endif
    }    
}