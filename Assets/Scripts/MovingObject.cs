using UnityEngine;
using System.Collections;
using DG.Tweening;

public abstract class MovingObject : Actor
{
    // 移動時間
    private readonly float _moveTime = 0.025f;
    public float MoveTime{ get { return _moveTime; } }

    [SerializeField]
    private LayerMask blockingLayer;
    private BoxCollider2D boxCollider;

    // 移動tween 
    Tween m_moveTween;

    protected virtual void Start()
    {
    }

    protected bool Move(Layer2D layer, Direction dir)
    {
        //// 移動開始場所
        //var start = new Vector3(ChipUtill.GetChipX(PosX), ChipUtill.GetChipY(PosY), 0.0f);
        var end2d = ChipUtil.MoveToPosition(GetChipPosition(), dir);
        var end = new Vector2(ChipUtil.GetWorldPosX(end2d.x), ChipUtil.GetWorldPosY(end2d.y));

        // 移動先まであたり判定
        //boxCollider.enabled = false;
        //hit = Physics2D.Linecast(start, end, blockingLayer);
        //boxCollider.enabled = true;

        // 当たっていなかったら移動
        var type = ChipUtil.GetExistsChip(layer, end2d);
        if ( (type!=ChipType.Wall)&& (type != ChipType.None))
        {
            // 移動
            if (m_moveTween == null)
            {
                m_moveTween = this.gameObject.transform
                    .DOLocalMove(new Vector3(end.x, end.y, 0.0f)
                    , _moveTime).OnComplete(() => { m_moveTween = null; });
            }
            return true;
        }

        return false;
    }

    // 移動しようとする
    protected virtual void AttemptMove(Layer2D layer,Direction dir)
    {
        // 移動してみる
        bool move = Move(layer,dir);

        // 当たっていなかったら終わりんご
        if (!move)
            return;

        //T hitComponent = hit.transform.GetComponent<T>();
        //// 移動していない時で何かぶつかった時の処理
        //if (!canMove && hitComponent != null)
        //    OnCantMove(hitComponent);
    }

    // 移動していないときの処理
    protected abstract void OnCantMove<T>(T component)
        where T : Component;
}
