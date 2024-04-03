using UnityEngine;
using UnityEngine.EventSystems;

namespace ZYYGameKit.UI
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class PolygonButton : UnityEngine.UI.Button
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            // 将点击的屏幕位置转换为世界射线
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);

            // 进行2D射线检测
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            // 检查是否碰到了PolygonCollider2D
            if (hit.collider != null)
            {
                var polygonCollider2D = hit.collider as PolygonCollider2D;
                if (polygonCollider2D)
                {
                    polygonCollider2D.GetComponent<PolygonButton>()?.onClick?.Invoke();
                }
            }
        }
    }
}