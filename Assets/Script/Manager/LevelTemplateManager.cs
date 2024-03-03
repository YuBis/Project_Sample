using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;

public class LevelTemplateManager : BaseGameDataManager<LevelTemplateManager, LevelTemplateData>
{
    public override void InitData()
    {
        LoadData(StaticString.LEVEL_TABLE);
    }
}