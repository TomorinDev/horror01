public class ChaseTargetNode : BehaviorTreeNode
{
    private EnemyAI _enemy;

    public ChaseTargetNode(EnemyAI enemy)
    {
        _enemy = enemy;
    }

    public override NodeState Evaluate()
    {
        _enemy.ChaseTarget();
        return NodeState.Running;
    }
}