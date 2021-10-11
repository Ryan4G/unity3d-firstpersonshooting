using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Player")]
public class Player : MonoBehaviour
{
    Transform m_transform;

    CharacterController m_ch;

    float m_moveSpeed = 3.0f;

    float m_gravity = 2.0f;

    public int m_life = 5;

    Transform m_camTransform;

    Vector3 m_camRot;

    float m_camHeight = 1.4f;

    // Start is called before the first frame update
    void Start()
    {
        m_transform = this.transform;

        m_ch = this.GetComponent<CharacterController>();

        m_camTransform = Camera.main.transform;

        // init camera position
        m_camTransform.position = m_transform.TransformPoint(0, m_camHeight, 0);

        m_camTransform.rotation = m_transform.rotation;

        m_camRot = m_camTransform.eulerAngles;

        // lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_life <= 0)
        {
            return;
        }

        Control();
    }

    void Control()
    {
        // get mouse moved distance
        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");

        m_camRot.x -= rv;
        m_camRot.y += rh;
        m_camTransform.eulerAngles = m_camRot;

        Vector3 camrot = m_camTransform.eulerAngles;
        camrot.x = 0;
        camrot.z = 0;
        m_transform.eulerAngles = camrot;

        Vector3 motion = Vector3.zero;
        motion.x = Input.GetAxis("Horizontal") * m_moveSpeed * Time.deltaTime;
        motion.z = Input.GetAxis("Vertical") * m_moveSpeed * Time.deltaTime;
        motion.y -= m_gravity * Time.deltaTime;

        m_ch.Move(m_transform.TransformDirection(motion));

        m_camTransform.position = m_transform.TransformPoint(0, m_camHeight, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Spawn.tif");
    }
}
