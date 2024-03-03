using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;

public class CharacterTemplateManager : BaseGameDataManager<CharacterTemplateManager, CharacterTemplateData>
{
    public override void InitData()
    {
        LoadData(StaticString.CHARACTER_TABLE);
    }
}