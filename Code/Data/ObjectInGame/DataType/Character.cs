using KinglandStudio.NeosKosmos.Data.BasicValue;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KinglandStudio.NeosKosmos.Data.BasicDataType
{
    [Serializable]
    [SerializeField]
    public class Character
    {
        public static Character bgTranslator = new("Background", -114514);
        public string charaName; //角色名字——不用于游戏内部
        public bool isBG = false;
        public float zoomCoefficient; //角色图片的缩放，实现距离改变
        public int id; //角色ID，虽然不知道现在有什么用
        public int idInScene; //剧中id,分辨同一场景中不同的同一角色（话说这真的有用吗）
        public int nowImageId; //展示图片のid
        public int layer; //图层
        public List<string> images = new(); //存储角色的所有图像
        public Position2 position = new(0, 0); //存储角色在屏幕上的位置
        public GameObject parent;
        public bool Equals(Character other)
        {
            if (other == null || !(charaName == other.charaName && idInScene == other.idInScene))
            {
                return false;
            }
            return true;
        }
        public Sprite GetCharacterImage(int index)
        {
            if (index >= images.Count)
            {
                return null;
            }
            return Resources.Load<Sprite>(@"Character_Image\" + charaName + @"\" + images[index]);
        }
        public Character(string a, int b)
        {
            charaName = a;
            id = b;
        }
        public Character(Character a)
        {
            charaName = a.charaName;
            zoomCoefficient = a.zoomCoefficient;
            id = a.id;
            idInScene = a.idInScene;
            layer = a.layer;
            images = a.images;
            nowImageId = a.nowImageId;
            position.x = a.position.x;
            position.y = a.position.y;
            isBG = a.isBG;
        }
    }
    public class CharacterComparer : IEqualityComparer<Character>
    {
        public bool Equals(Character a, Character b)
        {
            if(ReferenceEquals(a, b))
            {
                return true;
            }
            if(a is null || b is null)
            {
                return false;
            }
            return a.idInScene == b.idInScene;
        }
        public int GetHashCode(Character obj)
        {
            return obj.idInScene;
        }
    }
}