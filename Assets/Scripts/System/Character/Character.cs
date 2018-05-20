using System.Collections.Generic;
using UnityEngine;


namespace Defence.CharacterPackage
{
    public class Character
    {
        public enum TYPE_FACE { Normal, Smile, Angry }

        string m_key;
        Sprite m_icon;
        string m_name;

        Dictionary<TYPE_FACE, Sprite> faceDic = new Dictionary<TYPE_FACE, Sprite>();

        public string key { get { return m_key; } }
        public Sprite icon { get { return m_icon; } }
        public string name { get { return m_name; } }

        public Character(string key, Sprite icon, string name, Dictionary<TYPE_FACE, Sprite> faceDic)
        {
            m_key = key;
            m_icon = icon;
            m_name = name;
            this.faceDic = faceDic;
        }

        /// <summary>
        /// 캐릭터 얼굴 가져오기
        /// </summary>
        /// <param name="typeFace"></param>
        /// <returns></returns>
        public Sprite getCharacterFace(TYPE_FACE typeFace)
        {
            if (faceDic.ContainsKey(typeFace))
            {
                //얼굴이 빈 스프라이트이면 기본형 스프라이트로 제공
                if (faceDic[typeFace] == null)
                    return faceDic[Character.TYPE_FACE.Normal];
                return faceDic[typeFace];
            }
            return null;
        }
    }
}
