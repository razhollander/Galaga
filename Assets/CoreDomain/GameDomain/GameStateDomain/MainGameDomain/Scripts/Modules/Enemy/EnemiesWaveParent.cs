using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class EnemiesWaveParent : MonoBehaviour
{
    public void StartHorizontalYoyoMoving()
    {
        var cancellationTokenOnDestroy = this.GetCancellationTokenOnDestroy();
        transform.DOMove(transform.position - Vector3.right * transform.position.x, 3).SetLoops(-1, LoopType.Yoyo).WithCancellation(cancellationTokenOnDestroy);
    }
}
