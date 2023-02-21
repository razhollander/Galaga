using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class EnemiesWaveParent : MonoBehaviour
{
    public void StartHorizontalYoyoMoving()
    {
        var cancellationTokenOnDestroy = this.GetCancellationTokenOnDestroy();
        var position = transform.position;
        transform.DOMove(position - Vector3.right * position.x, 3).SetLoops(-1, LoopType.Yoyo).WithCancellation(cancellationTokenOnDestroy);
    }
}
