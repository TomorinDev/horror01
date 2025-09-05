using System.Collections.Generic;
public class SelectorNode : BehaviorTreeNode
{
    private List<BehaviorTreeNode> childNodes = new List<BehaviorTreeNode>();

    public SelectorNode(List<BehaviorTreeNode> nodes) => childNodes = nodes;

    public override NodeState Evaluate()
    {
        foreach (var node in childNodes)
        {
            var state = node.Evaluate();
            if (state == NodeState.Success)
                return NodeState.Success;
            if (state == NodeState.Running)
                return NodeState.Running;
        }
        return NodeState.Failure;
    }
}
