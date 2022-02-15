using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

namespace ProjectCronos
{
    [RequireComponent(typeof(Rigidbody), typeof(Animator), typeof(PlayerStatus))]
    public class Player : Character
    {
        /// <summary>
        /// �ړ����x
        /// </summary>
        [SerializeField]
        int moveSpeed = 10;

        /// <summary>
        /// ��]���x
        /// </summary>
        [SerializeField]
        int rotateSpeed = 10;

        /// <summary>
        /// ���G�͈�
        /// </summary>
        [SerializeField]
        int searchRange = 10;

        /// <summary>
        /// ���W�b�h�{�f�B
        /// </summary>
        Rigidbody rigid;

        /// <summary>
        /// �A�j���[�^�[
        /// </summary>
        Animator anim;

        /// <summary>
        /// �ߋ��̈ʒu
        /// </summary>
        Vector3 latestPos;

        /// <summary>
        /// ���͕���
        /// </summary>
        Vector3 inputVec;

        /// <summary>
        /// �{
        /// </summary>
        [SerializeField]
        Book book;

        /// <summary>
        /// �ڕW�ƂȂ�I�u�W�F�N�g
        /// </summary>
        [SerializeField]
        TargetObject targetObj;

        /// <summary>
        /// �e���o��p�x
        /// </summary>
        [SerializeField]
        float bulletFreq = 0.5f;
        float bulletFreqTime = 0.0f;

        [SerializeField]
        GameObject head;

        /// <summary>
        /// �������Ă��镐��
        /// </summary>
        [SerializeField]
        Weapon weapon;

        /// <summary>
        /// �����Ă����Ԃ�
        /// </summary>
        bool isRun = false;

        /// <summary>
        /// �J�n����
        /// </summary>
        void Start()
        {
            Initialize();
        }

        /// <summary>
        /// ������
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            rigid = this.GetComponent<Rigidbody>();
            anim = this.GetComponent<Animator>();
        }

        /// <summary>
        /// �o��������
        /// </summary>
        public override void Appear()
        {
            // �v���C���[�͈�U�������Ȃ�
        }

        /// <summary>
        /// ��e��
        /// </summary>
        public override void Damage(int value)
        {
            base.Damage(value);
        }

        /// <summary>
        /// ���S��
        /// </summary>
        public override void Death()
        {
            base.Death();
        }

        /// <summary>
        /// Update
        /// </summary>
        void Update()
        {
            //if (Gamepad.current.leftStickButton.wasPressedThisFrame)
            //{
            //    isRun = !isRun;
            //    anim.SetBool("IsRun", isRun);
            //}

            Move();
            Shot();
            RockonTarget();
        }

        /// <summary>
        /// FixedUpdate
        /// </summary>
        void FixedUpdate()
        {

        }

        /// <summary>
        /// �e������
        /// </summary>
        void Shot()
        {
            bulletFreqTime -= Time.deltaTime;

            if (Gamepad.current.rightTrigger.ReadValue() > 0.1f)
            {
                if (bulletFreqTime < 0)
                {
                    // HACK: �݌v�����ŏC������K�v����
                    book.Shot(targetObj.IsTargetEnemy() ? targetObj.transform.position : Camera.main.transform.forward * 1000);
                    bulletFreqTime = bulletFreq;
                }
            }
        }

        /// <summary>
        /// �^�[�Q�b�g���b�N�I��
        /// </summary>
        void RockonTarget()
        {
            if (Gamepad.current.rightStickButton.wasPressedThisFrame)
            {
                // �^�[�Q�b�g������ꍇ�A�v���C���[�Ƀ^�[�Q�b�g��߂�
                if (targetObj.IsTargetEnemy())
                {
                    Debug.Log("���b�N�I������");
                    targetObj.SetTarget(head);
                    return;
                }

                // ���Ȃ���Εt�߂̈�ԋ߂��G��T���ă��b�N�I���������s��
                RockOn();
            }
        }

        /// <summary>
        /// ���b�N�I������
        /// </summary>
        public void RockOn()
        {
            var enemys = Physics.SphereCastAll(
                this.transform.position, searchRange, this.transform.forward, 0.01f)
                .Where(h => h.transform.gameObject.tag == "Enemy")
                .Select(h => h.transform.gameObject)
                .ToList();

            if (enemys.Count > 0)
            {
                float minDistance = searchRange;
                foreach (var obj in enemys)
                {
                    float dist = Vector3.Distance(this.transform.position, obj.transform.position);
                    if (dist < minDistance)
                    {
                        minDistance = dist;

                        targetObj.GetComponent<TargetObject>().SetTarget(obj);
                    }
                }
            }
            else
            {
                targetObj.SetTarget(head);
            }
        }

        /// <summary>
        /// �ړ�
        /// </summary>
        void Move()
        {
            var vec = GetDirection();
            var speed = moveSpeed;

            rigid.velocity = new Vector3(vec.x * speed, rigid.velocity.y, vec.z * speed);

            anim.SetFloat("Speed", rigid.velocity.magnitude);

            if (vec.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vec, Vector3.up), rotateSpeed * Time.deltaTime);
            }

            latestPos = transform.position;
        }

        /// <summary>
        /// ���͂���Ă���������擾(�R���g���[���[)
        /// </summary>
        /// <returns>�ړ����������Ԃ�</returns>
        Vector3 GetDirection()
        {
            return SetCameraDirection(new Vector3(inputVec.x, 0, inputVec.y));
        }

        /// <summary>
        /// ���̓C�x���g
        /// </summary>
        /// <param name="context"></param>
        public void OnMove(InputAction.CallbackContext context)
        {
            inputVec = context.ReadValue<Vector2>();
        }

        /// <summary>
        /// �J�����̌�������ړ��������v�Z���ĕԂ�
        /// </summary>
        /// <param name="inputDir">���͕���</param>
        /// <returns>�ړ�����</returns>
        Vector3 SetCameraDirection(Vector3 inputDir)
        {
            // �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
            Vector3 moveForward = cameraForward * inputDir.z + Camera.main.transform.right * inputDir.x;

            return moveForward;
        }
    }
}
