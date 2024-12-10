using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputPanel : MonoBehaviour
{
    public static InputPanel instance { get; private set; } = null;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Button acceptButton;
    [SerializeField] private TMP_InputField inputField;

    private CanvasGroupController cgController;

    public bool isWaitingForUserInput {  get; private set; }

    public string lastInput { get; private set; } = string.Empty;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cgController = new CanvasGroupController(this, canvasGroup);

        cgController.alpha = 0;
        cgController.SetInteractableState(active: false);

        acceptButton.gameObject.SetActive(false);

        inputField.onValueChanged.AddListener(OnInputChanged);
        acceptButton.onClick.AddListener(OnAcceptInput);
    }

    public void Show(string title)
    {
        titleText.text = title;
        inputField.text = string.Empty;
        cgController.Show();
        cgController.SetInteractableState(active: true);
        isWaitingForUserInput = true;
    }

    public void Hide()
    {
        cgController.Hide();
        cgController.SetInteractableState(active: false);
        isWaitingForUserInput = false;
    }

    private void OnAcceptInput()
    {
        if (inputField.text == string.Empty)
            return;

        lastInput = inputField.text;
        Hide();
    }



    public void OnInputChanged(string dummy)
    {
        acceptButton.gameObject.SetActive(HasValidText());
    }

    private bool HasValidText()
    {
        return inputField.text != string.Empty;
    }
}
