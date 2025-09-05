using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;
using UnityEngine.Rendering;

public class MonsterBehaviorTree : MonoBehaviour
{
    public DarknessDetection darknessDetection;
    public CharStatusManager charStatusManager;
    // public RandomEyesEffect swirlingEyesEffect;
    public EyeEffectController eyeEffectController;
    public HorrorWallEffectController horrorWallEffectController; // Horror Wall Effect
    public DarknessPPEController darknessPPEController; // Darkness Post Processing Effect

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip audioClip; // Crescendo audio clip


    // Devices
    public List<GameObject> devices;


    public float actionInterval = 10f; // Time between proactive actions
    private float timer;

    private SelectorNode rootNode;

    void Start()
    {
        // Initialize behavior tree
        rootNode = new SelectorNode(new List<BehaviorTreeNode>
        {
            new DarknessAttackNode(darknessDetection, charStatusManager, audioSource, audioClip, this, eyeEffectController, horrorWallEffectController, darknessPPEController),
            // new ProactiveActionNode(devices, actionInterval)
        });

        timer = actionInterval;
    }

    void Update()
    {
        rootNode.Evaluate();
    }
}
