public class IsPlayerDetectedNode : BehaviorTreeNode
{
    private EnemyAI _enemy;

    public IsPlayerDetectedNode(EnemyAI enemy)
    {
        _enemy = enemy;
    }

    public override NodeState Evaluate()
    {
        return _enemy.IsPlayerInLineOfSight() ? NodeState.Success : NodeState.Failure;
    }
}

