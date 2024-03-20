using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.Data.Manager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KinglandStudio.NeosKosmos.UI
{
    public class CharacterDisplay : MonoBehaviour
    {
        public Character character;
        public Position2 velocity; //per second
        public Coroutine noiseCoroutine;
        private Vector3 nowPosition;
        private System.Random random = new();
        private SceneShowingManager sceneShowingManager;
        public void ResetGameobject()
        {
            if(character.parent == null)
            {
                character.parent = GameObject.Find(character.idInScene.ToString());
            }
        }
        public AbnormalStatus SetImage(Sprite targetImage)
        {
            if(targetImage == null)
            {
                return AbnormalStatus.Regular; //测试用，记得删
                return AbnormalStatus.Image_Not_Found;
            }
            GetComponent<Image>().sprite = targetImage;
            return AbnormalStatus.Regular;
        }
        public void StopMovingByVelocity()
        {
            velocity = new(0, 0);
        }
        public void TransformTo(float x, float y, float z)
        {
            gameObject.transform.localPosition = new(x, y, z);
            character.position.x = x;
            character.position.y = y;
        }
        public void TransformTo(Position2 position)
        {
            gameObject.transform.localPosition = new(position.x, position.y, transform.localPosition.z);
            character.position = position;
        }
        public void Zoom(float a)
        {
            gameObject.transform.localScale = Vector3.one * a;
            character.zoomCoefficient = a;
        }
        public void RotateChara(float angleX, float angleY, float angleZ) //X->上下，Y->左右，Z->斜向
        {
            gameObject.transform.Rotate(angleX, angleY, angleZ);
        }
        public void SetNoise(float lastTime, int xMax, int yMax, int xMin, int yMin)
        {
            noiseCoroutine = StartCoroutine(InfluenceByNoise(lastTime == -1 ? 99999999 : lastTime, xMax, yMax, xMin, yMin));
        }
        public IEnumerator InfluenceByNoise(float lastTime, int xMax, int yMax, int xMin, int yMin)
        {
            nowPosition = gameObject.transform.localPosition;
            float endTime = lastTime + Time.time;
            float nx, ny;
            while(endTime > Time.time)
            {
                nx = gameObject.transform.localPosition.x + random.Next(xMin / 4, xMax / 4 - 1) + (float)random.NextDouble();
                ny = gameObject.transform.localPosition.y + random.Next(yMin / 4, yMax / 4 - 1) + (float)random.NextDouble();
                if(nx - nowPosition.x > xMax)
                {
                    nx = nowPosition.x + xMax;
                }
                else if(nx - nowPosition.x < xMin)
                {
                    nx = nowPosition.x + xMin;
                }
                if(ny - nowPosition.y > yMax)
                {
                    ny = nowPosition.y + yMax;
                }
                else if (ny - nowPosition.y < yMin)
                {
                    ny = nowPosition.y + yMin;
                }
                gameObject.transform.localPosition = new(nx, ny, nowPosition.z);
                yield return new WaitForSeconds(0.04f);
            }
            StopNoise();
        }
        public void StopNoise()
        {
            if(noiseCoroutine != null)
            {
                gameObject.transform.localPosition = nowPosition;
                StopCoroutine(noiseCoroutine);
            }
            noiseCoroutine = null;
        }
        public void UpdateThePosition()
        {
            Zoom(character.zoomCoefficient);
            TransformTo(character.position.x, character.position.y, 0);
        }
        private void UpdatePositionInformation()
        {
            if(sceneShowingManager.characters.Count <= character.idInScene)
            {
                return;
            }
            sceneShowingManager.characters[character.idInScene] = character;
        }
        private void Update()
        {
            if(velocity != new Position2(0, 0) && velocity != null)
            {
                TransformTo(transform.localPosition.x + (velocity.x * Time.deltaTime), transform.localPosition.y + (velocity.y * Time.deltaTime), transform.localPosition.z);
            }
        }
        private void FixedUpdate()
        {
            UpdatePositionInformation();
        }
        private void Start()
        {
            sceneShowingManager = GameObject.Find("VisualManager").GetComponent<SceneShowingManager>();
        }
    }
}