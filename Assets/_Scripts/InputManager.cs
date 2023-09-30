using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera camera;

    [Header("Key Events")] 
    public bool pointerActive;
    public static event Action<bool> OnPointerPrimary;

    [Header("Pointer Information")]
    //Pointer Position
    public static Vector3 PointerPositionViewportSpace;
    public static Vector3 PointerPositionScreenSpace;
    public static Vector3 PointerPositionWorldSpace;
    //Inspector visible variables
    [SerializeField] private Vector3 _pointerPositionViewportSpace;
    [SerializeField] private Vector3 _pointerPositionScreenSpace;
    [SerializeField] private Vector3 _pointerPositionWorldSpace;


    void Awake()
    {
        camera = Camera.main;
    }

    public void PointerPrimary(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            pointerActive = true;
            OnPointerPrimary?.Invoke(pointerActive);
        }
        else if (context.canceled)
        {
            pointerActive = false;
            OnPointerPrimary?.Invoke(pointerActive);
        }
    }

    // Update is called once per frame
    public void PointerPositionUpdate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Read Pointer Position (Screen Space)
            PointerPositionScreenSpace = context.ReadValue<Vector2>();

            //Clamp to Min/Max Screen Space
            PointerPositionScreenSpace.x = MathF.Min(Screen.width, PointerPositionScreenSpace.x);
            PointerPositionScreenSpace.x = MathF.Max(0, PointerPositionScreenSpace.x);
            PointerPositionScreenSpace.y = MathF.Min(Screen.height, PointerPositionScreenSpace.y);
            PointerPositionScreenSpace.y = MathF.Max(0, PointerPositionScreenSpace.y);

            //Convert to viewport (0,0)-(1,1)
            PointerPositionViewportSpace = camera.ScreenToViewportPoint(PointerPositionScreenSpace);
            
            //Convert to world space
            PointerPositionWorldSpace = camera.ScreenToWorldPoint(new Vector3(PointerPositionScreenSpace.x, PointerPositionScreenSpace.y,
                -camera.transform.position.z));
            
            //Save references to be seen in editor.
            _pointerPositionViewportSpace = PointerPositionViewportSpace;
            _pointerPositionScreenSpace = PointerPositionScreenSpace;
            _pointerPositionWorldSpace = PointerPositionWorldSpace;
        }
    }
}
