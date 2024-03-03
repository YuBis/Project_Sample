using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class InitState : BaseState
{
    private void Awake()
    {
        STATE_TYPE = StateType.INIT;
        SCENE_NAME = string.Empty;
    }

    public override void BeginState()
    {
        base.BeginState();

        DataLoader dataManager;

        dataManager = StringTableManager.Instance;
        dataManager = SkillTemplateManager.Instance;
        dataManager = LevelTemplateManager.Instance;
        dataManager = CharacterTemplateManager.Instance;
        dataManager = StatSheetManager.Instance;

        dataManager = AIFactory.Instance;
        dataManager = ObjectPoolManager.Instance;
        dataManager = CombatManager.Instance;
        dataManager = KeyMappingManager.Instance;
        dataManager = StatManager.Instance;
        dataManager = UIManager.Instance;
        dataManager = StringTableManager.Instance;
        dataManager = CharacterManager.Instance;
        dataManager = BoardManager.Instance;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (DataLoader.LOADING_DATA == 0)
            StateManager.Instance.ChangeState(StateType.LOBBY);
    }

    public override void EndState()
    {
        base.EndState();
    }
}
