public abstract class BehaviorTreeNode
{
    public enum NodeState { Running, Success, Failure }
    public abstract NodeState Evaluate();
}
