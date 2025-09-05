using System.Collections.Generic;

public class SequenceNode : BehaviorTreeNode
{
    private List<BehaviorTreeNode> childNodes = new List<BehaviorTreeNode>();

    public SequenceNode(List<BehaviorTreeNode> nodes) => childNodes = nodes;

    public override NodeState Evaluate()
    {
        bool anyChildRunning = false;
        foreach (var node in childNodes)
        {
            var state = node.Evaluate();
            if (state == NodeState.Failure)
                return NodeState.Failure;
            if (state == NodeState.Running)
                anyChildRunning = true;
        }
        return anyChildRunning ? NodeState.Running : NodeState.Success;
    }
}