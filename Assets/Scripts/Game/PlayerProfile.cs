using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerProfile
{
    public static PlayerProfile Instance
    {
        get
        {
            if (_instance == null)
            {
                if (PlayerPrefs.HasKey("playerData"))
                {
                    var xmlSerializer = new XmlSerializer(typeof(PlayerProfile));
                    using (var reader = new StringReader(PlayerPrefs.GetString("playerData")))
                    {
                        _instance = (PlayerProfile)xmlSerializer.Deserialize(reader);
                    }
                }
                else
                {
                    _instance = new PlayerProfile();
                }
            }

            return _instance;
        }
    }

    private static PlayerProfile _instance;

    public void Flush()
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(PlayerProfile));
        using (StringWriter writer = new StringWriter())
        {
            xmlSerializer.Serialize(writer, this);
            PlayerPrefs.SetString("playerData", writer.ToString());
        }
    }

    public int currentLevel = 0;
    public int levelsPassed = 0;
}
