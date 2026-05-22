using UnityEngine;

public enum SkillUpgradeType
{
    None,//角色挂载了所有技能的脚本，若技能未解锁，则upgradetType是none类型，表示未解锁


    Dash,
    Dash_CloneOnStart,
    Dash_CloneOnStartAndArrival,
    Dash_ShardOnStart,
    Dash_ShardOnStartAndArrival,

    Shard,
    Shard_MoveToEnemy,
    Shard_MultiCast,
    Shard_Teleport,//传送
    Shard_TeleportAndHpRewind
}
