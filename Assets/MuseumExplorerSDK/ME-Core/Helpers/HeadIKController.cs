// Based on the example found in http://docs.unity3d.com/Manual/nav-CouplingAnimationAndNavigation.html
using UnityEngine;
using Helpers;
using Helpers.Extensions;
using System;
using System.Linq;

[RequireComponent(typeof(Animator))]
public class HeadIKController : Tweener
{
    /// <summary>
    /// Enable rudimentary linear weight smoothing. For better tweening, use the tweener. Should be disabled if tweening is used.
    /// </summary>
    public bool weightSmoothing = false;
    /// <summary>
    /// Used only if weightSmoothing = true.
    /// </summary>
    public float weightSmoothingSpeed = 1;
    /// <summary>
    /// Used in Tweener functions for changing the ik weight.
    /// </summary>
    public TweenAction.TweenMode tweenMode = TweenAction.TweenMode.Smoother;

    [Range(0, 1)]
    public float bodyWeight = 0.2f;
    [Range(0, 1)]
    public float headWeight = 0.5f;
    [Range(0, 1)]
    public float eyesWeight = 0.7f;
    [Range(0, 1)]
    public float clampWeight = 0.5f;
    /// <summary>
    /// Enable, if you want the character to always keep looking at the eye level
    /// </summary>
    public bool forceEyeLevel = false;
    /// <summary>
    /// Used for auto-releasing the target when we are navigating. Increase to release earlier.
    /// </summary>
    public float steeringTargetMinDistance = 1;

    [SerializeField]
    private Transform target;

    private int tweenId = 0;
    [SerializeField][Range(0, 1)]
    private float targetWeight = 1;
    private float currentWeight;
    private Vector3 lookDefaultPos;
    private Vector3 lookAtTargetPos;
    private Vector3 lookAtPos;

    private Animator animator;
    private Animator Animator
    {
        get
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            return animator;
        }
    }

    [SerializeField]
    private Transform head;
    public Transform Head
    {
        get
        {
            if (head == null)
            {
                head = Animator.GetBoneTransform(HumanBodyBones.Head);
                if (head == null)
                {
                    Debug.LogWarning("[HeadIKController] Cannot find the head, please define the transform manually.");
                    enabled = false;
                }
                else
                {
                    lookDefaultPos = head.position + transform.forward;
                    lookAtTargetPos = lookDefaultPos;
                    lookAtPos = lookDefaultPos;
                }
            }
            return head;
        }
    }

    void Start()
    {
        Debug.Log("[HeadIKController] Init " + Head);
        var rootMotionAgent = GetComponent<RootMotionAgent>();
        if (rootMotionAgent != null)
        {
			rootMotionAgent.MovementUpdate += OnMovementUpdate;
        }
    }

	void OnDestroy() {
		var rootMotionAgent = GetComponent<RootMotionAgent>();
		if (rootMotionAgent != null)
		{
			rootMotionAgent.MovementUpdate -= OnMovementUpdate;
		}
	}

	public void OnMovementUpdate(object sender, EventArg<float, float, UnityEngine.AI.NavMeshAgent> args) {
		var agent = args.arg3;
		if (agent != null) {
			if (enabled && agent.remainingDistance > steeringTargetMinDistance) {
				lookAtTargetPos = agent.steeringTarget;
			} else if (target == null) {
				StopLooking ();
			}
		}
	}

    #region For testing
    [SerializeField]
    private bool reset;
    [SerializeField]
    private bool look;
    private bool isLooking;
    #endregion

    void Update()
    {
        #region For testing
        if (reset)
        {
            Reset();
            reset = false;
        }
        if (look && !isLooking)
        {
            LookAt(target);
        }
        if (!look && isLooking)
        {
            StopLooking(disable: false);
        }
        #endregion

        // Update the target position in case that the target moves
        if (target != null && isLooking)
        {
            lookAtTargetPos = target.position;
        }
    }

    void OnAnimatorIK()
    {
        if (forceEyeLevel)
        {
            lookAtTargetPos.y = Head.position.y;
        }
        Vector3 curDir = lookAtPos - Head.position;
        Vector3 futDir = lookAtTargetPos - Head.position;
        curDir = Vector3.RotateTowards(curDir, futDir, 6.28f * Time.deltaTime, float.PositiveInfinity);
        lookAtPos = Head.position + curDir;
        currentWeight = weightSmoothing ? Mathf.MoveTowards(currentWeight, targetWeight, Time.deltaTime * weightSmoothingSpeed) : targetWeight;
        Animator.SetLookAtPosition(lookAtPos);
        Animator.SetLookAtWeight(currentWeight, bodyWeight, headWeight, eyesWeight, clampWeight);
    }

    public void LookAt(Transform target, float weight = 1, float tweenDuration = 1, bool forceEyeLevel = false)
    {
        this.target = target;
        LookAt(target.position, weight, tweenDuration, forceEyeLevel);
    }

    public void LookAt(Vector3 target, float weight = 1, float tweenDuration = 1, bool forceEyeLevel = false)
    {
        enabled = true;
        look = true;
        isLooking = true;
        lookAtTargetPos = target;
        this.forceEyeLevel = forceEyeLevel;
        if (tweenDuration > 0)
        {
            weightSmoothing = false;
            TweenTo(tweenId, to: weight, duration: tweenDuration, mode: tweenMode, updateCallback: value => targetWeight = value);
        }
        else
        {
            weightSmoothing = true;
            targetWeight = weight;
        }
    }

    public void StopLooking(float tweenDuration = 1, bool disable = true)
    {
        if (!isLooking) { return; }
        look = false;
        isLooking = false;
        Action callback = null;
        if (disable)
        {
            callback = () => ResetAndDisable();
        }
        // If this is the first time we tween, "from" value has to be provided, because by default it's 0, which is not what we want in this case.
        if (Tweens.ContainsKey(tweenId))
        {
            TweenTo(tweenId, to: 0, duration: tweenDuration, mode: tweenMode, updateCallback: value => targetWeight = value, readyCallback: callback);
        }
        else
        {
            TweenTo(tweenId, to: 0, from: targetWeight, duration: tweenDuration, mode: tweenMode, updateCallback: value => targetWeight = value, readyCallback: callback);
        }
    }

    public void Reset()
    {
        StopAll();
        target = null;
        lookAtPos = lookDefaultPos;
        isLooking = false;
        look = false;
    }

    public void ResetAndDisable()
    {
        Reset();
        enabled = false;
    }
}
