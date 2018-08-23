using UnityEngine;
using System.Collections;
namespace MySpace.Animation
{
    public class OptionAnimation : MonoBehaviour
    {
        public Sprite[] On;
        public Sprite[] Off;
        public bool isOn;
        float volume;
        public void OnClick()
        {
            if (isOn == true)
            {
                isOn = false;
                volume = AudioListener.volume;
                AudioListener.volume = 0;
                GetComponent<SpriteRenderer>().sprite = Off[0];
            }
            else
            {
                isOn = true;
                AudioListener.volume = volume;
                GetComponent<SpriteRenderer>().sprite = On[0];
            }
            //Debug.Log("!!!");
        }
        public void OnPress()
        {
            if (isOn)
                GetComponent<SpriteRenderer>().sprite = Off[On.Length - 1];
            else
                GetComponent<SpriteRenderer>().sprite = On[On.Length - 1];
        }
        public void OnFree()
        {
            if (isOn)
                GetComponent<SpriteRenderer>().sprite = On[0];
            else
                GetComponent<SpriteRenderer>().sprite = Off[0];
        }
    }
}