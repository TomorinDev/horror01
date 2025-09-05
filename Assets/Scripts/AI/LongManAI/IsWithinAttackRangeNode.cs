public class IsWithinAttackRangeNode : BehaviorTreeNode
{
    private EnemyAI enemy;

    public IsWithinAttackRangeNode(EnemyAI enemy) { this.enemy = enemy; }

    public override NodeState Evaluate()
    {
        return enemy.IsWithinAttackRange(enemy.player) ? NodeState.Success : NodeState.Failure;
    }
}