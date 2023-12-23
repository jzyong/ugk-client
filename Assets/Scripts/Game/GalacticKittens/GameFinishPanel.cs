using System;
using Game.GalacticKittens.Manager;
using Game.GalacticKittens.Player;
using Lobby;
using Network;
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

        [SerializeField] private GameObject victoryRenderer;
        [SerializeField] private GameObject defeatedRenderer;
        [SerializeField] private Vector3[] scorePositions;
        [SerializeField] private AudioClip victoryAudio;
        [SerializeField] private AudioClip defeatedaudio;


        private void Start()
        {
            backButton.onClick.AddListener(BackLobby);
            SetUI();
        }

        private void OnEnable()
        {
            MessageEventManager.Instance.AddEvent<GalacticKittensGameFinishResponse>(
                MessageEvent.GalacticKittensGameFinish, SetUI);
        }

        private void OnDisable()
        {
            MessageEventManager.Instance.RemoveEvent<GalacticKittensGameFinishResponse>(
                MessageEvent.GalacticKittensGameFinish, SetUI);
        }


        private void SetUI()
        {
            if (DataManager.Instance.GalacticKittens.GameFinishResponse != null)
            {
                SetUI(DataManager.Instance.GalacticKittens.GameFinishResponse);
            }
        }

        public void SetUI(GalacticKittensGameFinishResponse response)
        {
            if (response.Victory)
            {
                victoryRenderer.SetActive(true);
                GalacticKittensAudioManager.Instance.PlaySoundEffect(victoryAudio);
            }
            else
            {
                defeatedRenderer.SetActive(true);
                GalacticKittensAudioManager.Instance.PlaySoundEffect(defeatedaudio);
            }

            int i = 0;
            foreach (var statistic in response.Statistics)
            {
                Spaceship spaceship = DataManager.Instance.GalacticKittens.Spaceships[statistic.PlayerId];
                GameObject go = Instantiate(spaceship._characterDataSo.spaceshipScorePrefab, scorePositions[i],
                    Quaternion.identity);
                go.GetComponent<PlayerShipScore>().SetShip(statistic.Victory, (int)statistic.KillCount,
                    (int)statistic.UsePowerCount, (int)statistic.Score);
                i++;
            }
        }


        private void BackLobby()
        {
            DestroyImmediate(GalacticKittensAudioManager.Instance);
            SceneManager.LoadScene("Scenes/Lobby");
        }
    }
}