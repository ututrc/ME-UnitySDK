using UnityEngine;
using System;
/// <summary>
/// This script handles movement so that the navigation agent is used to determine the target position, but the actual movement is handled via animation root motion.
/// Rotation and movement are calculated, but not applied, unless rotateCharacterTowardAgent and pullCharacterTowardAgent are set true.
/// By default, the movement control is given to the animator, but the rotation control is not.
/// The calculations are sent in MovementUpdate event, which clients should use for example for sending the data to the animator.
/// </summary>
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class RootMotionAgent : MonoBehaviour
{
    public Animator animator;
    // This is the cap for the agent speed. If the maxSpeed is greater than the agent speed, this is not applied.
    // Both agent speed and angular speed are used to determine the actual movementspeed.
    public float maxSpeed = 1;
    // If this is set false, the character is rotated only by the root motion.
    // Animator angular speed is not used if set to false.
    public bool rotateCharacterTowardAgent = true;
    // Pulls the character towards agent position. Root motion is still used, but this adds to the total translation.
    public bool pullCharacterTowardAgent;
    // How fast the character is pulled toward the agent? Can be used as movement speed, when root motion is not applied
    public float pullSpeed = 0.5f;
    // Enable to debug the movement with mouse (point and click)
    public bool enableDebugMouseMovement;
    // Margin to compare the movement vector magnitude. Greater margin -> more distance is left between the agent and the character when stopping.
    // If the margin is too low, the character may not stop rotating/moving having reached the target.
    public float followMargin = 0.1f;

    // Disable if the agent is following waypoints and if the next is not the last waypoint.
    public bool autoBreaking = true;

    /// <summary>
    /// The first argument is turn speed (-1 to 1), the second is forward speed (0 to 1), and the last is the agent
    /// </summary>
    public EventHandler<EventArg<float, float, UnityEngine.AI.NavMeshAgent>> MovementUpdate = (sender, args) => { };

    private UnityEngine.AI.NavMeshAgent _agent;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (animator == null)
        {
            Debug.LogWarningFormat("[RootMotionAgent] ({0}): No animator found.");
        }
        else
        {
            MovementUpdate += (sender, args) =>
            {
                animator.SetFloat("Turn", args.arg1);
                animator.SetFloat("Forward", args.arg2);
            };
        }
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _agent.updatePosition = false;
        _agent.updateRotation = false;
    }

    void Update()
    {
        if (!(ApplicationManager.Instance.CurrentPlayState == AR.PlayState.Playing)) { return; }
        if (_agent.speed > maxSpeed) { _agent.speed = maxSpeed; }
        if (enableDebugMouseMovement && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                _agent.destination = hit.point;
            }
        }
        Vector3 movementVector = _agent.nextPosition - transform.position;
        Debug.DrawRay(transform.position, movementVector, Color.red);
        bool shouldMove = movementVector.magnitude > followMargin;
        float turn = 0;
        float forward = 0;
        if (shouldMove)
        {
            // TODO: Turning does not work as it should, although this code should work. The issue can be in the blend tree of the animator component, where the blends are not defined properly.
            //Vector3 localVector = transform.InverseTransformDirection(movementVector);
            //turn = Mathf.Atan2(localVector.x, localVector.z) / 3;
            //turn = Mathf.Clamp(turn, -1, 1);
            forward = autoBreaking ? Mathf.Clamp01(movementVector.magnitude) : 1;
        }
        if (rotateCharacterTowardAgent && movementVector != Vector3.zero)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movementVector), _agent.angularSpeed * Time.deltaTime);
        }
        if (pullCharacterTowardAgent)
        {
            transform.position = Vector3.MoveTowards(transform.position, _agent.nextPosition, pullSpeed * Time.deltaTime);
        }
        MovementUpdate(this, new EventArg<float, float, UnityEngine.AI.NavMeshAgent>(turn, forward, _agent));
    }
}
