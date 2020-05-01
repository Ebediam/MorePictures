using BS;

namespace MorePictures
{
    // This create an item module that can be referenced in the item JSON
    public class ItemModulePicture : ItemModule
    {
        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            item.gameObject.AddComponent<ItemPicture>();
        }
    }
}
