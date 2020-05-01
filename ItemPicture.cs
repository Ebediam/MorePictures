using BS;
using UnityEngine;

namespace MorePictures
{

    public class ItemPicture : MonoBehaviour
    {

        public Item item;
        public FrameData data;


        public bool handle1Grabbed;
        public bool handle2Grabbed;

        public float scaleRatio;
        public float originalScale;

        public bool scaleMode;

        protected void Awake()
        {
            item = this.GetComponent<Item>();
                        

            item.OnTeleUnGrabEvent += OnTeleUnGrabAction;
            item.OnTeleGrabEvent += OnTeleGrabAction;

            item.disallowDespawn = true;
            item.rb.isKinematic = true;




        }



        public void OnTeleGrabAction(Handle handle, Telekinesis teleGrabber)
        {
            handle.rb.isKinematic = false;

            if(handle == item.mainHandleRight)
            {
                handle1Grabbed = true;

            }
            else
            {
                handle2Grabbed = true;
                
            }

                
        }
        public void OnTeleUnGrabAction(Handle handle, Telekinesis teleGrabber)
        {

            if (handle == item.mainHandleRight)
            {
                handle1Grabbed = false;
            }
            else
            {
                handle2Grabbed = false;
            }

            if (!handle2Grabbed && !handle1Grabbed)
            {
                handle.rb.isKinematic = true;
            }
        

            if (data == null)
            {
                AddFrameToList();
            }



            UpdateData();
        }


        public void StartScaleMode()
        {
            scaleMode = true;
            scaleRatio = Vector3.Distance(Player.local.body.handLeft.transform.position, Player.local.handRight.transform.position);
            originalScale = transform.localScale.x;

        }

        public void Update()
        {
            if (!handle1Grabbed)
            {
                scaleMode = false;
                return;
            }

            if (!handle2Grabbed)
            {
                scaleMode = false;
                return;
            }

            if (scaleMode)
            {
                float distance = Vector3.Distance(Player.local.body.handLeft.transform.position, Player.local.handRight.transform.position);

                transform.localScale = new Vector3(distance*originalScale /scaleRatio , distance*originalScale / scaleRatio, 1);
                item.mainHandleLeft.transform.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1);
                item.mainHandleRight.transform.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1);
            }
            else
            {
                StartScaleMode();
            }





        }

        public void AddFrameToList()
        {
            data = new FrameData();
            data.id = LevelModuleMorePictures.frameDataList.Count;
            data.imageName = "Dickbutt";
            LevelModuleMorePictures.frameDataList.Add(data);
        }

        public void UpdateData()
        {
            data.xPosition = transform.position.x;
            data.yPosition = transform.position.y;
            data.zPosition = transform.position.z;

            data.xRot = transform.rotation.x;
            data.yRot = transform.rotation.y;
            data.zRot = transform.rotation.z;
            data.wRot = transform.rotation.w;
            data.scale = transform.localScale.x;
            LevelModuleMorePictures.UpdateFrameJson();

        }


    }
}