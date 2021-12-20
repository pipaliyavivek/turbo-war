using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public bool isLocalPlayer = false;
    bool isInput;
    [SerializeField] public Animator m_Animator;

    Vector3 oldPosition;
    Vector3 currentPosition;
    Quaternion oldRotation;
    Quaternion currentRotation;
    public FixedJoystick mJoysticks;

    // Use this for initialization
    void Start()
    {
        oldPosition = transform.position;
        currentPosition = oldPosition;
        oldRotation = transform.rotation;
        currentRotation = oldRotation;
        SimpleCamFollow.instance.m_Target = this.transform;
        mJoysticks = GameObject.FindObjectOfType<FixedJoystick>();
        //	transform.rotation = Quaternion.identity;
        //ShootNowl = GameObject.Find("Shootbtn").GetComponent<Button>();
        GameManager.Instance.mShootBullet.onClick.AddListener(shootBullet);
    }
    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return;}
        Movement();
        if (currentPosition != oldPosition)
        {
            NetworkManager.instance.GetComponent<NetworkManager>().CommandMove(transform.position);
            oldPosition = currentPosition;
        }
        if (currentRotation != oldRotation)
        {
            NetworkManager.instance.GetComponent<NetworkManager>().CommandTurn(transform.rotation);
            oldRotation = currentRotation;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shootBullet();
        }
    }
    public void CmdFire()
    {
        var bullet = Instantiate(bulletPrefab,
                                 bulletSpawn.position,
                                 bulletSpawn.rotation) as GameObject;
        Bullet b = bullet.GetComponent<Bullet>();
        b.playerFrom = this.gameObject;
        print("setting the velocity");
        print(bullet.transform.up);
        bullet.GetComponent<Rigidbody>().isKinematic = false;
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.up * 6, ForceMode.VelocityChange);
        Destroy(bullet, 2.0f);
    }
    void Rotate(Vector3 moveDir)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * 5f);
    }
    void Movement()
    {
        // Pass input value
        var horizontal = mJoysticks.Horizontal;
        var vertical = mJoysticks.Vertical;
        Vector3 movementDir = new Vector3(horizontal, 0f, vertical);
        isInput = movementDir != Vector3.zero;
        if (!isInput)
        {
            m_Animator.SetBool("Jump", false);
            m_Animator.SetBool("Run", false);
            return;
        }
        else
        {
            m_Animator.SetBool("HandAttack", false);
            m_Animator.SetBool("Jump", false);
            m_Animator.SetBool("Run", true);
            transform.localPosition = new Vector3(transform.localPosition.x + movementDir.x * GameManager.Instance.Movespeed * Time.deltaTime, transform.localPosition.y, transform.localPosition.z + movementDir.z * GameManager.Instance.Movespeed * Time.deltaTime);//old is -0.633f
            Rotate(movementDir);
        }
    }
    public void shootBullet()
    {
        NetworkManager n = NetworkManager.instance.GetComponent<NetworkManager>();
        if (m_Animator) 
        {
            m_Animator.SetBool("GunAttack", true);
            DOVirtual.DelayedCall(.25f, () =>
            {
                n.CommandShoot();
                StartCoroutine(stopAnimation());
            });
        }
    }
    IEnumerator stopAnimation()
    {
        yield return new WaitForSeconds(1.167f);
        m_Animator.SetBool("GunAttack", false);
    }

}
