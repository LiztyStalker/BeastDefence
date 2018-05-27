using UnityEngine;
using UnityEngine.UI;
using Defence.CharacterPackage;

public class UIContentsView : MonoBehaviour
{
    [SerializeField]
    Text m_contentsText;

    [SerializeField]
    Image m_image;

    [SerializeField]
    Text m_nameText;

    public void setContents(Contents contents)
    {

        Character character = CharacterManager.GetInstance.getCharacter(contents.character);
        if (character != null)
        {
            m_image.sprite = character.getCharacterFace(contents.typeFace);
            m_nameText.text = character.name;
        }
        m_contentsText.text = contents.contents;
    }
}

