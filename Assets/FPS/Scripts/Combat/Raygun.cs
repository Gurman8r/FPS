using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML
{
    public class Raygun : Item
    {
        public enum Mode
        {
            SingleShot = 0,
            Continuous = 1,
        }

        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Raygun Settings")]
        [SerializeField] LayerMask  m_groundLayer;
        [SerializeField] float      m_maxRange      = 100;
        [SerializeField] Transform  m_shotPos;              // Bullet spawn location
        [SerializeField] Mode       m_shotMode;             // How is input handled?
        [SerializeField] float      m_shotDelay     = 1f;   // Delay between shots
        [Space]
        [SerializeField] Laser      m_laserPrefab;
        [SerializeField] float      m_laserWidth    = 0.1f;
        [SerializeField] float      m_lineDuration  = 0.5f;

        [Header("Runtime")]
        [SerializeField] Vector3    m_lookPos;
        [SerializeField] bool       m_canShoot;
        [SerializeField] float      m_shotTimer;


        /* Properties
        * * * * * * * * * * * * * * * */
        public LayerMask layerMask
        {
            get { return m_groundLayer; }
        }

        public float maxRange
        {
            get { return m_maxRange; }
        }

        public Transform shotPos
        {
            get { return m_shotPos; }
        }

        public Mode shotMode
        {
            get { return m_shotMode; }
        }

        public float shotDelay
        {
            get { return m_shotDelay; }
        }

        public Vector3 lookPos
        {
            get { return m_lookPos; }
        }

        public bool canShoot
        {
            get { return (m_canShoot = (m_shotTimer >= m_shotDelay)); }
        }

        public float shotTimer
        {
            get { return m_shotTimer; }
            private set { m_shotTimer = value; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected override void Start()
        {
            base.Start();

            if (Application.isPlaying)
            {
                m_shotTimer = m_shotDelay;
            }
        }

        protected override void Update()
        {
            base.Update();

            if (Application.isPlaying)
            {
                if (!canShoot)
                {
                    m_shotTimer += Time.deltaTime;
                }

                RaycastHit hit;
                Ray ray = new Ray(shotPos.position, transform.forward);
                if (Physics.Raycast(ray, out hit, m_maxRange, m_groundLayer))
                {
                    m_lookPos = hit.point;
                }
                else
                {
                    m_lookPos = shotPos.position + (transform.forward * m_maxRange);
                }
            }
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(shotPos.position, 0.1f);

            if (owner)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(shotPos.position, lookPos);
                Gizmos.DrawWireSphere(lookPos, 0.1f);
            }
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public override void UpdatePrimary(string axis)
        {   
            switch (m_shotMode)
            {
            case Mode.SingleShot:
            {
                if(Input.GetButtonDown(axis) && canShoot)
                {
                    StartCoroutine(ShootCoroutine(Instantiate(m_laserPrefab)));

                    shotTimer = 0f;
                }
            }
            break;
            case Mode.Continuous:
            {
                if(Input.GetButtonDown(axis))
                {

                }
                if(Input.GetButton(axis))
                {

                }
                if(Input.GetButtonUp(axis))
                {

                }
            }
            break;
            }
        }

        public override void UpdateSecondary(string axis)
        {
        }

        private IEnumerator ShootCoroutine(Laser laser)
        {
            laser.gameObject.SetActive(true);
            laser.posA = shotPos.position;
            laser.posB = lookPos;
            laser.width = m_laserWidth;
            laser.transform.SetParent(null, true);

            yield return new WaitForSeconds(m_lineDuration);

            if (Application.isPlaying)
            {
                Destroy(laser.gameObject);
            }

            yield return new WaitForSeconds(m_shotDelay);
        }
    }

}
