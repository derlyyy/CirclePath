using UnityEngine;

public class MainCam : MonoBehaviour
{
    public static MainCam instance;

    public Camera cam;
    
    private void Awake()
    {
        instance = this;

        cam = GetComponent<Camera>();
    }
}