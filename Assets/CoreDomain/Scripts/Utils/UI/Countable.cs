using UnityEngine;
using TMPro;
using System.Text;
using Cysharp.Threading.Tasks;

namespace CoreDomain.Scripts.Utils.Command
{
    public class Countable : MonoBehaviour
    {
        private const char ZeroDigit = '0';

        [SerializeField] private float _textAnimtaionSpeed = 2;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private int _minNumOfDigits = 6;
        [SerializeField] string PrefixText = "";

        int _savedTotalNumber = 0;
        int _viewTotalNumber = 0;
        string _zeroDigits = "";

        protected virtual void OnEnable()
        {
            SetStartingValue();
        }

        protected virtual void SetStartingValue(int value = 0)
        {
            _viewTotalNumber = value;
            _savedTotalNumber = value;
            SetText();
        }

        public void SetNumber(int newNumber)
        {
            AddNumberEffect(newNumber - _savedTotalNumber).Forget();
            _savedTotalNumber = newNumber;
        }

        private void UpdateText(int addedNumber)
        {
            _viewTotalNumber += addedNumber;
            SetText();
        }

        private async UniTask AddNumberEffect(int number)
        {
            var numberLeftToAdd = number;
            var isPositive = number >= 0;

            while (numberLeftToAdd > 0)
            {
                var numberToAddThisFrame = Mathf.CeilToInt(Time.deltaTime * number * _textAnimtaionSpeed);

                if ((isPositive && numberToAddThisFrame < numberLeftToAdd) ||
                    (!isPositive && numberToAddThisFrame > numberLeftToAdd))
                {
                    UpdateText(numberToAddThisFrame);
                    numberLeftToAdd -= numberToAddThisFrame;
                }
                else
                {
                    UpdateText(numberLeftToAdd);
                    numberLeftToAdd -= numberLeftToAdd;
                }

                await UniTask.Yield();
            }
        }

        private void SetText()
        {
            var stringTotalNumber = _viewTotalNumber.ToString();
            var numOfZeros = _minNumOfDigits - stringTotalNumber.Length;

            if (numOfZeros != _zeroDigits.Length)
            {
                StringBuilder sb = new StringBuilder();

                for (var i = 0; i < numOfZeros; i++)
                {
                    sb.Append(ZeroDigit);
                }

                _zeroDigits = sb.ToString();
            }

            _text.text = PrefixText + _zeroDigits + stringTotalNumber;
        }
    }
}