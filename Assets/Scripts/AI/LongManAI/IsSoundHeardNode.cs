public class IsSoundHeardNode : BehaviorTreeNode
{
    private EnemyAI enemy;

    public IsSoundHeardNode(EnemyAI enemy)
    {
        this.enemy = enemy;
    }

    public override NodeState Evaluate()
    {
        if (enemy.IsSoundDetected())
        {
            enemy.ResetSoundDetection();
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
