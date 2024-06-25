using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyUI.Toast;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WestDragonController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Button upButton;
    private Button downButton;
    private FixedJoystick fixedJoystick;
    private Rigidbody rigidBody;
    private Slider slider;
    private bool spawned = false;
    private Vector3 pos_ball;
    private float min_ball_pos;
    private float max_ball_pos;
    private float slider_val = 0.001f;

    private void OnEnable(){
        // slider = FindObjectOfType<Slider>();
        // slider.onValueChanged.AddListener(delegate{ValueChangeChecked();});
        fixedJoystick = FindObjectOfType<FixedJoystick>();
        rigidBody = gameObject.GetComponent<Rigidbody>();
        upButton = GameObject.Find("ButtonUp").GetComponent<Button>();
        downButton = GameObject.Find("ButtonDown").GetComponent<Button>();
    }
    void Start(){
        var upButtonHoldAndRelease = upButton.GetComponent<ButtonHoldAndRelease>();
        var downButtonHoldAndRelease = downButton.GetComponent<ButtonHoldAndRelease>();
        upButtonHoldAndRelease.OnButtonDownEvent += StartUp;
        upButtonHoldAndRelease.OnButtonHoldEvent += IncreaseHeight;
        upButtonHoldAndRelease.OnButtonUpEvent += ReleaseButton;
        downButtonHoldAndRelease.OnButtonDownEvent += StartDown;
        downButtonHoldAndRelease.OnButtonHoldEvent += DecreaseHeight;
        downButtonHoldAndRelease.OnButtonUpEvent += ReleaseButton;
        pos_ball = transform.position;
    }
    void StartUp()
    {
        // pos_ball.x = transform.position.x;
        // pos_ball.z = transform.position.z;
        // // Debug.Log(transform.position.x);
        // // Debug.Log(transform.position.z);
        // pos_ball.y = transform.position.y + 0.05f;
        // transform.position = pos_ball.normalized;
    }
    void StartDown()
    {
        // pos_ball.x = transform.position.x;
        // pos_ball.z = transform.position.z;
        // pos_ball.y -= 0.05f;
        // transform.position = pos_ball.normalized;
    }
    void IncreaseHeight()
    {
        pos_ball.x = transform.position.x;
        pos_ball.z = transform.position.z;
        pos_ball.y += 0.025f;
        transform.position = pos_ball;
    }
    void DecreaseHeight()
    {
        pos_ball.x = transform.position.x;
        pos_ball.z = transform.position.z;
        pos_ball.y -= 0.025f;
        transform.position = pos_ball;
    }
    void ReleaseButton()
    {
        
    }
    // private void ValueChangeChecked(){
    //     if(!spawned){
    //         pos_ball = transform.position;
    //         min_ball_pos = pos_ball.y - 1.0f;
    //         max_ball_pos = pos_ball.y + 2.5f;
    //         spawned = true;
    //     }
    //     slider_val = slider.value;
    //     update_y_value();
    // }
    // private void update_y_value(){
    //     pos_ball.x = transform.position.x;
    //     pos_ball.z = transform.position.z;
    //     pos_ball.y = map(slider_val);
    //     transform.position = pos_ball;
    // }
    // private float map(float val){
    //     float min_slider_val = 0.15f;
    //     float max_slider_val = 4.0f;
    //     return (val - min_slider_val) / (max_slider_val - min_slider_val) * (max_ball_pos - min_ball_pos) + min_ball_pos;
    // }
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
            var anim = collision.gameObject.GetComponentInChildren<ParticleSystem>();
            anim.Play(true);
            // float totalDuration = anim.duration + anim.startLifetime;
            // Toast.Show(text.text, 2f);
            // rigidBody.velocity = Vector3.zero;
            rigidBody.rotation = Quaternion.identity;
            
            CatGameManager.GetInstance().CheckAnswer(text.text);

            this.GameObject().transform.localScale *= 1.1f;
            Destroy(collision.gameObject,0.35f);
        }
    }

}
