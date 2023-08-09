using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// テクスチャを揺らめかせる機能を提供するクラス
    /// </summary>
    public class WaveTexture : MonoBehaviour
    {
        List<SpriteRenderer> srList;

        [SerializeField]
        float pixelsPerUnit = 100;//数値が大きくなるほど小さく表示されます

        [SerializeField]
        float hunyoSpeed, huyoHuyoDO, waveSpeed, waveWaveDO;//こちらの値を変更するとスピードとか変わります。

        [SerializeField]
        int cutCount;//画像をスライスする枚数

        [SerializeField]
        Texture2D texture;//ここに対象の画像を貼り付けます。

        // Start is called before the first frame update
        void Start()
        {
            srList = new List<SpriteRenderer>();

            int i = 0;
            foreach (var item in GetSlicedTexture())
            {
                var obj = new GameObject("sprite_" + i);
                obj.transform.SetParent(transform);
                obj.AddComponent<SpriteRenderer>();
                srList.Add(obj.GetComponent<SpriteRenderer>());
                srList[i].sprite = item;
                i++;
            }

            float height = srList[0].bounds.size.y;
            float top = (srList.Count / 2f) * height * -1;
            foreach (var item in srList)
            {
                item.transform.localPosition = new Vector3(0, top);
                top += height;
            }
        }

        private void FixedUpdate()
        {
            float i_huyo = 0, i_wave = 0;

            foreach (var item in srList)
            {
                float xsize = Mathf.Sin((Time.frameCount * Time.deltaTime * huyoHuyoDO) + i_huyo / 20f) * 0.48f + 1;
                i_huyo += hunyoSpeed;

                item.transform.localScale = new Vector3(xsize, item.transform.localScale.y);

                float xpos = Mathf.Sin((Time.frameCount * Time.deltaTime * waveWaveDO) + i_wave / 20f) * 0.48f;//ココの 0.48f の部分が「ふり幅」です。値を変えると大きく動くようになります。
                i_wave += waveSpeed;

                item.transform.localPosition = new Vector3(xpos, item.transform.localPosition.y);
            }

        }

        public Sprite[] GetSlicedTexture()//スクリプトからtextureを弄る場合、gameobjectを破棄してもテクスチャやスプライトは残るらしいので Destroy('テクスチャインスタンス')などの処理が必要っぽいです
        {
            Sprite[] sprites = new Sprite[cutCount];

            for (int y = 0; y < cutCount; y++)
            {
                Sprite sprite = Sprite.Create(
                    texture,
                    new Rect(0, 1f * texture.height / cutCount * y, texture.width, 1f * texture.height / cutCount),
                    new Vector2(0.5f, 0.5f),
                    pixelsPerUnit
                    );
                sprites[y] = sprite;
            }

            return sprites;
        }
    }
}
