using UnityEngine;
using UnityEngine.UI;
using System;

namespace ProjectCronos
{
    /// <summary>
    /// �|�b�v�A�b�v�̃x�[�X�ƂȂ�N���X
    /// </summary>
    public abstract class PopupBase : MonoBehaviour
    {
        [SerializeField]
        Button positiveButton;
        [SerializeField]
        Button negativeButton;
        [SerializeField]
        Button otherButton;

        /// <summary>
        /// �|�W�e�B�u�{�^�������������̃R�[���o�b�N
        /// </summary>
        protected Action positiveAction;
        /// <summary>
        /// �l�K�e�B�u�{�^�������������̃R�[���o�b�N
        /// </summary>
        protected Action negativeAction;
        /// <summary>
        /// ���̑��{�^�������������̃R�[���o�b�N
        /// </summary>
        protected Action otherAction;
        /// <summary>
        /// �|�b�v�A�b�v��������̃R�[���o�b�N
        /// </summary>
        protected Action closeAction;

        void Start()
        {
            positiveButton.onClick.AddListener(OnClickPositiveButton);
            negativeButton.onClick.AddListener(OnClickNegativeButton);
            otherButton.onClick.AddListener(OnClickOtherButton);
        }

        public abstract void Setup(Action callback);

        public void ButtonSetup()
        {
            bool isExistPositiveAction = positiveAction != null;
            bool isExistNegativeAction = negativeAction != null;
            bool isExistOtherAction = otherAction != null;

            positiveButton.gameObject.SetActive(isExistPositiveAction);
            negativeButton.gameObject.SetActive(isExistNegativeAction);
            otherButton.gameObject.SetActive(isExistOtherAction);
        }

        void OnClickPositiveButton()
        {
            if (positiveAction != null)
            {
                positiveAction.Invoke();
            }
        }

        void OnClickNegativeButton()
        {
            if (negativeAction != null)
            {
                negativeAction.Invoke();
            }
        }

        void OnClickOtherButton()
        {
            if (otherButton != null)
            {
                otherAction.Invoke();
            }
        }

        /// <summary>
        /// �|�b�v�A�b�v����鏈��
        /// </summary>
        protected void Close()
        {
            if (closeAction != null)
            {
                closeAction.Invoke();
            }

            PopupManager.Instance.PopPopup();
            Destroy(this.gameObject);
        }
    }
}
