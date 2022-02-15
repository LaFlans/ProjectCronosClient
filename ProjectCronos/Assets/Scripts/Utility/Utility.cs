using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectCronos
{
    public class Utility : MonoBehaviour
    {
        #region Prefab

        /// <summary>
        /// �X�N���[�����W�ł̃v���n�u�𐶐�
        /// </summary>
        /// <param name="path">�v���n�u�̃p�X</param>
        /// <param name="pos">�v���n�u�𐶐�����ʒu</param>
        /// <param name="parent">�v���n�u�̐e�ɂ���I�u�W�F�N�g</param>
        public static void CreateUIPrefab(string path, Vector3 pos, GameObject parent)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            Vector3 rectPos = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
            Instantiate(prefab, rectPos, Quaternion.identity, parent.transform);
        }

        /// <summary>
        /// �X�N���[�����W�ł̃v���n�u�𐶐�
        /// </summary>
        /// <param name="path">�v���n�u�̃p�X</param>
        /// <param name="pos">�v���n�u�𐶐�����ʒu</param>
        /// <param name="time">�v���n�u����������</param>
        /// <param name="parent">�v���n�u�̐e�ɂ���I�u�W�F�N�g</param>
        public static void CreateUIPrefab(string path, Vector3 pos, float time, GameObject parent)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            Vector3 rectPos = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
            GameObject obj = Instantiate(prefab, rectPos, Quaternion.identity, parent.transform);
            Destroy(obj, time);
        }

        /// <summary>
        /// �v���n�u�̃T�E���h�𐶐����Ďw�肵�����Ԍ�ɏ���
        /// <param name="path">�T�E���h�v���n�u�̃p�X</param>
        /// <param name="time">�v���n�u����������</param>
        /// </summary>
        public static void CreateSE(string path, float time)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab);
            Destroy(obj, time);
        }

        /// <summary>
        ///�@�v���n�u�𐶐����ăQ�[���I�u�W�F�N�g��Ԃ�
        /// </summary>
        /// <param name="path"> �v���n�u�̃p�X</param>
        /// <returns>�v���n�u�̃Q�[���I�u�W�F�N�g</returns>
        public static GameObject CreatePrefab(string path)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            return Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// �v���n�u�𐶐����Ďw�肵�����Ԍ�ɏ���
        /// <param name="path">�v���n�u�̃p�X</param>
        /// <param name="time">�v���n�u����������</param>
        /// </summary>
        public static void CreatePrefab(string path, float time)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            Destroy(obj, time);
        }

        /// <summary>
        /// �v���n�u���w�肵���ʒu�ɐ������Ďw�肵�����Ԍ�ɏ���
        /// <param name="path">�v���n�u�̃p�X</param>
        /// <param name="pos">�v���n�u�𐶐�����ʒu</param>
        /// <param name="time">�v���n�u����������</param>
        /// </summary>
        public static void CreatePrefab(string path, Vector3 pos, float time)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
            Destroy(obj, time);
        }

        /// <summary>
        /// �v���n�u���w�肵���ʒu�ɐ������Ďw�肵���e���Z�b�g���Ďw�肵�����Ԍ�ɏ���
        /// <param name="path">�v���n�u�̃p�X</param>
        /// <param name="pos">�v���n�u�𐶐�����ʒu</param>
        /// <param name="time">�v���n�u����������</param>
        /// <param name="parent">�v���n�u�̐e�ɂ���I�u�W�F�N�g</param>
        /// </summary>
        public static void CreatePrefab(string path, Vector3 pos, float time, GameObject parent)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity, parent.transform);
            Destroy(obj, time);
        }

        /// <summary>
        /// �v���n�u���w�肵���ʒu�Ɖ�]�Ő������Ďw�肵�����Ԍ�ɏ���
        /// <param name="path">�v���n�u�̃p�X</param>
        /// <param name="pos">�v���n�u�𐶐�����ʒu</param>
        /// <param name="rotation">�v���n�u�𐶐�����p�x</param>
        /// <param name="time">�v���n�u����������</param>
        /// </summary>
        public static void CreatePrefab(string path, Vector3 pos, Quaternion rotation, float time)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab, pos, rotation);
            Destroy(obj, time);
        }
        /// <summary>
        /// �v���n�u���w�肵���ʒu�Ɖ�]�Ő������Ďw�肵�����Ԍ�ɏ���
        /// <param name="path">�v���n�u�̃p�X</param>
        /// <param name="pos">�v���n�u�𐶐�����ʒu</param>
        /// <param name="rotation">�v���n�u�𐶐�����p�x</param>
        /// <param name="time">�v���n�u����������</param>
        /// <param name="parent">�v���n�u�̐e�ɂ���I�u�W�F�N�g</param>
        /// </summary>
        public static void CreatePrefab(string path, Vector3 pos, Quaternion rotation, float time, GameObject parent)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab, pos, rotation, parent.transform);
            Destroy(obj, time);
        }

        /// <summary>
        /// �v���n�u���w�肵���ʒu�Ɖ�]�Ő�������
        /// </summary>
        /// <param name="path">�v���n�u�̃p�X</param>
        /// <param name="pos">�v���n�u�𐶐�����ʒu</param>
        /// <param name="rotation">�v���n�u�𐶐�����p�x</param>
        public static void CreatePrefab(string path, Vector3 pos, Quaternion rotation)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            Instantiate(prefab, pos, rotation);
        }

        /// <summary>
        /// �v���n�u���w�肵���ʒu�ƃI�C���[�p�Ő�������
        /// </summary>
        /// <param name="path">�v���n�u�̃p�X</param>
        /// <param name="pos">�v���n�u�𐶐�����ʒu</param>
        /// <param name="eularAngle">�v���n�u�𐶐�����p�x(�I�C���[�p)</param>
        public static void CreatePrefab(string path, Vector3 pos, Vector3 eularAngle)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
            obj.transform.eulerAngles = eularAngle;
        }

        /// <summary>
        /// �v���n�u���w�肵���ʒu�ƃI�C���[�p�Ő�������
        /// </summary>
        /// <param name="path">�v���n�u�̃p�X</param>
        /// <param name="pos">�v���n�u�𐶐�����ʒu</param>
        /// <param name="eularAngle">�v���n�u�𐶐�����p�x(�I�C���[�p)</param>
        /// <param name="parent">�v���n�u�̐e�ɂ���I�u�W�F�N�g</param>
        public static void CreatePrefab(string path, Vector3 pos, Vector3 eularAngle, GameObject parent)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity, parent.transform);
            obj.transform.eulerAngles = eularAngle;
        }
        /// <summary>
        /// �v���n�u���w�肵���ʒu�ƃI�C���[�p�Ő�������
        /// </summary>
        /// <param name="path">�v���n�u�̃p�X</param>
        /// <param name="pos">�v���n�u�𐶐�����ʒu</param>
        /// <param name="eularAngle">�v���n�u�𐶐�����p�x(�I�C���[�p)</param>
        /// <param name="time">�v���n�u����������</param>
        public static void CreatePrefab(string path, Vector3 pos, Vector3 eularAngle, float time)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
            obj.transform.eulerAngles = eularAngle;
            Destroy(obj, time);
        }


        /// <summary>
        /// �v���n�u���w�肵���ʒu�Ő�������
        /// </summary>
        /// <param name="path">�v���n�u�̃p�X</param>
        /// <param name="pos">�v���n�u�𐶐�����ʒu</param>
        public static void CreatePrefab(string path, Vector3 pos)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            Instantiate(prefab, pos, Quaternion.identity);
        }
        /// <summary>
        /// �v���n�u���w�肵���ʒu�ɐ������Ďw�肵���e���Z�b�g���Ďw�肵�����Ԍ�ɏ���
        /// <param name="path">�v���n�u�̃p�X</param>
        /// <param name="time">�v���n�u����������</param>
        /// <param name="parent">�v���n�u�̐e�ɂ���I�u�W�F�N�g</param>
        /// </summary>
        public static void CreatePrefab(string path, float time, GameObject parent)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent.transform);
            Destroy(obj, time);
        }
        /// <summary>
        /// �v���n�u���w�肵���ʒu�ɐ������Ďw�肵���e���Z�b�g���Ďw�肵�����Ԍ�ɏ���
        /// <param name="path">�v���n�u�̃p�X</param>
        /// <param name="pos">�v���n�u�𐶐�����ʒu</param>
        /// <param name="parent">�v���n�u�̐e�ɂ���I�u�W�F�N�g</param>
        /// </summary>
        public static void CreatePrefab(string path, Vector3 pos, GameObject parent)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            Instantiate(prefab, pos, Quaternion.identity, parent.transform);
        }

        #endregion

        #region Clone
        //****************************************************************************************************************************************************************************************
        //�@�N���[��
        //****************************************************************************************************************************************************************************************
        /// <summary>
        /// �Q�[���I�u�W�F�N�g�̃N���[���𐶐�����
        /// </summary>
        /// <param name="go"></param>
        /// <returns>�N���[���Q�[���I�u�W�F�N�g</returns>
        public static GameObject CreateClone(GameObject go)
        {
            var clone = GameObject.Instantiate(go) as GameObject;
            clone.transform.localPosition = go.transform.localPosition;
            clone.transform.localScale = go.transform.localScale;
            return clone;
        }
        #endregion

        #region Material
        /// <summary>
        /// �^�[�Q�b�g�̎q�I�u�W�F�N�g�܂ނ��ׂẴI�u�W�F�N�g�̃}�e���A���J���[���w�肵���F�ɕς���
        /// </summary>
        /// <param name="targetObject">�^�[�Q�b�g�I�u�W�F�N�g</param>
        /// <param name="color">�ύX�������F</param>
        public static void ChangeMaterialOfGameObject(GameObject targetObject, Material mat)
        {
            //���͂��ꂽ�I�u�W�F�N�g��Renderer��S�Ď擾���A����ɂ���Renderer�ɐݒ肳��Ă���SMaterial��ς���
            foreach (Renderer targetRenderer in targetObject.GetComponents<Renderer>())
            {
                targetRenderer.material = mat;
                //foreach (Material material in targetRenderer.materials)
                //{
                //    material.color = color;
                //}
            }

            //���͂��ꂽ�I�u�W�F�N�g�̎q�ɂ����l�̏������s��
            for (int i = 0; i < targetObject.transform.childCount; i++)
            {
                ChangeMaterialOfGameObject(targetObject.transform.GetChild(i).gameObject, mat);
            }
        }
        /// <summary>
        /// �^�[�Q�b�g�̎q�I�u�W�F�N�g�܂ނ��ׂẴI�u�W�F�N�g�̃}�e���A���J���[���w�肵���F�ɕς���
        /// </summary>
        /// <param name="targetObject">�^�[�Q�b�g�I�u�W�F�N�g</param>
        /// <param name="color">�ύX�������F</param>
        public static void ChangeColorOfGameObject(GameObject targetObject, Color color)
        {
            //���͂��ꂽ�I�u�W�F�N�g��Renderer��S�Ď擾���A����ɂ���Renderer�ɐݒ肳��Ă���SMaterial�̐F��ς���
            foreach (Renderer targetRenderer in targetObject.GetComponents<Renderer>())
            {
                foreach (Material material in targetRenderer.materials)
                {
                    material.color = color;
                }
            }

            //���͂��ꂽ�I�u�W�F�N�g�̎q�ɂ����l�̏������s��
            for (int i = 0; i < targetObject.transform.childCount; i++)
            {
                ChangeColorOfGameObject(targetObject.transform.GetChild(i).gameObject, color);
            }
        }
        /// <summary>
        /// �^�[�Q�b�g�̎q�I�u�W�F�N�g�܂ނ��ׂẴI�u�W�F�N�g�̎w�肵���}�e���A���J���[���w�肵���F�ɕς���
        /// </summary>
        /// <param name="targetObject">�^�[�Q�b�g�I�u�W�F�N�g</param>
        /// <param name="colorName">�ύX�������J���[�̖��O</param>
        /// <param name="color">�ύX�������F</param>
        public static void ChangeColorOfGameObject(GameObject targetObject, string colorName, Color color)
        {
            //���͂��ꂽ�I�u�W�F�N�g��Renderer��S�Ď擾���A����ɂ���Renderer�ɐݒ肳��Ă���SMaterial�̐F��ς���
            foreach (Renderer targetRenderer in targetObject.GetComponents<Renderer>())
            {

                foreach (Material material in targetRenderer.materials)
                {
                    material.SetColor(colorName, color);
                }
            }

            //���͂��ꂽ�I�u�W�F�N�g�̎q�ɂ����l�̏������s��
            for (int i = 0; i < targetObject.transform.childCount; i++)
            {
                ChangeColorOfGameObject(targetObject.transform.GetChild(i).gameObject, colorName, color);
            }
        }



        #endregion

        #region IEnumerator

        //�@�ړI�n(target)�ƖړI�n�܂ł̎���(time)
        public static IEnumerator ToTargetPos(GameObject obj, Vector3 target, float time)
        {
            Vector3 startPos = obj.transform.position;
            Vector3 endPos = target;

            float startTime = Time.time; //�@�����\�莞��
            var diff = Time.time - startTime;
            while (time > diff)
            {
                diff = Time.time - startTime; //�@�o�ߎ���
                var rate = diff / time;
                obj.transform.position = Vector3.Lerp(startPos, endPos, rate);
                yield return null;
            }
            obj.transform.position = new Vector3(endPos.x, endPos.y, endPos.z);

            yield return null;
        }
        //�@�ړI�n(target)�ƖړI�n�܂ł̎���(time)
        public static IEnumerator ToTargetLookPos(GameObject obj, Vector3 target, float time)
        {
            Vector3 startPos = obj.transform.position;
            Vector3 endPos = target;

            float startTime = Time.time; //�@�����\�莞��
            var diff = Time.time - startTime;
            while (time > diff)
            {
                diff = Time.time - startTime; //�@�o�ߎ���
                var rate = diff / time;
                obj.transform.position = Vector3.Lerp(startPos, endPos, rate);

                obj.transform.LookAt(target);
                yield return null;
            }
            obj.transform.position = new Vector3(endPos.x, endPos.y, endPos.z);

            yield return null;
        }

        //�@�t�F�[�h�C��
        public static IEnumerator FadeIn(Image fadeImage, float speed)
        {
            while (fadeImage.color.a > 0)
            {
                fadeImage.color -= new Color(0, 0, 0, speed);
                yield return new WaitForSeconds(0.01f);
            }
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
            yield return null;
        }
        //�@�t�F�[�h�A�E�g
        public static IEnumerator FadeOut(Image fadeImage, float speed)
        {
            while (fadeImage.color.a < 1)
            {
                fadeImage.color += new Color(0, 0, 0, speed);
                yield return new WaitForSeconds(0.01f);
            }
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
            yield return null;
        }
        #endregion

        #region UI
        /// <summary>
        /// �^�[�Q�b�g�̎q�I�u�W�F�N�g�܂ނ��ׂẴI�u�W�F�N�g�̃e�L�X�g�J���[���w�肵���F�ɕς���
        /// </summary>
        /// <param name="targetText">�^�[�Q�b�g�e�L�X�g</param>
        /// <param name="color">�ύX�������F</param>
        public static void ChangeColorOfText(GameObject targetText, Color color)
        {
            //���͂��ꂽ�I�u�W�F�N�g��Text��S�Ď擾���A�e�L�X�g�J���[��ς���
            foreach (Text target in targetText.GetComponents<Text>())
            {
                target.color = color;
            }

            //���͂��ꂽ�I�u�W�F�N�g�̎q�ɂ����l�̏������s��
            for (int i = 0; i < targetText.transform.childCount; i++)
            {
                ChangeColorOfGameObject(targetText.transform.GetChild(i).gameObject, color);
            }
        }

        #endregion
    }
}
