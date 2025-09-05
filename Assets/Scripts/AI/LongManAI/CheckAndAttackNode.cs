public class CheckAndAttackNode : BehaviorTreeNode
{
    private EnemyAI enemy;

    public CheckAndAttackNode(EnemyAI enemy) { this.enemy = enemy; }

    public override NodeState Evaluate()
    {
        enemy.CheckAndAttackTarget(enemy.player.gameObject);
        return NodeState.Success;
    }
}