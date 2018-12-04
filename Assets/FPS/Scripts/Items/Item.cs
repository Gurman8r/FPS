using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    [ExecuteInEditMode]
    public abstract class Item : MonoBehaviour
    {
        public static readonly string Tag = "Item";

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

        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + holdPos.localPosition, 0.1f);
        }


        /* Functions
        * * * * * * * * * * * * * * * */
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
            else if(owner && !value)
            {
                owner = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void EnableAnimator(bool value)
        {
            animator.enabled = value;
        }

        public void EnablePhysics(bool value)
        {
            rigidbody.isKinematic = !value;
            rigidbody.useGravity = value;
            collider.isTrigger = !value;
        }

        public void Reparent(Transform parentTransform, bool worldPositionStays)
        {
            transform.SetParent(parentTransform, worldPositionStays);

            if(parentTransform && !worldPositionStays)
            {
                transform.localRotation = Quaternion.identity;
                transform.localPosition = Vector3.zero;
            }
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }


        public abstract void UpdatePrimary(bool press, bool hold, bool release);

        public abstract void UpdateSecondary(bool press, bool hold, bool release);
    }
}
