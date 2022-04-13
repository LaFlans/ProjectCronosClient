using UnityEngine;
using TMPro;
using System;

namespace ProjectCronos
{
    /// <summary>
    /// �f�t�H���g�|�b�v�A�b�v
    /// </summary>
    class DefaultPopup : PopupBase
    {
        [SerializeField]
        TextMeshProUGUI titleText;
        [SerializeField]
        TextMeshProUGUI messageText;
        [SerializeField]
        TextMeshProUGUI positiveButtonMessageText;
        [SerializeField]
        TextMeshProUGUI negativeButtonMessageText;
        [SerializeField]
        TextMeshProUGUI otherButtonMessageText;
        [SerializeField]
        CanvasGroup canvasGroup;

        bool isSetup = false;
        bool isOpenAnimation = false;

        public override void Setup(Action callback)
        {
            canvasGroup.alpha = 0;

            // ���b�Z�[�W�ݒ�
            titleText.text = "�f�t�H���g�|�b�v�A�b�v";
            messageText.text = "�e�X�g�p�̃|�b�v�A�b�v�ł�";
            positiveButtonMessageText.text = "YES";
            negativeButtonMessageText.text = "NO";
            otherButtonMessageText.text = "OTHER";

            // �A�N�V�����ݒ�
            positiveAction = OnClickPositiveButton;
            negativeAction = OnClickNegativeButton;
            otherAction = OnClickOtherButton;
            closeAction = callback;

            // �{�^���̐ݒ�
            ButtonSetup();

            isSetup = true;
        }

        void Update()
        {
            if (!isOpenAnimation && isSetup)
            {
                isOpenAnimation = true;
                canvasGroup.alpha = 1;

                // �A�j���[�V�����J�n
                GetComponent<Animator>().SetTrigger("Open");
            }
        }

        void OnClickPositiveButton()
        {
            Debug.Log("�|�W�e�B�u�{�^������������I");
            Close();
        }

        void OnClickNegativeButton()
        {
            Debug.Log("�l�K�e�B�u�{�^������������I");
            Close();
        }

        void OnClickOtherButton()
        {
            Debug.Log("���̑��{�^������������I");
            Close();
        }
    }
}