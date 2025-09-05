public class StunNode : BehaviorTreeNode
{
    private EnemyAI enemyAI;

    public StunNode(EnemyAI enemy)
    {
        enemyAI = enemy;
    }

    public override NodeState Evaluate()
    {
        if (enemyAI.Stun()) 
        {
            return NodeState.Success; 
        }
        return NodeState.Failure; 
    }
}