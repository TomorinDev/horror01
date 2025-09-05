using System.Collections.Generic;

public class LMBehaviourTree : BehaviorTreeNode
{
    private SelectorNode rootNode;

    public LMBehaviourTree(EnemyAI enemy)
    {
        var isPlayerDetectedNode = new IsPlayerDetectedNode(enemy);
        var isSoundHeardNode = new IsSoundHeardNode(enemy);
        var chaseTargetNode = new ChaseTargetNode(enemy);
        var isWithinAttackRangeNode = new IsWithinAttackRangeNode(enemy);
        var checkAndAttackNode = new CheckAndAttackNode(enemy);
        var stunNode = new StunNode(enemy);

        var roamSequence = new SequenceNode(new List<BehaviorTreeNode>
        {
            new RoamNode(enemy)
        });
        var chaseSequence = new SequenceNode(new List<BehaviorTreeNode>
        {
            new SelectorNode(new List<BehaviorTreeNode>
            {
                isPlayerDetectedNode,
                isSoundHeardNode
            }),
            chaseTargetNode
        });

        var attackSequence = new SequenceNode(new List<BehaviorTreeNode>
        {
            isWithinAttackRangeNode,
            checkAndAttackNode
        });

        var stunSequence = new SequenceNode(new List<BehaviorTreeNode>
        {
            stunNode
        });

        rootNode = new SelectorNode(new List<BehaviorTreeNode>
        {
            stunSequence,
            attackSequence,
            chaseSequence,
            roamSequence
        });
    }

    public override NodeState Evaluate() => rootNode.Evaluate();
}
