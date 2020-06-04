using ClassicUO;
using ClassicUO.Configuration;
using ClassicUO.Network;
using ClassicUO.Utility.Logging;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoginConfigPanel : MonoBehaviour
{

    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject backPanel;
    [SerializeField] InputField serveripInput;
    [SerializeField] InputField serverportInput;
    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(() => {
            StartGame();
        });
        quitButton.onClick.AddListener(() => {
            Application.Quit(0);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        if (!File.Exists(Path.Combine(CUOEnviroment.ExecutablePath, "art.mul")))
        {
            // show a popup
            FindObjectOfType<UIPopup>().Show("提示", "请手动拷贝UO所有文件到如下位置:" + CUOEnviroment.ExecutablePath);
            return;
        }
        Hide();
        Settings.GlobalSettings.IP = serveripInput.text;
        Settings.GlobalSettings.Port = ushort.Parse(serverportInput.text);
    }

    public void Show() { loginPanel.SetActive(true);backPanel.SetActive(true); }
    public void Hide() { loginPanel.SetActive(false); backPanel.SetActive(false); }
}
