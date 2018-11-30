using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ML
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {

        /* Variables
        * * * * * * * * * * * * * * * */
        private Rigidbody m_rigidbody;

        [Header("Settings")]
        [SerializeField] BulletData m_data;
        [SerializeField] UnityEvent m_onSpawn;
        [SerializeField] UnityEvent m_onHitUnit;
        [SerializeField] UnityEvent m_onDeath;

        [Header("Runtime")]
        [SerializeField] float          m_timer;
        [SerializeField] Vector3        m_startingVelocity;
        [SerializeField] List<Collider> m_hitList;


        /* Properties
        * * * * * * * * * * * * * * * */
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

        public BulletData data
        {
            get { return m_data; }
            set { m_data = value; }
        }

        public float timer
        {
            get { return m_timer; }
            private set { m_timer = value; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        private void Start()
        {
            
        }

        private void Update()
        {
            if(data.lifeSpan > 0f)
            {
                if(timer >= data.lifeSpan)
                {
                    Kill();

                    return;
                }
                else
                {
                    timer += Time.deltaTime;
                }
            }
        }


        /* Collisions
        * * * * * * * * * * * * * * * */
        private void OnTriggerEnter(Collider other)
        {
            if(AddHit(other))
            {
                if (other.gameObject.layer == data.layerMask)
                {
                    Debug.Log("Hit Level: " + other.name);
                }

                if (other.gameObject.tag == Unit.Tag)
                {
                    Unit unit;
                    if ((unit = other.GetComponent<Unit>()) && (unit != data.owner))
                    {
                        Debug.Log("Hit Unit: " + other.name);

                        unit.triggers.OnRecieveDamage(new UnitEventData(UnitSystem.current)
                        {
                            damage = data.damage
                        });

                        if(data.owner)
                        {
                            data.owner.triggers.OnDoDamage(new UnitEventData(UnitSystem.current)
                            {
                                damage = data.damage
                            });
                        }
                    }
                }
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        private bool AddHit(Collider other)
        {
            if(other && (!m_hitList.Contains(other)))
            {
                m_hitList.Add(other);
                return true;
            }
            return false;
        }

        public void Spawn()
        {
            timer = 0f;

            rigidbody.position = data.position;

            rigidbody.velocity = data.direction * data.speed;

            m_startingVelocity = rigidbody.velocity;
        }

        public void Kill()
        {
            if(Application.isPlaying)
            {
                Destroy(gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }
    }
}
