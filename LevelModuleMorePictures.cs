using UnityEngine;
using BS;
using System.IO;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;



namespace MorePictures
{
    // This create an level module that can be referenced in the level JSON
    public class LevelModuleMorePictures : LevelModule
    {
        

        public ItemData pictureFrameData;
        public Item picture;
        public static List<FrameData> frameDataList = new List<FrameData>();

        public static string levelName;
        public override void OnLevelLoaded(LevelDefinition levelDefinition)
        {
            initialized = true;
            levelName = levelDefinition.data.id;
            try
            {
                FillFrameList();
                
                if(frameDataList.Count > 0)
                {
                    SpawnFrames();
                }

            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

        }


        public static void FillFrameList()
        {
            StreamReader streamReader = new StreamReader(Application.streamingAssetsPath + "/MorePictures/FrameData_"+levelName+".json");
            string _output = streamReader.ReadToEnd();
            Debug.Log(_output);
            streamReader.Close();

            if(_output != "")
            {
                frameDataList = JsonConvert.DeserializeObject<List<FrameData>>(_output);

                Debug.Log("FrameDataList contains " + frameDataList.Count + " elements");
            }

        }

        public void SpawnFrames()
        {
            pictureFrameData = Catalog.current.GetData<ItemData>("PictureFrame");

            foreach (FrameData data in frameDataList)
            {             
                picture = pictureFrameData.Spawn();

                ItemPicture itemFrame = picture.GetComponent<ItemPicture>();

                Vector3 position = new Vector3(data.xPosition, data.yPosition, data.zPosition);
                Quaternion rotation = new Quaternion(data.xRot, data.yRot, data.zRot, data.wRot);

                picture.transform.position = position;
                picture.transform.rotation = rotation;

                picture.transform.localScale = new Vector3(data.scale, data.scale, 1f);

                itemFrame.item.mainHandleLeft.transform.localScale = new Vector3(1 / data.scale, 1 / data.scale, 1f);
                itemFrame.item.mainHandleRight.transform.localScale = new Vector3(1 / data.scale, 1 / data.scale, 1f);


                if(File.Exists(Application.streamingAssetsPath + "/MorePictures/" + data.imageName))
                {
                    Material mat = itemFrame.transform.Find("Picture").GetComponent<MeshRenderer>().material;

                    Material newMat = new Material(mat);
                    itemFrame.transform.Find("Picture").GetComponent<MeshRenderer>().material = newMat;
                    

                    byte[] imageData = File.ReadAllBytes(Application.streamingAssetsPath + "/MorePictures/"+data.imageName);

                    Debug.Log("imageData length: " + imageData.Length);
                    
                    
                    Texture2D tex = new Texture2D(512, 512);

                    tex.LoadImage(imageData);
                    newMat.SetTexture("_texture", tex);

                }







                itemFrame.data = data;

            }
        }



        public static void UpdateFrameJson()
        {
            
            string output = JsonConvert.SerializeObject(frameDataList);

            StreamWriter streamWriter = new StreamWriter(Application.streamingAssetsPath + "/MorePictures/FrameData_" + levelName + ".json");
            streamWriter.Write(output);
            streamWriter.Close();
        }

       


    }
}
