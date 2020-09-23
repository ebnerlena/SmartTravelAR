using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGyro : MonoBehaviour
{
    private void Start()
    {
        GyroManager.Instance.EnableGyro();
    }
  
    void Update()
    {
        if (MobileOnlyActivator.IsMobile)
        {
            var rot = GyroManager.Instance.GetGyroRotation();
            Quaternion quat = new Quaternion(rot.x, rot.y, -rot.z, rot.w);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, quat, Time.deltaTime * 5.0f);
        }
       //this.transform.parent.transform.rotation = Quaternion.Euler(transform.parent.transform.eulerAngles.x, transform.parent.transform.eulerAngles.y,0); 
    }

}
