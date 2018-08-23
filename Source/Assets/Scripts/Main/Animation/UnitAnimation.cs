using UnityEngine;
using System.Collections;
using MySpace.Main;

namespace MySpace.Animation
{
    public class UnitAnimation : MonoBehaviour
    {
        public Vector3 offset = new Vector3(0.1f,0.1f,0);
        public AudioSource audio;
        public AudioClip clip;
        #region Wait
        WaitForEndOfFrame waitEndFrame = new WaitForEndOfFrame();
        /// <summary>
        /// wait 0.025s
        /// </summary>
        WaitForSeconds wait0 = new WaitForSeconds(0.025f);
        /// <summary>
        /// wait 0.05s
        /// </summary>
        WaitForSeconds wait1 = new WaitForSeconds(0.05f);
        /// <summary>
        /// wait 0.1s
        /// </summary>
        WaitForSeconds wait2 = new WaitForSeconds(0.1f);
        /// <summary>
        /// wait 0.25s
        /// </summary>
        WaitForSeconds wait3 = new WaitForSeconds(0.25f);
        /// <summary>
        /// wait 0.5s
        /// </summary>
        WaitForSeconds wait4 = new WaitForSeconds(0.5f);
        /// <summary>
        /// wait 1s
        /// </summary>
        WaitForSeconds wait5 = new WaitForSeconds(1f);
        /// <summary>
        /// wait 5s
        /// </summary>
        WaitForSeconds wait6 = new WaitForSeconds(5f);
        /// <summary>
        /// wait 10s
        /// </summary>
        WaitForSeconds wait7 = new WaitForSeconds(10f);
        /// <summary>
        /// wait 100s
        /// </summary>
        WaitForSeconds wait8 = new WaitForSeconds(100f);
        #endregion
        public Sprite stand0;
        public Sprite stand1;
        public Sprite attack0;
        public Sprite attack1;
        // Use this for initialization
        public bool isAttack;
        public bool isQuick;
        public bool isExchange;
        public bool isAlive;
        Vector3 postion;
        void Start()
        {
            audio = gameObject.AddComponent<AudioSource>();
            audio.clip = clip;
            isAlive = true;
            postion = transform.position;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public void Exchange(bool value)
        {
            if (isExchange != value)
            {
                transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
                isExchange = value;
            }
            if (isExchange)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
        public void Attack()
        {
            if (isAttack)
            {
                isQuick = true;
                attackTime = 0;
                spriteRenderer.sprite = stand0;
            }
            isAttack = true;
        }
        public void Death()
        {
            audio.Play();
            isAlive = false;
            spriteRenderer.sprite = GameController.sceneType.deaths[Random.Range(0, GameController.sceneType.deaths.Length)];
            if (isExchange)
            {
                transform.position = transform.position + new Vector3(offset.x, offset.y, offset.z);
            }
            else
            {
                transform.position = transform.position + new Vector3(-offset.x, offset.y, offset.z);
            }
        }
        SpriteRenderer spriteRenderer;
        float attackTime = 0;
        float standTime = 0;
        bool isStand = true;
        void Update()
        {
            if (isAlive)
            {
                if (isAttack)
                {
                    attackTime += Time.deltaTime;
                    if (attackTime >= 0.05f)
                    {
                        spriteRenderer.sprite = attack0;
                    }
                    if (attackTime >= 0.075f)
                    {
                        spriteRenderer.sprite = attack1;
                    }
                    if (attackTime >= 0.125f)
                    {
                        spriteRenderer.sprite = attack0;
                    }
                    if (attackTime >= 0.25f)
                    {
                        spriteRenderer.sprite = stand0;
                        isAttack = false;
                        isQuick = false;
                        attackTime = 0;
                    }
                }
                else
                {
                    standTime += Time.deltaTime;
                    if (isStand == false && standTime > 0.20f)
                    {
                        isStand = true;
                        spriteRenderer.sprite = stand0;
                        standTime = 0;
                    }
                    if (isStand == true && standTime > 0.2f)
                    {
                        isStand = false;
                        spriteRenderer.sprite = stand1;
                        standTime = 0;
                    }
                }
            }
        }
        /**协程响应速度不理想，改用帧刷新*/
        //IEnumerator RunStatus()
        //{
        //    bool isStand = true;
        //    float standTime = 0;
        //    float attackTime = 0;
        //    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        //    while (true)
        //    {
        //        if (isAttack)
        //        {
        //            standTime = 0f;
        //            //yield return wait0;
        //            spriteRenderer.sprite = attack0;
        //            if (!isQuick) yield return wait1; else yield return 0;
        //            spriteRenderer.sprite = attack1;
        //            if (!isQuick) yield return wait1; else yield return 0;
        //            spriteRenderer.sprite = attack0;
        //            if (!isQuick) yield return wait1; else yield return 0;
        //            spriteRenderer.sprite = stand0;
        //            isAttack = false;
        //            isQuick = false;
        //            isStand = true;
        //        }
        //        else
        //        {
        //            standTime += Time.deltaTime;
        //            if (isStand == false && standTime > 0.20f)
        //            {
        //                isStand = true;
        //                spriteRenderer.sprite = stand0;
        //                standTime = 0;
        //            }
        //            if(isStand == true && standTime > 0.2f)
        //            {
        //                isStand = false;
        //                spriteRenderer.sprite = stand1;
        //                standTime = 0;
        //            }
        //            yield return wait0;
        //        }
        //    }
        //}
    }
}