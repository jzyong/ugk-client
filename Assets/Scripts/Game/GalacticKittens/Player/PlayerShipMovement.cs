using System;
using Network;
using Network.Sync;
using UnityEngine;

namespace Game.GalacticKittens.Player
{
    /// <summary>
    /// 飞船移动控制
    /// </summary>
    public class PlayerShipMovement : MonoBehaviour
    {
        enum MoveType
        {
            constant,
            momentum
        }

       public  enum VerticalMovementType
        {
            none,
            upward,
            downward
        }

        [Serializable]
        public struct PlayerLimits
        {
            public float minLimit;
            public float maxLimit;
        }

        [SerializeField] MoveType m_moveType = MoveType.momentum;

        [SerializeField] PlayerLimits m_verticalLimits;

        [SerializeField] PlayerLimits m_hortizontalLimits;

        [Header("ShipSprites")] [SerializeField]
        SpriteRenderer m_shipRenderer;

        [SerializeField] Sprite m_normalSprite;

        [SerializeField] Sprite m_upSprite;

        [SerializeField] Sprite m_downSprite;

        [SerializeField] private float m_speed;

        private float m_inputX;
        private float m_inputY;

        private VerticalMovementType m_previousVerticalMovementType = VerticalMovementType.none;
        private VerticalMovementType m_currentVerticalMovementType = VerticalMovementType.none;

        const string k_horizontalAxis = "Horizontal";
        const string k_verticalAxis = "Vertical";

        private SnapTransform _snapTransform;


        private void Start()
        {
            _snapTransform = GetComponent<SnapTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            // 需要判断是否为自己
            if (!_snapTransform.IsOnwer)
                return;

            HandleKeyboardInput();

            UpdateVerticalMovementSprite();

            AdjustInputValuesBasedOnPositionLimits();

            MovePlayerShip();
        }

        private void HandleKeyboardInput()
        {
            /*
            There two types of movement:
            constant -> linear move, there are no time acceleration
            momentum -> move with acceleration at the start

            Note: feel free to add your own type as well
        */
            if (m_moveType == MoveType.constant)
            {
                HandleMoveTypeConstant();
            }
            else if (m_moveType == MoveType.momentum)
            {
                HandleMoveTypeMomentum();
            }
        }

        /// <summary>
        /// 匀速运动
        /// </summary>
        private void HandleMoveTypeConstant()
        {
            m_inputX = 0f;
            m_inputY = 0f;

            // Horizontal input
            if (Input.GetKey(KeyCode.D))
            {
                m_inputX = 1f;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                m_inputX = -1f;
            }

            // Vertical input and set the ship sprite
            if (Input.GetKey(KeyCode.W))
            {
                m_inputY = 1f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                m_inputY = -1f;
            }
        }

        /// <summary>
        /// 动量运动
        /// </summary>
        private void HandleMoveTypeMomentum()
        {
            m_inputX = Input.GetAxis(k_horizontalAxis);
            m_inputY = Input.GetAxis(k_verticalAxis);
        }

        private void UpdateVerticalMovementSprite()
        {
            m_previousVerticalMovementType = m_currentVerticalMovementType;

            UpdateCurrentVerticalMovementType();

            if (m_currentVerticalMovementType != m_previousVerticalMovementType)
            {
                // inform the server of the update to vertical movement type
                GalacticKittensShipMoveStateRequest request = new GalacticKittensShipMoveStateRequest()
                {
                    State = (uint)m_currentVerticalMovementType
                };
                NetworkManager.Instance.Send(MID.GalacticKittensShipMoveStateReq, request);
            }
        }

        // TODO 服务器返回消息进行调用设置
        private void UpdateCurrentVerticalMovementType()
        {
            // Change the ship sprites base on the input value
            if (m_inputY > 0f)
            {
                m_shipRenderer.sprite = m_upSprite;
                m_currentVerticalMovementType = VerticalMovementType.upward;
            }
            else if (m_inputY < 0f)
            {
                m_shipRenderer.sprite = m_downSprite;
                m_currentVerticalMovementType = VerticalMovementType.downward;
            }
            else
            {
                m_shipRenderer.sprite = m_normalSprite;
                m_currentVerticalMovementType = VerticalMovementType.none;
            }
        }


        /// <summary>
        /// 飞船飞行状态  
        /// </summary>
        /// <param name="newVerticalMovementType"></param>
        public void NewVerticalMovementClientType(VerticalMovementType newVerticalMovementType)
        {
            switch (newVerticalMovementType)
            {
                case VerticalMovementType.none:
                    m_shipRenderer.sprite = m_normalSprite;
                    break;
                case VerticalMovementType.upward:
                    m_shipRenderer.sprite = m_upSprite;
                    break;
                case VerticalMovementType.downward:
                    m_shipRenderer.sprite = m_downSprite;
                    break;
            }
        }

        // Check the limits of the player and adjust the input
        private void AdjustInputValuesBasedOnPositionLimits()
        {
            PlayerMovementInputLimitAdjuster.AdjustInputValuesBasedOnPositionLimits(
                transform.position,
                ref m_inputX,
                ref m_inputY,
                m_hortizontalLimits,
                m_verticalLimits
            );
        }

        private void MovePlayerShip()
        {
            // Take the value from the input and multiply by speed and time
            // Debug.LogWarning($"横向速度：{m_inputX} 纵向速度：{m_inputY}");
            float speedTimesDeltaTime = m_speed * Time.deltaTime;

            float newYposition = m_inputY * speedTimesDeltaTime;
            float newXposition = m_inputX * speedTimesDeltaTime;

            // move the ship
            transform.Translate(newXposition, newYposition, 0f, Space.World);
        }
    }
}