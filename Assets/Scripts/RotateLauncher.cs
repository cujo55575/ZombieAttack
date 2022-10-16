using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RotateLauncher : MonoBehaviour
{
    void Update()
    {

        Vector3 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        Vector3 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        transform.rotation = Quaternion.Euler(new Vector3(0f, -angle, -4f));
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg;
    }

}
