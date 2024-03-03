using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;

public enum StatType
{
    TARGET_COUNT,
    HIT_COUNT,
    DAMAGE_PER,

    HP,
    MP,
    DAMAGE,
    MOVE_SPEED,
    JUMP_RANGE,

    EXP,
    SIGHT,

    HP_COST,
    MP_COST,

    KNOCK_BACK,

    Count,
}


public class StatSheetManager : BaseGameDataManager<StatSheetManager, StatSheetData>
{
    public override void InitData()
    {
        LoadData(StaticString.STAT_SHEET);
    }
}