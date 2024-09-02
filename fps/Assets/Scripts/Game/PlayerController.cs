//using Photon.Pun;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Realtime;

//public class PlayerController : MonoBehaviourPun, IPunObservable
//{

//    //���
//    public Animator ani;
//    public Rigidbody body;
//    public Transform camTf;  //��������

//    //��ֵ
//    public int CurHp = 10;
//    public int MaxHp = 10;
//    public float MoveSpeed = 3.5f;

//    public float H; //ˮƽֵ
//    public float V; //��ֱֵ
//    public Vector3 dir; //�ƶ�����

//    public Vector3 offset; //��������ɫ֮���ƫ��ֵ

//    public float Mouse_X; //���ƫ��ֵ
//    public float Mouse_Y;
//    public float scroll; //������ֵ
//    public float Angle_X; //x�����ת�Ƕ�
//    public float Angle_Y; //y�����ת�Ƕ�

//    public Quaternion camRotation; //�������ת����Ԫ��

//    public Gun gun; //ǹ�Ľű�

//    //����
//    public AudioClip reloadClip;
//    public AudioClip shootClip;

//    public bool isDie = false;

//    public Vector3 currentPos;
//    public Quaternion currentRotation;

//    void Start()
//    {
//        Angle_X = transform.eulerAngles.x;
//        Angle_Y = transform.eulerAngles.y;

//        ani = GetComponent<Animator>();
//        body = GetComponent<Rigidbody>();
//        gun = GetComponentInChildren<Gun>();
//        camTf = Camera.main.transform;
//        currentPos = transform.position;
//        currentRotation = transform.rotation;
//        if (photonView.IsMine)
//        {
//            Game.uiManager.GetUI<FightUI>("FightUI").UpdateHp(CurHp, MaxHp);
//        }
//    }

//    void Update()
//    {
//        //�ж��Ƿ��Ǳ������  ֻ�ܲ���������ɫ
//        if (photonView.IsMine)
//        {
//            if (isDie == true)
//            {
//                return;
//            }
//            UpdatePosition();
//            UpdateRotation();
//            InputCtl();
//        }
//        else
//        {
//            UpdateLogic();
//        }
//    }

//    //������ɫ���·��͹��������ݣ�λ�� ��ת��
//    public void UpdateLogic()
//    {
//        transform.position = Vector3.Lerp(transform.position, currentPos, Time.deltaTime * MoveSpeed * 10);
//        transform.rotation = Quaternion.Slerp(transform.rotation, currentRotation, Time.deltaTime * 500);
//    }
//    private void LateUpdate()
//    {
//        ani.SetFloat("Horizontal", V );
//        ani.SetFloat("Vertical", H );
//    }

//    public void InputCtl()
//    {
//        if (Input.GetMouseButtonDown(0)) {

//            //�ж��ӵ�����
//            if (gun.BulletCount > 0)
//            {
//                //������ڲ�������ӵ��Ķ������ܿ�ǹ
//                if (ani.GetCurrentAnimatorStateInfo(1).IsName("Reload"))
//                {
//                    return;
//                }
//                gun.BulletCount--;
//                Game.uiManager.GetUI<FightUI>("FightUI").UpdateBulletCount(gun.BulletCount);
//                //���ſ��𶯻�
//                ani.Play("Fire", 1, 0);
//                //gun.Attack();
//                StopAllCoroutines();
//                StartCoroutine(AttackCo());
//            }
//        }

//        if (Input.GetKeyDown(KeyCode.R))
//        {
//            //����ӵ�
//            AudioSource.PlayClipAtPoint(reloadClip, transform.position); //��������ӵ�������
//            ani.Play("Reload");
//            gun.BulletCount = 10;
//            Game.uiManager.GetUI<FightUI>("FightUI").UpdateBulletCount(gun.BulletCount);
//        }
//    }

//    //����Эͬ����
//    IEnumerator AttackCo()
//    {
//        //�ӳ�0.1��ŷ����ӵ�
//        yield return new WaitForSeconds(0.1f);
//        //���������Ч
//        AudioSource.PlayClipAtPoint(shootClip, transform.position);

//        //���߼�� ������ĵ㷢������
//        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, Input.mousePosition.z));
//        //���߿��Ըĳ���ǹ��λ��Ϊ��ʼ�� ���ͣ����������䵽����

//        RaycastHit hit;
//        if (Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask("Player")))
//        {
//            Debug.Log("�䵽��ɫ");
//            hit.transform.GetComponent<PlayerController>().GetHit();
//        }

//        photonView.RPC("AttackRpc", RpcTarget.All);  //�������ִ�� AttackRpc ����
//    }

//    [PunRPC]
//    public void AttackRpc()
//    {
//        gun.Attack();
//    }

//    //����
//    public void GetHit()
//    {
//        if (isDie == true)
//        {
//            return;
//        }

//        //ͬ�����н�ɫ����
//        photonView.RPC("GetHitRPC", RpcTarget.All);
//    }

//    [PunRPC]
//    public void GetHitRPC()
//    {
//        CurHp -= 1;  //��һ��Ѫ
//        if (CurHp <= 0)
//        {
//            CurHp = 0;
//            isDie = true;
//        }

//        if (photonView.IsMine)
//        {
//            Game.uiManager.GetUI<FightUI>("FightUI").UpdateHp(CurHp, MaxHp);
//            Game.uiManager.GetUI<FightUI>("FightUI").UpdateBlood();

//            if (CurHp == 0)
//            {
//                Invoke("gameOver", 3);  //3�����ʾʧ�ܽ���       
//            }
//        }
//    }

//    private void gameOver()
//    {
//        //��ʾ���
//        Cursor.visible = true;
//        Cursor.lockState = CursorLockMode.None;
//        //��ʾʧ�ܽ���
//        Game.uiManager.ShowUI<LossUI>("LossUI").onClickCallBack = OnReset;
//    }

//    //����
//    public void OnReset()
//    {
//        //�������
//        Cursor.visible = false;
//        Cursor.lockState = CursorLockMode.Locked;
//        photonView.RPC("OnResetRPC", RpcTarget.All);
//    }

//    [PunRPC]
//    public void OnResetRPC()
//    {
//        isDie = false;
//        CurHp = MaxHp;
//        if (photonView.IsMine)
//        {
//            Game.uiManager.GetUI<FightUI>("FightUI").UpdateHp(CurHp, MaxHp);
//        }
//    }

//    //��ɫ���ƶ�
//    public void UpdatePosition()
//    {
//        V=Input.GetAxisRaw("Vertical");
//        H=Input.GetAxisRaw("Horizontal");
//        dir = camTf.forward*V+camTf.right*H;
//        body.MovePosition(transform.position + dir * MoveSpeed * Time.deltaTime);
//    }

//    //
//    public void UpdateRotation()
//    {
//        Mouse_X = Input.GetAxisRaw("Mouse X");
//        Mouse_Y = Input.GetAxisRaw("Mouse Y");
//        scroll = Input.GetAxis("Mouse ScrollWheel");

//        Angle_X -= Mouse_Y;
//        Angle_Y += Mouse_X;

//        Angle_X = ClampAngle(Angle_X,-60,60);
//        Angle_Y = ClampAngle(Angle_Y,-360,360);

//        camRotation=Quaternion.Euler(Angle_X,Angle_Y,0);

//        camTf.rotation=camRotation;

//        offset.z += scroll;

//        camTf.position = transform.position + camTf.rotation * offset;

//        transform.eulerAngles=new Vector3(0,camTf.eulerAngles.y,0);
//    }

//    //���ƽǶ���-360��360֮��
//    public float ClampAngle(float val,float min,float max )
//    {
//        if (val < -360)
//        {
//            val += 360;
//        }
//        if (val > 360)
//        {
//            val -= 360;
//        }
//        return Mathf.Clamp(val, min, max);
//    }

//    private void OnAnimatorIK(int layerIndex)
//    {
//        if (ani!= null)
//        {
//            ani.SetBoneLocalRotation(HumanBodyBones.Chest,Quaternion.Euler(Angle_X, 0, 0));
//        }
//    }

//    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//    {
//        if (stream.IsWriting)
//        {
//            //��������
//            stream.SendNext(H);
//            stream.SendNext(V);
//            stream.SendNext(Angle_X);
//            stream.SendNext(transform.position);
//            stream.SendNext(transform.rotation);
//        }
//        else
//        {
//            //��������
//            H = (float)stream.ReceiveNext();
//            V = (float)stream.ReceiveNext();
//            Angle_X = (float)stream.ReceiveNext();
//            currentPos = (Vector3)stream.ReceiveNext();
//            currentRotation = (Quaternion)stream.ReceiveNext();
//        }
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using XLua;
using System.IO;
[Hotfix]
//��ɫ������ 
public class PlayerController : MonoBehaviourPun, IPunObservable
{
    private int count = 0;
    //���
    public Animator ani;
    public Rigidbody body;
    public Transform camTf;  //��������

    //��ֵ
    public int CurHp = 10;
    public int MaxHp = 10;
    public float MoveSpeed = 5f;

    public float H; //ˮƽֵ
    public float V; //��ֱֵ
    public Vector3 dir; //�ƶ�����

    public Vector3 offset; //��������ɫ֮���ƫ��ֵ

    public float Mouse_X; //���ƫ��ֵ
    public float Mouse_Y;
    public float scroll; //������ֵ
    public float Angle_X; //x�����ת�Ƕ�
    public float Angle_Y; //y�����ת�Ƕ�

    public Quaternion camRotation; //�������ת����Ԫ��

    public Gun gun; //ǹ�Ľű�
    public int total;
    //����
    public AudioClip reloadClip;
    public AudioClip shootClip;

    public bool isDie = false;

    public Vector3 currentPos;
    public Quaternion currentRotation;

    void Start()
    {
        if(Game.isFixed == false)
        {
            total = 10;
        }
        else
        {
            total = 5;
            Game.uiManager.GetUI<FightUI>("FightUI").UpdateBulletCount(5, total);
        }

        Angle_X = transform.eulerAngles.x;
        Angle_Y = transform.eulerAngles.y;

        ani = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        gun = GetComponentInChildren<Gun>();
        camTf = Camera.main.transform;
        currentPos = transform.position;
        currentRotation = transform.rotation;
        if (photonView.IsMine)
        {
            Game.uiManager.GetUI<FightUI>("FightUI").UpdateHp(CurHp, MaxHp);
        }
    }


    void Update()
    {
        //�ж��Ƿ��Ǳ������  ֻ�ܲ���������ɫ
        if (photonView.IsMine)
        {
            if (isDie == true)
            {
                return;
            }
            UpdatePosition();
            UpdateRotation();
            InputCtl();
        }
        else
        {
            UpdateLogic();
        }
    }

    //������ɫ���·��͹��������ݣ�λ�� ��ת��
    public void UpdateLogic()
    {
        transform.position = Vector3.Lerp(transform.position, currentPos, Time.deltaTime * MoveSpeed * 10);
        transform.rotation = Quaternion.Slerp(transform.rotation, currentRotation, Time.deltaTime * 500);
    }

    private void LateUpdate()
    {
        ani.SetFloat("Horizontal", H);
        ani.SetFloat("Vertical", V);
        ani.SetBool("isDie", isDie);
    }

    //����λ��
    public void UpdatePosition()
    {
        H = Input.GetAxisRaw("Horizontal");
        V = Input.GetAxisRaw("Vertical");
        dir = camTf.forward * V + camTf.right * H;
        body.MovePosition(transform.position + dir * Time.deltaTime * MoveSpeed);
    }

    //������ת��ͬʱ�����������λ�õ���תֵ��
    public void UpdateRotation()
    {
        Mouse_X = Input.GetAxisRaw("Mouse X");
        Mouse_Y = Input.GetAxisRaw("Mouse Y");
        scroll = Input.GetAxis("Mouse ScrollWheel");

        Angle_X = Angle_X - Mouse_Y;
        Angle_Y = Angle_Y + Mouse_X;

        Angle_X = ClampAngle(Angle_X, -60, 60);
        Angle_Y = ClampAngle(Angle_Y, -360, 360);

        camRotation = Quaternion.Euler(Angle_X, Angle_Y, 0);

        camTf.rotation = camRotation;

        offset.z += scroll;

        camTf.position = transform.position + camTf.rotation * offset;

        transform.eulerAngles = new Vector3(0, camTf.eulerAngles.y, 0);
    }
    private void gameOver()
    {
        //��ʾ���
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //��ʾʧ�ܽ���
        //Game.uiManager.ShowUI<LossUI>("LossUI").onClickCallBack = OnReset;
        AssetBundle ab = null;
        //�ж��Ƿ�����µ���Դ����Ĭ����Դ
        if (!File.Exists(Application.persistentDataPath + "/ui"))
        {
            //����ab��Դ����ui
            ab = ABAssetMgr.Instance.LoadAsset(Application.streamingAssetsPath, "ui");
        }
        else
        {
            //����ab��Դ����ui
            ab = ABAssetMgr.Instance.LoadAsset(Application.persistentDataPath, "ui");
        }
        var loseUI = ab.LoadAsset<GameObject>("LoseUI");
        GameObject lose = Instantiate(loseUI);
        lose.AddComponent<LossUI>();
        lose.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }
    [LuaCallCSharp]
    //��ɫ����
    public void InputCtl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //�ж��ӵ�����
            if (gun.BulletCount > 0)
            {
                //������ڲ�������ӵ��Ķ������ܿ�ǹ
                if (ani.GetCurrentAnimatorStateInfo(1).IsName("Reload"))
                {
                    return;
                }

                gun.BulletCount--;
                Game.uiManager.GetUI<FightUI>("FightUI").UpdateBulletCount(gun.BulletCount,total);
                //���ſ��𶯻�
                ani.Play("Fire", 1, 0);

                StopAllCoroutines();
                StartCoroutine(AttackCo());
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //����ӵ�
            AudioSource.PlayClipAtPoint(reloadClip, transform.position); //��������ӵ�������
            ani.Play("Reload");
            gun.BulletCount = total;
            //Game.gscore = total;
            Game.uiManager.GetUI<FightUI>("FightUI").UpdateBulletCount(gun.BulletCount, total);
        }
    }

    //����Эͬ����
    IEnumerator AttackCo()
    {
        //�ӳ�0.1��ŷ����ӵ�
        yield return new WaitForSeconds(0.1f);
        //���������Ч
        AudioSource.PlayClipAtPoint(shootClip, transform.position);

        //���߼�� ������ĵ㷢������
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, Input.mousePosition.z));
        //���߿��Ըĳ���ǹ��λ��Ϊ��ʼ�� ���ͣ����������䵽����

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask("Player")))
        {
            Debug.Log("�䵽��ɫ");
            hit.transform.GetComponent<PlayerController>().GetHit();
        }
        else
        {
            count++;
            Debug.Log("not:"+count);
        }
        //gun.Attack();
        photonView.RPC("AttackRpc", RpcTarget.All);  //�������ִ�� AttackRpc ����
    }

    [PunRPC]
    public void AttackRpc()
    {
        gun.Attack();
    }

    //����
    public void GetHit()
    {
        if (isDie == true)
        {
            return;
        }
        //ͬ�����н�ɫ����
        photonView.RPC("GetHitRPC", RpcTarget.All);
    }

    [PunRPC]
    public void GetHitRPC()
    {
        CurHp -= 1;  //��һ��Ѫ
        if (CurHp <= 0)
        {
            CurHp = 0;
            isDie = true;
        }

        if (photonView.IsMine)
        {
            Game.uiManager.GetUI<FightUI>("FightUI").UpdateHp(CurHp, MaxHp);
            //        Game.uiManager.GetUI<FightUI>("FightUI").UpdateBlood();

            if (CurHp == 0)
            {
                Invoke("gameOver", 3);  //3�����ʾʧ�ܽ���       
            }
        }
    }



    ////����
    //public void OnReset()
    //{
    //    //�������
    //    Cursor.visible = false;
    //    Cursor.lockState = CursorLockMode.Locked;
    //    photonView.RPC("OnResetRPC", RpcTarget.All);
    //}

    //[PunRPC]
    //public void OnResetRPC()
    //{
    //    isDie = false;
    //    CurHp = MaxHp;
    //    if (photonView.IsMine)
    //    {
    //        Game.uiManager.GetUI<FightUI>("FightUI").UpdateHp(CurHp, MaxHp);
    //    }
    //}

    //���ƽǶ���-360 �� 360֮��
    public float ClampAngle(float val, float min, float max)
    {
        if (val > 360)
        {
            val -= 360;
        }

        if (val < -360)
        {
            val += 360;
        }

        return Mathf.Clamp(val, min, max);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (ani != null)
        {
            Vector3 angle = ani.GetBoneTransform(HumanBodyBones.Chest).localEulerAngles;
            angle.x = Angle_X;
            ani.SetBoneLocalRotation(HumanBodyBones.Chest, Quaternion.Euler(angle));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //��������
            stream.SendNext(H);
            stream.SendNext(V);
            stream.SendNext(Angle_X);
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            //��������
            H = (float)stream.ReceiveNext();
            V = (float)stream.ReceiveNext();
            Angle_X = (float)stream.ReceiveNext();
            currentPos = (Vector3)stream.ReceiveNext();
            currentRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}