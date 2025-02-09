using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBossAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private GameObject ShieldProjectile;
    [Header("Four Direction")]
    private Vector2[] cardinalPoints = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    private Vector2[] diagonalPoints = new Vector2[]
    {
        new Vector2(1, 1).normalized,
        new Vector2(-1, 1).normalized,
        new Vector2(1, -1).normalized,
        new Vector2(-1, -1).normalized
    };

    [Header("방패 속도 설정")]
    private float shieldSpeed = 5.0f;
    private float shieldDuration = 2.0f;

    [Header("돌진 속도 설정")]
    private float dashSpeed = 3.0f;
    private float dashWaitingDuration = 2.0f;
    private float dashDuration = 1.0f;
    private Rigidbody2D EnemyRb;

    [Header("경고선 설정")]
    public GameObject redLinePrefab;
    public float lineLength;
    public float waitingDuration = 1.5f;
    private List<GameObject> warningLines = new List<GameObject>();

    // TODO: 살짝 답답하군
    public void Start()
    {
        lineLength = shieldSpeed * shieldDuration;
        EnemyRb = GetComponent<Rigidbody2D>();
    }

    public void Attack()
    {
        StartCoroutine(ExecuteDashAttackAfterDelay());
    }

    public void ShieldAttack()
    {
        StartCoroutine(TotalShieldAttack());
    }

    // 방패 던지기 패턴

    private IEnumerator TotalShieldAttack()
    {
        yield return StartCoroutine(ExecuteShieldAttackAfterDelay(cardinalPoints));
        // TODO: 매직넘버 지우기
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(ExecuteShieldAttackAfterDelay(diagonalPoints));

        GetComponent<ShieldBossEnemyAI>().IsBlocking = false;
    }
    private void ShieldPattern(Vector2[] Points)
    {
        foreach (var point in Points)
        {
            GameObject Shield = Instantiate(ShieldProjectile, transform);
            Shield.SetActive(true);
            ShieldProjectileAttack temp = Shield.GetComponent<ShieldProjectileAttack>();
            temp.SetDir(point);
            temp.StartCoroutine(temp.ShieldAttack());
        }
    }

    private void ShowWarningLines(Vector2[] Points, float lineLength)
    {
        if (Points.Length == 0 || Points == null)
        {
            // 예외처리
            Debug.Log("No Points to make Warning Lines");
            return;
        }

        Vector2 startPosition = transform.position;
        foreach (Vector2 dir in Points)
        {
            GameObject lineObj = Instantiate(redLinePrefab, startPosition, Quaternion.identity);
            LineRenderer lr = lineObj.GetComponent<LineRenderer>();
            if (lr != null )
            {
                lr.SetPosition(0, startPosition);
                lr.SetPosition(1, startPosition + dir * lineLength);
            }
            warningLines.Add(lineObj);
        }
    }

    private IEnumerator ExecuteShieldAttackAfterDelay(Vector2[] Points)
    {
        ShowWarningLines(Points, this.lineLength);
        yield return new WaitForSeconds(waitingDuration);
        
        foreach (GameObject line in warningLines)
        {
            Destroy(line);
        }
        warningLines.Clear();

        ShieldPattern(Points);
    }

    // 대쉬 패턴
    private void DashPattern(Vector2[] dir)
    {
        EnemyRb = GetComponent<Rigidbody2D>();
        if (dir.Length > 0)
        {
            EnemyRb.AddForce(dir[0] * dashSpeed, ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("No Points to make Warning Lines");
            return;
        }
    }

    private IEnumerator ExecuteDashAttackAfterDelay()
    {
        // 편법으로 하기.
        Vector2[] dir = new Vector2[] { GetComponent<ShieldBossEnemyMovement>().GetDirection().normalized };
        ShowWarningLines(dir, dashDuration * dashSpeed);
        yield return new WaitForSeconds(dashWaitingDuration);

        foreach (GameObject line in warningLines)
        {
            Destroy(line);
        }
        warningLines.Clear();

        DashPattern(dir);
        yield return new WaitForSeconds(dashDuration);
        EnemyRb.velocity = Vector3.zero;

        GetComponent<ShieldBossEnemyAI>().IsBlocking = false;
    }
}
