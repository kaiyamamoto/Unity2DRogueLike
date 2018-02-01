using UnityEngine;
using System.Collections;

public class Enemy : MovingObject
{
    public int playerDamage;

    private Transform target;                      
    private bool skipMove;                             


    protected override void Start()
    {
        InGameManager.GetInstance().AddEnemyToList(this);

        target = GameObject.FindGameObjectWithTag("Player").transform;

        base.Start();
    }


    protected override Vector3 AttemptMove(Layer2D layer, Direction dir)
    {
        if (skipMove)
        {
            skipMove = false;
            return new Vector3(0.0f, 0.0f, 0.0f);

        }

        base.AttemptMove(layer,dir);

        skipMove = true;

        return new Vector3(0.0f, 0.0f, 0.0f);
    }


    public void MoveEnemy(Layer2D layer)
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)

            yDir = target.position.y > transform.position.y ? 1 : -1;

        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        var dir = ChipUtil.CreateDirection(new Vector2Int(xDir, yDir));

        AttemptMove(layer, dir);
    }

    protected override void OnCantMove<T>(T component)
    {
        // プレイヤーと当たった時
        Player hitPlayer = component as Player;
    }
}
