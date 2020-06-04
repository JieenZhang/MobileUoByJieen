// Note: this script has to be on an always-active UI parent, so that we can
// always find it from other code. (GameObject.Find doesn't find inactive ones)
using UnityEngine;
using UnityEngine.UI;

public class UIPopup : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Text titleText;
    [SerializeField] Text messageText;
    [SerializeField] Button okButton;

    void Start()
    {
        okButton.onClick.AddListener(() => {
            titleText.text = "";
            messageText.text = "";
            panel.SetActive(false);
        });
    }

    public void Show(string title, string message)
    {
        if(panel.activeSelf)
        {
            return;
        }
        // append error if visible, set otherwise. then show it.
        //if (panel.activeSelf)
        {
            titleText.text = title;
            messageText.text = message;
        }
        panel.SetActive(true);
    }
}
