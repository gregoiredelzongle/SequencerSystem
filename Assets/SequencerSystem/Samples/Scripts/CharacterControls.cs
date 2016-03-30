using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterControls : MonoBehaviour {

    public float speed = 2.0f;

    public bool IsMoving { get { return rb2D.velocity != Vector2.zero; } }

    Rigidbody2D rb2D;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

	// Update is called once per frame
	void Update ()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * speed;
        rb2D.velocity = input;
	}

    void OnDisable()
    {
        rb2D.velocity = Vector2.zero;
    }
}
