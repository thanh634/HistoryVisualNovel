using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    public Button myButton; // Reference to your Button component

    void Start()
    {
        // Ensure the button is assigned
        if (myButton != null)
        {
            // Add a listener to the button's onClick event
            myButton.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("Button reference is missing!");
        }
    }

    // Method called when the button is clicked
    private void OnButtonClick()
    {
        Debug.Log("Button clicked!");
        // Add your custom functionality here
    }

    // Optional: Remove the listener to avoid potential memory leaks
    void OnDestroy()
    {
        if (myButton != null)
        {
            myButton.onClick.RemoveListener(OnButtonClick);
        }
    }
}
