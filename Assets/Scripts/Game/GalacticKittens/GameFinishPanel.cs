using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.GalacticKittens
{
    /// <summary>
    /// 游戏结束
    /// </summary>
    public class GameFinishPanel : MonoBehaviour
    {
        [SerializeField] private Button backButton;

        //TODO 游戏结果面板

        private void Start()
        {
            backButton.onClick.AddListener(BackLobby);
        }

        private void BackLobby()
        {
            SceneManager.LoadScene("Scenes/Lobby");
        }
    }
}