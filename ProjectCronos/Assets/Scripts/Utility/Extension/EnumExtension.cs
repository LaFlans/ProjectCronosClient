using System;
using System.ComponentModel;
using System.Linq;

namespace ProjectCronos
{
    internal static class EnumExtension
    {
        /// <summary>
        /// EnumのValueからDescriptionを取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns>設定されているDescriptionを返す、設定されていない場合、Enumを文字列にして返す</returns>
        public static string GetDescriptionFromValue<T>(this T value) where T : Enum //where T : Enum とすることで Tがenumでない場合はコンパイル時にエラーにしてくれる
        {
            //valueはenum型確定なので空文字が返ることはない
            string strValue = value.ToString();

            var description =
                typeof(T).GetField(strValue)    //FiledInfoを取得
                .GetCustomAttributes(typeof(DescriptionAttribute), false)   //DescriptionAttributeのリストを取得
                .Cast<DescriptionAttribute>()   //DescriptionAttributeにキャスト
                .FirstOrDefault()               //最初の一つを取得、なければnull
                ?.Description;                  //DescriptionAttributeがあればDescriptionを、なければnullを返す

            return description ?? strValue;     //descriptionがnullならstrValueを返す
        }
    }
}
