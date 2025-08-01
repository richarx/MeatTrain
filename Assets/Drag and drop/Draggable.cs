using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class Draggable : MonoBehaviour
{
    [SerializeField] private float dropSpeed;
    public float DropSpeed => dropSpeed;

    [SerializeField] private GameObject shadowPrefab;
    [SerializeField] private LayerMask shadowInteractMask;
    [HideInInspector] public GameObject shadow;

    [HideInInspector] public UnityEvent OnDrag = new UnityEvent();
    [HideInInspector] public UnityEvent OnDrop = new UnityEvent();
    [HideInInspector] public UnityEvent OnFallGround = new UnityEvent();


    public bool IsBeingDragged => isBeingDragged;
    private bool isBeingDragged;

    public bool IsFalling => isFalling;
    private bool isFalling;

    private SqueezeAndStretch squeeze;
    private Blink blink;
    private Rigidbody2D rb;
    private Digestable digestable;

    private RaycastHit2D ray;

    private void Start()
    {
        squeeze = GetComponent<SqueezeAndStretch>();
        blink = GetComponent<Blink>();
        rb = GetComponent<Rigidbody2D>();
        digestable = GetComponent<Digestable>();
    }

    private void Update()
    {
        if (digestable.isBeingDigested)
            return;

        if (shadow != null)
            ShadowScale();

        if (!isFalling && shadow != null)
            ShadowFollow();

        if (isFalling == true && transform.position.y < shadow.transform.position.y + 0.5f)
            StopFalling();
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
        if (digestable.isBeingDigested)
            return;

        OnDrag.Invoke();

        isFalling = false;

        DragAndDrop.Instance.Register(this.gameObject);
        isBeingDragged = true;

        if (squeeze != null)
            squeeze.Trigger();

        if (blink != null)
            blink.Trigger();

        if (shadow == null)
            shadow = Instantiate(shadowPrefab, transform.position - new Vector3(0, 0.25f, 0), Quaternion.identity);
    }

    private void Drop()
    {
        OnDrop.Invoke();

        DragAndDrop.Instance.UnRegister();
        isBeingDragged = false;
        isFalling = true;

        rb.velocity = Vector2.down * dropSpeed * Time.fixedDeltaTime;
    }
    public void DestroyShadow()
    {
        Destroy(shadow);
    }

    private void StopFalling()
    {
        OnFallGround.Invoke();

        rb.velocity = Vector2.zero;
        isFalling = false;
    }

    private void ShadowFollow()
    {
        //4.3 f min ground height size - 0.5 max ground height size
        shadow.transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y - 0.5f, -4.3f, 0.5f), 0);
        
        if (rb.velocity == Vector2.zero && !DragAndDrop.Instance.isDragging)
            Destroy(shadow);
    }

    private void ShadowScale()
    {
        ray = Physics2D.Raycast(transform.position, Vector2.down, 1000f, shadowInteractMask);
        float shadowScale = Mathf.Clamp((10 - ray.distance) / 10, 0.25f, 1f);

        if (shadow != null)
        {
            if (ray.distance > 10f)
                shadow.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            else if (ray.distance <= 10f && ray.distance > 0f)
                shadow.transform.localScale = new Vector3(shadowScale, shadowScale, shadowScale);
        }
    }
}
