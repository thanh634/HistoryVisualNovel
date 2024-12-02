using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ColorExtensions
{
    public static Color SetAlpha(this Color orginial, float alpha)
    {
        return new Color(orginial.r, orginial.g, orginial.b, alpha);
    }

    public static Color GetColorFromName(this Color originial, string colorName)
    {
        switch (colorName.ToLower())
        {
            case "red":
                return Color.red;
            case "green":
                return Color.green;
            case "blue":
                return Color.blue;
            case "yellow":
                return Color.yellow;
            case "white":
                return Color.white;
            case "black":
                return Color.black;
            case "gray":
                return Color.gray;
            case "cyan":
                return Color.cyan;
            case "magenta":
                return Color.magenta;
            case "orange":
                return new Color(1f, 0.5f, 0f);
            default:
                Debug.LogWarning("Unrecognized color name: " + colorName);
                return Color.clear;


        }
    }
}
