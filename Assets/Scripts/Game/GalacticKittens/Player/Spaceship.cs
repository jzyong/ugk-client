using Network;
using Network.Sync;
using UnityEngine;

namespace Game.GalacticKittens.Player
{
    /// <summary>
    /// 飞船
    /// </summary>
    public class Spaceship : MonoBehaviour
    {
        private SnapTransform _snapTransform;


        public long Id { set; get; }


        private void Start()
        {
            _snapTransform = GetComponent<SnapTransform>();
        }

        private void Update()
        {
            //TODO 监听按键事件，进行移动

            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireReq();
            }
        }


        //玩家开火
        private void FireReq()
        {
            GalacticKittensFireRequest request = new GalacticKittensFireRequest();
            NetworkManager.Instance.Send(MID.GalacticKittensFireReq, request);
        }
    }
}