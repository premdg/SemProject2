using System.Collections;
using System.Collections.Generic;
using System.Net.Cache;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace TurryWoods
{
    public class PlayerController : MonoBehaviour, IAttackAnimListener, IMessageReceiver
    {
        public static PlayerController Instance {get ; private set;}
       
        public bool IsRespawning 
        {
            get { return m_IsRespawning;}
        }
        public MeleeWeapon meleeWeapon;
        public float maxForwardSpeed = 8.0f;
        public float rotationSpeed;
        public float m_MaxRotationSpeed;
        public float m_MinRotationSpeed;
        public float gravity;
        public Transform attackHand;

        private static PlayerController s_Instance;
        private PlayerInput m_PlayerInput;
        private CharacterController m_ChController;
        private Animator m_Animator;
        private CameraController m_CameraController;
        private HudManager m_HUDManager;
        private Damagable m_Damageable;
        private Quaternion m_TargetRotation;

        private AnimatorStateInfo m_CurrentStateInfo;
        private AnimatorStateInfo m_NextStateInfo;
        private bool m_IsAnimTransitioning;
        private bool m_IsRespawning;
        private float m_DesiredForwardSpeed;
        private float m_ForwardSpeed;
        private float m_VerticalSpeed;
        const float k_Acceleration = 30.0f;
        const float k_Deceleration = 75.0f;

        private readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        private readonly int m_HashMelleAttack = Animator.StringToHash("MeleeAttack");
        private readonly int m_HashDeath = Animator.StringToHash("Death");
        private readonly int m_HashBlockInput = Animator.StringToHash("BlockInput");

        private void Awake()
        {
            m_ChController = GetComponent<CharacterController>();
            m_PlayerInput = GetComponent<PlayerInput>();
            m_Animator = GetComponent<Animator>();
            m_Damageable = GetComponent<Damagable>();
            m_CameraController = Camera.main.GetComponent<CameraController>();
            m_HUDManager = FindObjectOfType<HudManager>();
            s_Instance = this;

            m_HUDManager.SetMaxHealth(m_Damageable.maxHitPoints);

            Instance = this;

            //Debug.Log("Awake");
        }


        private void FixedUpdate()
        {
            UpdateInputBlocking();
            CacheAnimationState();
            ComputeForwardMovement();
            ComputeVerticalMovement();
            ComputeRotation();

            if (m_PlayerInput.IsMoveInput)
            {float rotationSpeed = Mathf.Lerp(m_MaxRotationSpeed, m_MinRotationSpeed, m_ForwardSpeed / m_DesiredForwardSpeed);
                m_TargetRotation = Quaternion.RotateTowards(
                    transform.rotation,
                    m_TargetRotation,
                    400 * Time.fixedDeltaTime);
                transform.rotation = m_TargetRotation;
            }
            m_Animator.ResetTrigger(m_HashMelleAttack);
            if (m_PlayerInput.IsAttacking)
            {
                m_Animator.SetTrigger(m_HashMelleAttack);

            }
        }


        private void OnAnimatorMove()
        {
            if(m_IsRespawning) { return; }

            m_ChController.Move(m_Animator.deltaPosition);
            Vector3 movement = m_Animator.deltaPosition;
            movement += m_VerticalSpeed * Vector3.up * Time.fixedDeltaTime;
            m_ChController.Move(movement);
        }
        public void OnRecieveMessage(IMessageReceiver.MessageType type, object sender, object message)
        {
            if (type == IMessageReceiver.MessageType.DAMAGED)
            {
                m_HUDManager.SetHealth((sender as Damagable).currentHitPoints); 
                Debug.Log("Receiving Damage");
            }
            if (type == IMessageReceiver.MessageType.DEAD)
            {
                m_IsRespawning = true;
                m_Animator.SetTrigger(m_HashDeath);
                m_HUDManager.SetHealth(0);
                SceneManager.LoadScene("Testing");
            }
        }

        public void MeleeAttackStart()
        {
            if(meleeWeapon !=null)
            {
                meleeWeapon.BeginAttack();
            }
            
        }
        public void StartRespawn()
        {
            transform.position = Vector3.zero;
            m_HUDManager.SetHealth(m_Damageable.maxHitPoints);
            m_Damageable.SetInitialHealth();
        }
        public void Finishrespawn()
        {
            m_IsRespawning = false;
        }
        public void MeleeAttackEnd()
        {
            if(meleeWeapon !=null)
            {
                meleeWeapon.EndAttack();
            }
        }
        public void UseItemFrom(InventorySlot slot)
        {
            if (meleeWeapon != null)
            {
                if (slot.itemPrefab.name == meleeWeapon.name) { return; }
                else
                {
                    Destroy(meleeWeapon.gameObject);
                }
            }

            meleeWeapon = Instantiate(slot.itemPrefab, transform)
                .GetComponent<MeleeWeapon>();
            meleeWeapon.GetComponent<FixedUpdateFollow>().SetFolowee(attackHand);
            meleeWeapon.name = slot.itemPrefab.name;
            meleeWeapon.SetOwner(gameObject);
        }
        private void ComputeVerticalMovement()
        {
            m_VerticalSpeed = -gravity;
        }

        private void ComputeForwardMovement()
        {
            Vector3 moveInput = m_PlayerInput.MoveInput.normalized;
            m_DesiredForwardSpeed = moveInput.magnitude * maxForwardSpeed;

            float acceleration = m_PlayerInput.IsMoveInput ? k_Acceleration : k_Deceleration;

            m_ForwardSpeed = Mathf.MoveTowards(m_ForwardSpeed, m_DesiredForwardSpeed, Time.fixedDeltaTime * acceleration);
            m_Animator.SetFloat(m_HashForwardSpeed, m_ForwardSpeed);
        }

        private void ComputeRotation()
        {
            Vector3 moveInput = m_PlayerInput.MoveInput.normalized;
            Vector3 cameraDirection = Quaternion.Euler(0, m_CameraController.Playercam.m_XAxis.Value, 0) * Vector3.forward;
            Quaternion targetRotation;
            if (Mathf.Approximately(Vector3.Dot(moveInput, Vector3.forward), -1.0f))
            {
                targetRotation = Quaternion.LookRotation(-cameraDirection);
            }
            else
            {
                Quaternion movementRotation = Quaternion.FromToRotation(Vector3.forward, moveInput);
                targetRotation = Quaternion.LookRotation(movementRotation * cameraDirection);
            }

            m_TargetRotation = targetRotation;

        }
        private void CacheAnimationState()
        {
            m_CurrentStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
            m_NextStateInfo = m_Animator.GetNextAnimatorStateInfo(0);

            m_IsAnimTransitioning = m_Animator.IsInTransition(0);
        }

        private void UpdateInputBlocking()
        {
            bool InputBlocked = m_CurrentStateInfo.tagHash == m_HashBlockInput && !m_IsAnimTransitioning;
            InputBlocked |= m_NextStateInfo.tagHash == m_HashBlockInput;  
            Debug.Log(InputBlocked);
            m_PlayerInput.playerControllerInputBlocked = InputBlocked;
        }
    }
}