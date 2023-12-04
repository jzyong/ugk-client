using Network;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lobby.UI.Login
{
    public class LoginPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField accountInput;

        [SerializeField] private TMP_InputField passwordInput;

        [SerializeField] private Button loginButton;


        void Start()
        {
            loginButton.onClick.AddListener(Login);
            MessageEventManager.Singleton.AddEvent<LoginResponse>(MessageEvent.Login, LoginRes);
        }

        private void OnDestroy()
        {
            MessageEventManager.Singleton.RemoveEvent<LoginResponse>(MessageEvent.Login, LoginRes);
        }


        private void Login()
        {
            LoginRequest request = new LoginRequest()
            {
                Account = accountInput.text,
                Password = passwordInput.text
            };
            NetworkManager.Instance.Send(MID.LoginReq, request);
        }

        private void LoginRes(LoginResponse response)
        {
            // 登录成功
            if (response.Result == null || response.Result.Status == 200)
            {
                NetworkManager.Instance.Send(MID.LoadPlayerReq,new LoadPlayerRequest()
                {
                    PlayerId = response.PlayerId
                });
                SceneManager.LoadScene("Lobby");
            }
            else
            {
                // 给弹窗
                var noticePanel =  gameObject.GetComponentInChildren<NoticePanel>(true);
               
                if (noticePanel!=null)
                {
                    noticePanel.Show(response.Result.Msg);
                }
                Debug.LogWarning($"登录错误：{response.Result.Msg}");
            }
        }
    }
}