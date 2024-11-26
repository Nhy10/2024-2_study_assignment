using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    UIManager MyUIManager;

    public GameObject BallPrefab;   // prefab of Ball

    // Constants for SetupBalls
    public static Vector3 StartPosition = new Vector3(0, 0, -6.35f);
    public static Quaternion StartRotation = Quaternion.Euler(0, 90, 90);
    const float BallRadius = 0.286f;
    const float RowSpacing = 0.02f;

    GameObject PlayerBall;
    GameObject CamObj;

    const float CamSpeed = 3f;

    const float MinPower = 15f;
    const float PowerCoef = 1f;

    void Awake()
    {
        // PlayerBall, CamObj, MyUIManager를 얻어온다.
        // ---------- TODO ---------- 
        PlayerBall = GameObject.Find("PlayerBall");
        CamObj = GameObject.Find("Main Camera");
        MyUIManager = FindObjectOfType<UIManager>();

        // -------------------- 
    }

    void Start()
    {
        SetupBalls();
    }

    // Update is called once per frame
    void Update()
    {
        // 좌클릭시 raycast하여 클릭 위치로 ShootBallTo 한다.
        // ---------- TODO ---------- 
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                ShootBallTo(hit.point);
            }
        }
        // -------------------- 
    }

    void LateUpdate()
    {
        CamMove();
    }

    void SetupBalls()
    {
        // 15개의 공을 삼각형 형태로 배치한다.
        // 가장 앞쪽 공의 위치는 StartPosition이며, 공의 Rotation은 StartRotation이다.
        // 각 공은 RowSpacing만큼의 간격을 가진다.
        // 각 공의 이름은 {index}이며, 아래 함수로 index에 맞는 Material을 적용시킨다.
        // Obj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/ball_1");
        // ---------- TODO ---------- 

        int idx = 1;
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col <= row; col++)
            {
                GameObject ball = Instantiate(BallPrefab);
                ball.name = $"{idx}";
                float xoffset = col * (BallRadius * 2 + RowSpacing) - (row * (BallRadius + RowSpacing));
                float zoffset = row * (BallRadius * 2 + RowSpacing);

                ball.transform.position = StartPosition + new Vector3(xoffset, 0, zoffset);
                ball.transform.rotation = StartRotation;

                string materialname = $"Materials/ball_{idx}";
                ball.GetComponent<MeshRenderer>().material = Resources.Load<Material>(materialname);

                idx++;
            }
        }
        // -------------------- 
    }
    void CamMove()
    {
        // CamObj는 PlayerBall을 CamSpeed의 속도로 따라간다.
        // ---------- TODO ---------- 
        Vector3 targetposition = new Vector3(PlayerBall.transform.position.x, CamObj.transform.position.y, PlayerBall.transform.position.z);
        CamObj.transform.position = Vector3.Lerp(CamObj.transform.position, targetposition, CamSpeed * Time.deltaTime);
        // targetposition * CamSpeed * Time.deltaTime;
        // -------------------- 
    }

    float CalcPower(Vector3 displacement)
    {
        return MinPower + displacement.magnitude * PowerCoef;
    }

    void ShootBallTo(Vector3 targetPos)
    {
        // targetPos의 위치로 공을 발사한다.
        // 힘은 CalcPower 함수로 계산하고, y축 방향 힘은 0으로 한다.
        // ForceMode.Impulse를 사용한다.
        // ---------- TODO ---------- 
        Rigidbody rb = PlayerBall.GetComponent<Rigidbody>();
        if (rb)
        {
            Vector3 direction = targetPos - PlayerBall.transform.position;
            direction.y = 0;
            float power = CalcPower(direction);

            rb.AddForce(direction.normalized * power, ForceMode.Impulse); //단위벡터 * direction 크기
        }
        // -------------------- 
    }

    // When ball falls
    public void Fall(string ballName)
    {
        // "{ballName} falls"을 1초간 띄운다.
        // ---------- TODO ---------- 
        string message = $"{ballName} falls";
        MyUIManager.DisplayText(message, 1f);
        // -------------------- 
    }
}
