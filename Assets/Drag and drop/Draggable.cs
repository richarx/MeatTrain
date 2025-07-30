using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private SqueezeAndStretch squeeze;
    private Blink blink;

    private void Start()
    {
        squeeze = GetComponent<SqueezeAndStretch>();
        blink = GetComponent<Blink>();
    }

    private void OnMouseDown()
    {
        DragAndDrop.Instance.Register(this.gameObject);
        squeeze.Trigger();
        blink.Trigger();
    }

    private void OnMouseUp()
    {
        DragAndDrop.Instance.UnRegister();
    }
}
