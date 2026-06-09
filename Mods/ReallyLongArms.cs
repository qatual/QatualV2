using UnityEngine;

public class ReallyLongArms : MonoBehaviour
{
    public GameObject objectTarget;
    private Vector3 originalScale;

    void Awake()
    {
        // SCRIPT MADE BY KALBITE.
        if (objectTarget == null)
        {
            Debug.LogError("Target object not assigned in SkyArmsResizer script.");
            return;
        }

        originalScale = objectTarget.transform.localScale;
        ResizeObject(true);
    }

    void ResizeObject(bool resize)
    {
        if (objectTarget != null)
        {
            objectTarget.transform.localScale = resize ? new Vector3(1.3f, 1.3f, 1.3f) : originalScale;
        }
    }

    void OnEnable()
    {
        ResizeObject(true);
    }

    void OnDisable()
    {
        ResizeObject(false);
    }
}

