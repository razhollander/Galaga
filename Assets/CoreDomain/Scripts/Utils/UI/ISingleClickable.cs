using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.UI;

namespace CoreDomain.Scripts.Utils.UI
{
    public interface ISingleClickable
    {
        public IDisposable SubscribeToClick(Button button)
        {
            return button.OnClickAsObservable().Take(1).Subscribe(unit => OnCloseClicked());
        }

        public IDisposable SubscribeToClicks(Button[] buttons)
        {
            IObservable<Unit> closeButtonsClickStream = null;

            foreach (var closeButton in buttons)
            {
                closeButtonsClickStream = closeButtonsClickStream == null
                    ? closeButton.OnClickAsObservable()
                    : closeButtonsClickStream.Merge(closeButton.OnClickAsObservable());
            }

            return closeButtonsClickStream.Take(1).Subscribe(unit => OnCloseClicked());
        }

        public UniTask OnCloseClicked();
    }
}