using UnityEngine;
using UnityEngine.UI;

namespace Criaath.Goldio
{
    public class GoldioSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private GoldioType _type;

        private void Awake()
        {
            _slider.onValueChanged.AddListener((value) =>
            {
                GoldioManager.Instance.SetVolume(_type, (float)value);
            });
        }
    }
}
