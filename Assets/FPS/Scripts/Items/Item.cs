using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    [ExecuteInEditMode]
    public abstract class Item : MonoBehaviour
    {
        public const float FireDelayThreshold = 0.09f;

        public enum UseMode
        {
            Single = 0,
            Continuous = 1,
        }

        public const string Tag = "Item";

        /* Variables
        * * * * * * * * * * * * * * * */
        private Collider    m_collider;
        private Rigidbody   m_rigidbody;
        private Animator    m_animator;
        private AudioSource m_audioSource;

        [Header("Item Settings")]
        [SerializeField] Transform  m_model;
        [SerializeField] Transform  m_holdPos;
        [SerializeField] ItemInfo   m_info;        
        [Space]
        [SerializeField] bool       m_interactable  = true;
        [SerializeField] UseMode    m_useMode       = UseMode.Single;
        [SerializeField] float      m_useDelay      = 1f;
        [SerializeField] bool       m_staticReticle;
        [SerializeField] int        m_maxResource   = 0;        
        [SerializeField] float      m_reloadDelay   = 2.5f;
        [SerializeField] bool       m_fixedReload   = true;
        [SerializeField] bool       m_autoReload    = true;
        [Space]
        [SerializeField] UnityEvent m_onDrop;
        [SerializeField] UnityEvent m_onEquip;
        [SerializeField] UnityEvent m_onStore;

        [Header("Item Runtime")]
        [SerializeField] UnitInventory.Hand m_hand;
        [SerializeField] Unit       m_owner;
        [SerializeField] float      m_useTimer;
        [SerializeField] bool       m_canUse;
        [SerializeField] bool       m_onCooldown;
        [SerializeField] int        m_curResource;
        [SerializeField] float      m_reloadTimer;

        /* Properties
        * * * * * * * * * * * * * * * */
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
                if(!m_audioSource)
                {
                    m_audioSource = GetComponent<AudioSource>();
                }
                return m_audioSource;
            }
        }


        public Transform model
        {
            get { return m_model; }
            set { m_model = value; }
        }

        public Transform holdPos
        {
            get { return m_holdPos; }
            set { m_holdPos = value; }
        }

        public ItemInfo info
        {
            get { return m_info; }
            set { m_info = value; }
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

        public int maxResource
        {
            get { return m_maxResource; }
        }

        public bool autoReload
        {
            get { return m_autoReload; }
        }

        public bool fixedReload
        {
            get { return m_fixedReload; }
        }
        

        public UnitInventory.Hand hand
        {
            get { return m_hand; }
            private set { m_hand = value; }
        }

        public Unit owner
        {
            get { return m_owner; }
            set { m_owner = value; }
        }

        public float reloadDelay
        {
            get { return m_reloadDelay; }
        }
        
        public bool onCooldown
        {
            get { return m_onCooldown; }
            protected set { m_onCooldown = value; }
        }

        public float cooldownTimer
        {
            get { return m_reloadTimer; }
            protected set { m_reloadTimer = value; }
        }

        public float useTimer
        {
            get { return m_useTimer; }
            protected set { m_useTimer = value; }
        }

        public int curResource
        {
            get { return m_curResource; }
            private set { m_curResource = value; }
        }

        public bool canUse
        {
            get
            {
                return m_canUse =
                    (!onCooldown) &&
                    (useTimer >= useDelay) &&
                    (hasResource);
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

        public bool hasResource
        {
            get
            {
                return (maxResource > 0)
                    ? (curResource > 0)
                    : (true);
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
                return (reloadDelay > 0f)
                    ? (cooldownTimer / reloadDelay)
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


        /* Core
        * * * * * * * * * * * * * * * */
        private void Awake()
        {
            if(Application.isPlaying)
            {
                if (!owner)
                {
                    Reparent(null, true);
                    EnablePhysics(true);
                    EnableAnimator(false);
                }
            }
        }

        protected virtual void Start()
        {
            if(Application.isPlaying)
            {
                if (gameObject.tag != Tag)
                {
                    gameObject.tag = Tag;
                }

                useTimer = useDelay;

                SetResource(maxResource);

                onDrop.AddListener(() => { CancelReload(); });
                onEquip.AddListener(() => { CancelReload(); });
                onStore.AddListener(() => { CancelReload(); });
            }
        }

        protected virtual void Update()
        {
            if(Application.isPlaying)
            {
            }
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + holdPos.localPosition, 0.1f);
        }


        /* Functions
        * * * * * * * * * * * * * * * */
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

        public void Reparent(Transform parentTransform, bool worldPositionStays)
        {
            transform.SetParent(parentTransform, worldPositionStays);

            if (parentTransform && !worldPositionStays)
            {
                transform.localRotation = Quaternion.identity;
                transform.localPosition = Vector3.zero;
            }
        }

        public bool SetOwner(Unit owner, UnitInventory.Hand hand)
        {
            if (!this.owner && owner) // pickup
            {
                this.hand = hand;
                this.owner = owner;
                return true;
            }
            else if (this.owner && (this.owner == owner)) // owner is same
            {
                return true; 
            }
            else if (this.owner && !owner) // drop
            {
                this.hand = null;
                this.owner = null;
                return true;
            }
            else
            {
                return false;
            }
        }


        public abstract void UpdatePrimary(InputState input);

        public abstract void UpdateSecondary(InputState input);


        public virtual void OnDrop()
        {
            m_onDrop.Invoke();
        }

        public virtual void OnEquip()
        {
            m_onEquip.Invoke();
        }

        public virtual void OnStore()
        {
            m_onStore.Invoke();
        }


        protected void SetResource(int value)
        {
            curResource = Mathf.Clamp(value, 0, maxResource);
        }

        protected void ConsumeResource()
        {
            SetResource(curResource - 1);
        }

        public void Reload()
        {
            if (!onCooldown && (curResource != maxResource))
            {
                StartCoroutine(ReloadCoroutine());
            }
        }

        public void CancelReload()
        {
            StopAllCoroutines();
            onCooldown = false;
        }

        private IEnumerator ReloadCoroutine()
        {
            onCooldown = true;

            for (cooldownTimer = fixedReload
                    ? 0f
                    : (reloadDelay * ((float)curResource / (float)maxResource));
                cooldownTimer < reloadDelay;
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
