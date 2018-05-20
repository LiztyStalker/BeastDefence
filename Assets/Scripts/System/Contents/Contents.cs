using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Contents
{
    public enum TYPE_CONTENTS_EVENT{Start, End}
    public enum TYPE_CONTENTS_POS { Left, Right }

    string m_key;
    string m_stageKey;
    string m_character;
    string m_typeFace;
    TYPE_CONTENTS_EVENT m_typeContentsEvent;
    TYPE_CONTENTS_POS m_typeContentsPos;

    string m_contents;

    public string key { get { return m_key; } }
    public TYPE_CONTENTS_EVENT typeContentsEvent { get { return m_typeContentsEvent; } }
    public TYPE_CONTENTS_POS typeContentsPos { get { return m_typeContentsPos; } }
    public string stageKey { get { return m_stageKey; } }
    public string character { get { return m_character; } }
    public string typeFace { get { return m_typeFace; } }
    public string contents { get { return m_contents; } }


    public Contents(string key,
        string stageKey,
        string character,
        string typeFace,
        string contents,
        TYPE_CONTENTS_EVENT typeEvent,
        TYPE_CONTENTS_POS typePos

        )
    {
        m_key = key;
        m_stageKey = stageKey;
        m_character = character;
        m_typeFace = typeFace;
        m_contents = contents;
        m_typeContentsEvent = typeEvent;
        m_typeContentsPos = typePos;
    }
}

