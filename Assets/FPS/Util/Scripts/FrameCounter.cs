using System;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    [ExecuteInEditMode]
    public class FrameCounter : MonoBehaviour
    {
        /*  Variables
        * * * * * * * * * * * * * * * * */
        [SerializeField] Text       m_text;
        [SerializeField] string     m_format = "{0:0.0} ms ({1:0.} fps)";
        [SerializeField] Vector2    m_offset = new Vector2(16f, 16f);
        [SerializeField] TextAnchor m_anchor = TextAnchor.UpperLeft;
        [SerializeField] Color      m_color = Color.white;

        private static float m_fps, m_msec, m_delta;

        /*  Properties
        * * * * * * * * * * * * * * * * */
        public Text text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        public string format
        {
            get { return m_format; }
            set { m_format = value; }
        }

        public Vector2 offset
        {
            get { return m_offset; }
            set { m_offset = value; }
        }

        public TextAnchor anchor
        {
            get { return m_anchor; }
            set { m_anchor = value; }
        }

        public Color color
        {
            get { return m_color; }
            set { m_color = value; }
        }


        /*  Core
        * * * * * * * * * * * * * * * * */
        private void Update()
        {
            if (Application.isPlaying)
            {
                m_delta += (Time.deltaTime - m_delta) * 0.1f;
                m_fps = ((Time.timeScale > 0f) ? (1.0f / m_delta) : 0f);
                m_msec = m_delta * 1000f;

                if (m_text)
                {
                    m_text.text = string.Format(m_format, m_msec, m_fps);
                }
            }
            else
            {
                m_delta = 0f;
                m_fps = 60f;
                m_msec = 16.6f;

                if (m_text)
                {
                    m_text.text = m_format;
                }
            }
        }

        private void OnGUI()
        {
            if (m_text)
                return;

            int width = Screen.width;
            int height = Screen.height * 2 / 100;

            GUI.Label(
                new Rect(m_offset, new Vector2(width, height)),
                string.Format(m_format, m_msec, m_fps),
                new GUIStyle
                {
                    alignment = m_anchor,
                    fontSize = height,
                    normal = new GUIStyleState { textColor = m_color }
                });
        }

    }
}
