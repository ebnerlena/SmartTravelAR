/*using GoogleARCore;
using UnityEngine;

public class BoxScaler : MonoBehaviour
{
    public GameObject box;
    public float x_in_m, z_in_m;

    private float x_scale_normalized, z_scale_normalized;
    private float x_y_ratio;

    void Start()
    {
        Vector3 m_scale = box.transform.localScale;

        x_scale_normalized = (m_scale.x / x_in_m);
        z_scale_normalized = (m_scale.z / z_in_m);
        
        x_y_ratio = m_scale.x / m_scale.y;
    }

    public void ScaleDown(AugmentedImage image) {
        
        float x_new = x_scale_normalized * image.ExtentX;
        float z_new = z_scale_normalized * image.ExtentZ;
        float y_new = x_new * x_y_ratio;
        
        box.transform.localScale = new Vector3(x_new, y_new, z_new);
    }
} */
