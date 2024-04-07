using UnityEngine;

namespace ZYYGameKit.UI
{
    public interface IUIData
    {
        
    }
    
    public class BaseUI : MonoBehaviour
    {

        public virtual void Init(IUIData uiData = null)
        {
            
        }

        public virtual void Open(IUIData uiData = null)
        {
            gameObject.SetActive(true);
        }
        
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public virtual void Close()
        {
            Destroy(gameObject);
        }
    }
}