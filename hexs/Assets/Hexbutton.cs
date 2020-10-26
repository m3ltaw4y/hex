using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hexbutton : MonoBehaviour
{
    [SerializeField] private GameObject movingHexs;
    [SerializeField] private List<Image> hexs;
    [SerializeField] private Sprite lightHex;

    private bool spinnerActive;
    private bool hovering;

    private float lastAngle;
    private float targetAngle;
    
    private Vector3 center;
    public void OnTap()
    {
        var pos = movingHexs.transform.localPosition; 
        pos.y = -3.96f;
        movingHexs.transform.localPosition = pos;
        spinnerActive = true;
        foreach (var hex in hexs)
        {
            hex.sprite = lightHex;
        }

        center = transform.position;
    }

    public void HexHover(bool hover)
    {
        hovering = hover;
    }

    void Update()
    {
        if (!spinnerActive)
        {
            return;
        }

        var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var delta = point - center;
        double rads = Math.Atan2(delta.y, delta.x);
        var deg = (float)(Mathf.Rad2Deg * rads) - 30;
        Drift(deg);
    }

    void Drift(float deg)
    {
        deg += 360;
        lastAngle += 360;
        targetAngle += 360;
        
        var mod = deg % 60;

        if (hovering)
        {
            if (mod < 30)
            {
                targetAngle = (int) (deg / 60) * 60;
            }
            else if (mod >= 30)
            {
                targetAngle = ((int)(deg / 60)+1) * 60;
            }
        }

        if (hovering && Math.Abs(targetAngle - deg) > 15)
        {
            deg = lastAngle + (deg - lastAngle) * .8f;
        }
        else
        {
            deg = lastAngle + (targetAngle - lastAngle) * .3f;
        }
        
        targetAngle -= 360;
        deg -= 360;
        movingHexs.transform.rotation = Quaternion.Euler(0,0,deg);

        lastAngle = deg;
    }
}
