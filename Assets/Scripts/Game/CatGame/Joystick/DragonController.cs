using System.Collections;
using System.Collections.Generic;
using EasyUI.Toast;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragonController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private FixedJoystick fixedJoystick;
    private Rigidbody rigidBody;
    private Slider slider;
    private bool spawned = false;
    private Vector3 pos_ball;
    private float min_ball_pos;
    private float max_ball_pos;
    private float slider_val = 0.001f;

    private void OnEnable(){
        slider = FindObjectOfType<Slider>();
        slider.onValueChanged.AddListener(delegate{ValueChangeChecked();});
        fixedJoystick = FindObjectOfType<FixedJoystick>();
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }
    private void ValueChangeChecked(){
        if(!spawned){
            pos_ball = transform.position;
            min_ball_pos = pos_ball.y - 1.0f;
            max_ball_pos = pos_ball.y + 2.5f;
            spawned = true;
        }
        slider_val = slider.value;
        update_y_value();
    }
    private void update_y_value(){
        pos_ball.x = transform.position.x;
        pos_ball.z = transform.position.z;
        pos_ball.y = map(slider_val);
        transform.position = pos_ball;
    }
    private float map(float val){
        float min_slider_val = 0.15f;
        float max_slider_val = 1.0f;
        return (val - min_slider_val) / (max_slider_val - min_slider_val) * (max_ball_pos - min_ball_pos) + min_ball_pos;
    }
    private void FixedUpdate(){
        float xVal = fixedJoystick.Horizontal;
        float yVal = fixedJoystick.Vertical;
        Vector3 movement = new Vector3(xVal, 0, yVal);
        rigidBody.velocity = movement * speed;
        if(xVal != 0 && yVal != 0){
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.Atan2(xVal, yVal) * Mathf.Rad2Deg, transform.eulerAngles.z);
        }
    }


    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Spawnable")){
            var text = collision.gameObject.GetComponentInChildren<TextMeshProUGUI>();
            Toast.Show(text.text, 2f);
            Debug.Log("touched");
            // rigidBody.velocity = Vector3.zero;
            rigidBody.rotation = Quaternion.identity;
            Destroy(collision.gameObject);
        }
    }

}
