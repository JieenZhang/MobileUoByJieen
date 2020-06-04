using ClassicUO;
using ClassicUO.Game;
using ClassicUO.Game.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] float zoomMin = 5;
    [SerializeField] float zoomMax = 50;
    [SerializeField] float zoomStepSize = 5;
    [SerializeField] Button plusButton;
    [SerializeField] Button minusButton;

    // Start is called before the first frame update
    void Start()
    {
        plusButton.onClick.AddListener(() => {
            GameScene gs = Client.Game.GetScene<GameScene>();
            if(gs != null)
            {
                gs.ZoomOut();
            }
        });
        minusButton.onClick.AddListener(() => {
            GameScene gs = Client.Game.GetScene<GameScene>();
            if (gs != null)
            {
                gs.ZoomIn();
            }
        });

        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(World.Player != null && !panel.activeSelf)
        {
            panel.SetActive(World.Player != null); // hide while not in the game world
        }
        else
        {
            panel.SetActive(World.Player != null); // game quit need hide
        }
    }
}
