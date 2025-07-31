using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Draggable : MonoBehaviour
{
    [SerializeField] private float dropSpeed;
    [SerializeField] private GameObject shadowPrefab;
    [HideInInspector] public GameObject shadow;

    private SqueezeAndStretch squeeze;
    private Blink blink;
    private Rigidbody2D rb;

    private RaycastHit2D ray;

    private void Start()
    {
        squeeze = GetComponent<SqueezeAndStretch>();
        blink = GetComponent<Blink>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (shadow != null)
            ShadowFollow();
    }

    private void OnMouseDown()
    {
        Drag();
    }

    private void OnMouseUp()
    {
        Drop();
    }

    private void Drag()
    {
        DragAndDrop.Instance.Register(this.gameObject);
        squeeze.Trigger();
        blink.Trigger();

        if (shadow == null)
            shadow = Instantiate(shadowPrefab, transform.position - new Vector3(0, 0.25f, 0), Quaternion.identity);
    }

    private void Drop()
    {
        DragAndDrop.Instance.UnRegister();
        rb.velocity = Vector2.down * dropSpeed * Time.fixedDeltaTime;
    }

    private void ShadowFollow()
    {
        int groundLayer = 6;
        int groundMask = 1 << groundLayer;
        int stomachLayer = 8;
        int stomachMask = 1 << stomachLayer;
        int combinedMask = groundMask | stomachMask;

        ray = Physics2D.Raycast(transform.position, Vector2.down, 1000f, combinedMask);

        if (shadow != null)
        {
            if (ray.distance > 10f)
                shadow.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            else if (ray.distance <= 10f && ray.distance > 5f)
                shadow.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            else
                shadow.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        shadow.transform.position = new Vector3(transform.position.x, ray.point.y, 0);

        if (rb.velocity == Vector2.zero && !DragAndDrop.Instance.isDragging)
            Destroy(shadow);
    }
}
