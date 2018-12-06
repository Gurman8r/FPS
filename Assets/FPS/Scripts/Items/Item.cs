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
        [SerializeField] UnityEvent m_onDrop;
        [SerializeField] UnityEvent m_onEquip;
        [SerializeField] UnityEvent m_onStore;

        [Header("Item Runtime")]
        [SerializeField] Unit       m_owner;

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

        public Unit owner
        {
            get { return m_owner; }
            set { m_owner = value; }
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

        public bool SetOwner(Unit value)
        {
            if (!owner && value)
            {
                owner = value;
                return true;
            }
            else if (owner && (owner == value))
            {
                return true;
            }
            else if (owner && !value)
            {
                owner = null;
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
    }
}
