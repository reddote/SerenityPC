using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HealthBar
{
    readonly GameObject healthBar;
    readonly RectTransform rectTransform;
    readonly Image fillImage;
    int lastDrawRequestFrame;

    public HealthBar(GameObject instantiated)
    {
        healthBar = instantiated;

        fillImage = FindFill(healthBar);
        rectTransform = FindRect(healthBar);
    }

    RectTransform FindRect(GameObject obj)
    {
        return obj.GetComponent<RectTransform>();
    }

    Image FindFill(GameObject obj)
    {
        // Find the fill image from healthbar 
        Image fillImage = null;
        
            Image image = obj.GetComponent<Image>();

            if (image != null)
            {
                bool isFill = image.fillMethod == Image.FillMethod.Horizontal;

                if (isFill)
                {
                    fillImage = image;
                }
            }
        

        return fillImage;
    }

    public RectTransform GetRect()
    {
        return rectTransform;
    }

    public void SetFillAmount(float fillAmount)
    {
        fillImage.fillAmount = fillAmount;
    }

    public bool ShouldDrawThisFrame()
    {
        return lastDrawRequestFrame == Time.frameCount;
    }

    public void SetLastDrawRequestFrame(int frame)
    {
        lastDrawRequestFrame = frame;
    }
}

/// <summary>
/// Draw healthbar for enemies if they are within the camera view. Add to gameObject with Canvas component. 
/// </summary>
[RequireComponent(typeof(Canvas))]
public class HealthbarDrawer : MonoBehaviour
{
    public static HealthbarDrawer Instance { get; private set; }
    public GameObject healthBarPrefab;

    Canvas canvas;
    public Camera droneCamera;
    List<HealthBar> healthBars = new List<HealthBar>();
    Plane[] planes;
    Vector3 outOfCameraScreenPoint;
    Vector3 outOfCameraViewportPoint = Vector3.one * 2;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
       
        canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        // Update planes once each frame 
        planes = GeometryUtility.CalculateFrustumPlanes(droneCamera);
        // Update out of camera pos each frame 
        outOfCameraScreenPoint = droneCamera.ViewportToScreenPoint(outOfCameraViewportPoint);
    }

    void LateUpdate()
    {
        // Check each healthbar 
        for (int i = 0; i < healthBars.Count; i++)
        {
            HealthBar bar = healthBars[i];

            // If you shouldn't draw the bar this frame 
            if (!bar.ShouldDrawThisFrame())
            {
                // Put healthbar out of screen 
                RectTransform rect = bar.GetRect();
                rect.position = outOfCameraScreenPoint;
            }
        }
    }

    public void DrawHealthbar(Collider col, Vector3 drawPos, float fillAmount)
    {
        // If you can't see the collider with camera, don't draw healthbar 
        if (!IsColliderVisible(col))
        {
            return;
        }
        // Get healthbar from the prefabs 
        HealthBar bar = GetHealthBar();
        // Set the position of the healthbar
        Vector3 screenPos = droneCamera.WorldToScreenPoint(drawPos);
        RectTransform rect = bar.GetRect();
        rect.position = screenPos;
        // Set last draw frame of the healthbar 
        int thisFrame = Time.frameCount;
        bar.SetLastDrawRequestFrame(thisFrame);
        // Set the fill amount of the healthbar 
        bar.SetFillAmount(fillAmount);
    }

    bool IsColliderVisible(Collider col)
    {
        return GeometryUtility.TestPlanesAABB(planes, col.bounds);
    }

    HealthBar GetHealthBar()
    {
        // Find an healthbar that is not drawn this frame 
        HealthBar bar = healthBars.Find(hBar => !hBar.ShouldDrawThisFrame());

        if (bar == null)
        {
            // Create new healthbar 
            GameObject obj = Instantiate(healthBarPrefab, canvas.transform);
            bar = new HealthBar(obj);
            // Add the bar to healthBars
            healthBars.Add(bar);
        }

        return bar;
    }
}
