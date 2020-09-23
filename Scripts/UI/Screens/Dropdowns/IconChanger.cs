using UnityEngine;
using UnityEngine.UI;

public class IconChanger : MonoBehaviour
{
    public Sprite plane;
    public Sprite train;
    public Sprite car;

    Image current;
    
    public void ChangeTexture(string type)
    {
        current = gameObject.GetComponent(typeof(Image)) as Image;

        switch (type)
        {
            case "Plane":
                current.sprite = plane;
                break;
            case "Train":
                current.sprite = train;
                break;
            case "Car":
                current.sprite = car;
                break;
        }
    }

}
