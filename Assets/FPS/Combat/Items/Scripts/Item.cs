using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [ExecuteInEditMode]
    public abstract class Item : MonoBehaviour
    {
        public const string Tag = "Item";

        public enum UseMode
        {
            Single = 0,
            Continuous = 1,
        }

        /* Variables
        * * * * * * * * * * * * * * * */
        private Animator    m_animator;
        private AudioSource m_audio;
        private Collider    m_collider;
        private Rigidbody   m_rigidbody;

        [Header("Base Item Settings")]
        [SerializeField] UniqueID   m_UID;
        [SerializeField] bool       m_interactable  = true;
        [SerializeField] UseMode    m_useMode       = UseMode.Single;
        [SerializeField] float      m_useDelay      = 1f;
        [SerializeField] bool       m_staticReticle;
        [SerializeField] int        m_maxResource   = -1;
        [SerializeField] bool       m_isRenewable   = false;
        [SerializeField] float      m_cooldownDelay = 2.5f;
        [SerializeField] bool       m_fixedCooldown = true;
        [SerializeField] bool       m_autoCooldown  = true;
        [Space]
        [SerializeField] Transform  m_model;
        [SerializeField] Transform  m_holdRoot;
        [SerializeField] UnityEvent m_onDrop;
        [SerializeField] UnityEvent m_onEquip;
        [SerializeField] UnityEvent m_onStore;


        [Header("Base Item Runtime")]        
        [SerializeField] Unit       m_owner;
        [SerializeField] Transform  m_originalRoot;
        [SerializeField] float      m_cooldownTimer;
        [SerializeField] bool       m_onCooldown;
        [SerializeField] bool       m_canUse;
        [SerializeField] float      m_useTimer;
        [SerializeField] int        m_curResource;



        /* Properties
        * * * * * * * * * * * * * * * */
        public UniqueID UID
        {
            get { return m_UID; }
        }

        public Animator animator
        {
            get
            {
                if(!m_animator)
                {
                    m_animator = GetComponent<Animator>();
                }
                return m_animator;
            }
        }

        public new AudioSource audio
        {
            get
            {
                if(!m_audio)
                {
                    m_audio = GetComponent<AudioSource>();
                }
                return m_audio;
            }
        }

        public new Collider collider
        {
            get
            {
                if(!m_collider)
                {
                    m_collider = GetComponent<Collider>();
                }
                return m_collider;
            }
        }

        public new Rigidbody rigidbody
        {
            get
            {
                if(!m_rigidbody)
                {
                    m_rigidbody = GetComponent<Rigidbody>();
                }
                return m_rigidbody;
            }
        }

        public bool interactable
        {
            get { return m_interactable; }
            protected set { m_interactable = value; }
        }

        public UseMode useMode
        {
            get { return m_useMode; }
            set { m_useMode = value; }
        }

        public float useDelay
        {
            get { return m_useDelay; }
            set { m_useDelay = value; }
        }

        public bool staticReticle
        {
            get { return m_staticReticle; }
        }

        public bool isRenewable
        {
            get { return m_isRenewable; }
        }

        public bool autoReload
        {
            get { return m_autoCooldown; }
        }

        public bool fixedReload
        {
            get { return m_fixedCooldown; }
        }

        public float cooldownDelay
        {
            get { return m_cooldownDelay; }
        }
        
        public bool onCooldown
        {
            get { return m_onCooldown; }
            protected set { m_onCooldown = value; }
        }

        public float cooldownTimer
        {
            get { return m_cooldownTimer; }
            protected set { m_cooldownTimer = value; }
        }

        public float useTimer
        {
            get { return m_useTimer; }
            protected set { m_useTimer = value; }
        }

        public bool canUse
        {
            get
            {
                return m_canUse =
                    (!onCooldown) &&
                    (useTimer >= useDelay) &&
                    (resourceAvailable);
            }
        }

        public float useDelta
        {
            get
            {
                return (onCooldown)
                    ? 0f
                    : (staticReticle)
                        ? (1f)
                        : (useDelay > 0f)
                            ? (!canUse)
                                ? (useTimer / useDelay)
                                : (1f)
                            : (1f);
            }
        }
        
        public int maxResource
        {
            get { return m_maxResource; }
        }

        public int curResource
        {
            get { return m_curResource; }
            private set { m_curResource = value; }
        }

        public bool resourceAvailable
        {
            get
            {
                return (maxResource > 0) ? (curResource > 0) : (true);
            }
        }

        public float resourceDelta
        {
            get
            {
                return (maxResource > 0)
                    ? ((float)(curResource) / (float)(maxResource))
                    : (1f);
            }
        }

        public float reloadDelta
        {
            get
            {
                return (cooldownDelay > 0f)
                    ? (cooldownTimer / cooldownDelay)
                    : (1f);
            }
        }

        public UnityEvent onDrop
        {
            get { return m_onDrop; }
        }

        public UnityEvent onEquip
        {
            get { return m_onEquip; }
        }

        public UnityEvent onStore
        {
            get { return m_onStore; }
        }

        public Unit owner
        {
            get { return m_owner; }
            set { m_owner = value; }
        }

        public Transform holdRoot
        {
            get { return m_holdRoot; }
            set { m_holdRoot = value; }
        }

        public Transform model
        {
            get { return m_model; }
            set { m_model = value; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected virtual void Awake()
        {
            if(Application.isPlaying)
            {
                if (!owner)
                {
                    SetRoot(m_originalRoot = transform.parent, true);
                    EnablePhysics(true);
                    EnableAnimator(false);
                }
            }
        }

        protected virtual void Start()
        {
            if(Application.isPlaying)
            {
                useTimer = useDelay;

                SetResource(maxResource);

                onDrop.AddListener(() => { CancelCooldown(); });
                onEquip.AddListener(() => { CancelCooldown(); });
                onStore.AddListener(() => { CancelCooldown(); });
            }
        }

        protected virtual void Update()
        {
            if(Application.isPlaying)
            {
                if (owner)
                {
                    transform.localPosition = holdRoot.localPosition;
                }

                if (isRenewable)
                {
                    if (autoReload && !resourceAvailable)
                    {
                        StartCooldown();
                    }

                    animator.SetBool("Reloading", onCooldown);
                }
            }
        }

        protected virtual void OnDrawGizmos()
        {
            if(holdRoot)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position + holdRoot.localPosition, 0.1f);
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public void SetRoot(Transform parentTransform, bool worldPositionStays)
        {
            transform.SetParent(
                parentTransform
                    ? parentTransform
                    : m_originalRoot,
                worldPositionStays);

            if (parentTransform && !worldPositionStays)
            {
                transform.localRotation = Quaternion.identity;
                transform.localPosition = Vector3.zero;
            }
        }

        public bool SetOwner(Unit owner)
        {
            if (!this.owner && owner) // pickup
            {
                this.owner = owner;
                return true;
            }
            else if (this.owner && (this.owner == owner)) // owner is same
            {
                return true;
            }
            else if (this.owner && !owner) // drop
            {
                this.owner = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void OnDrop(Transform value)
        {
            SetRoot(value, true);
            EnablePhysics(true);
            EnableAnimator(false);
        }

        public void OnEquip(Transform value)
        {
            SetRoot(value, false);
            EnablePhysics(false);
            EnableAnimator(true);
            EnableGameObject(true);
        }

        public void OnStore(Transform value)
        {
            SetRoot(value, false);
            EnablePhysics(false);
            EnableAnimator(false);
            EnableGameObject(false);
        }

        public void EnableAnimator(bool value)
        {
            animator.enabled = value;
        }

        public void EnableGameObject(bool value)
        {
            gameObject.SetActive(value);
        }

        public void EnablePhysics(bool value)
        {
            rigidbody.isKinematic = !value;
            rigidbody.useGravity = value;
            collider.enabled = value;
        }

        public abstract void HandleInput(ButtonState lhs, ButtonState rhs);


        protected void SetResource(int value)
        {
            curResource = Mathf.Clamp(value, 0, maxResource);
        }

        protected void ConsumeResource()
        {
            SetResource(curResource - 1);
        }

        public void StartCooldown()
        {
            if (isRenewable && !onCooldown && (curResource != maxResource))
            {
                StartCoroutine(CooldownCoroutine());
            }
        }

        public void CancelCooldown()
        {
            StopAllCoroutines();
            onCooldown = false;
        }

        private IEnumerator CooldownCoroutine()
        {
            onCooldown = true;

            for (cooldownTimer = fixedReload
                    ? 0f
                    : (cooldownDelay * ((float)curResource / (float)maxResource));
                cooldownTimer < cooldownDelay;
                cooldownTimer += Time.deltaTime)
            {
                if (!fixedReload)
                {
                    SetResource((int)(maxResource * reloadDelta));
                }

                yield return null;
            }

            SetResource(maxResource);

            onCooldown = false;
        }
    }
}
