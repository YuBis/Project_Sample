using Spine.Unity;
using System.Collections;
using UnityEngine;

public class Player : GameCharacter
{

    public override void Initialize(string templateKey, TeamType teamType)
    {
        base.Initialize(templateKey, teamType);

        Physics2D.IgnoreLayerCollision(GAMEOBJECT.layer, LayerMask.NameToLayer(StaticString.MONSTER_LAYER), false);
    }

    public override void OnDeath()
    {
        Physics2D.IgnoreLayerCollision(GAMEOBJECT.layer, LayerMask.NameToLayer(StaticString.MONSTER_LAYER));
    }
}
