using System.Collections;
using System.Collections.Generic;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI.Login
{
    public class LoginPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField accountInput;

        [SerializeField] private TMP_InputField passwordInput;

        [SerializeField]
        private Button loginButton;
        

        void Start()
        {
            loginButton.onClick.AddListener(Login);
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void Login()
        {
            LoginRequest request = new LoginRequest()
            {
                Account = accountInput.text,
                Password = passwordInput.text
            };
            NetworkManager.singleton.Send(MID.LoginReq,request);
        }
    }
}