public class RoamNode : BehaviorTreeNode
{
    private EnemyAI _enemy;

    public RoamNode(EnemyAI enemy)
    {
        _enemy = enemy;
    }

    public override NodeState Evaluate()
    {
        _enemy.Roam();
        return NodeState.Running;
    }
}

