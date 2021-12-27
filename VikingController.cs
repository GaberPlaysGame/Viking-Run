using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VikingController : MonoBehaviour
{
    [SerializeField] bool run, jump, left, right, dead = false;
    bool ske_on = true;

    [SerializeField] float slowly_go_back = 0.7f;
    [SerializeField] float var_gravity = 0.098f;
    [SerializeField] float var_jump_force = 30f;
    [SerializeField] float var_run_speed = 10;
    [SerializeField] float var_terminal_speed = -15f;
    public float velocity_x = 0f;
    public float velocity_y = 0f;
    public float velocity_z = 0f;
    float rot_vel = 0f;
    float start_time;
    int time;
    int return_count = 0;
    [SerializeField] private float target_angle = 90, angle = 90;

    Animator ani_vik, ani_ske;
    Button go_menu;
    GameObject ui;
    CharacterController cc_vik, cc_ske;
    public GameObject skeleton;
    public GameObject viking;
    public AudioSource music;
    Vector3 vec_enemy_at;
    Text go_enter;
    Text go_move;
    Text go_jump;
    Text go_turn;
    Text go_score;
    Text go_dead;
    Text go_endscore;

    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();
        music.Play();
        skeleton = GameObject.Find("Antagonist");
        ui = GameObject.Find("Canvas");
        viking = transform.GetChild(0).gameObject;

        Platform_Queue pq = new Platform_Queue();

        left = false;
        right = false;
        run = false;
        jump = false;

        cc_vik = viking.GetComponent<CharacterController>();
        cc_ske = skeleton.GetComponent<CharacterController>();

        ani_vik = viking.GetComponent<Animator>();
        ani_ske = cc_ske.GetComponent<Animator>();

        foreach (Transform child in ui.transform)
        {
            if (child.name == "Text_enter")
                //foreach (Transform child2 in child.transform)
                //{
                //if (child2.name == "Text_enter")
                go_enter = child.GetComponent<Text>();
            //}
            else if (child.name == "Text_turn")
                go_turn = child.GetComponent<Text>();
            else if (child.name == "Text_move")
                go_move = child.GetComponent<Text>();
            else if (child.name == "Text_jump")
                go_jump = child.GetComponent<Text>();
            else if (child.name == "Text_score")
                go_score = child.GetComponent<Text>();
            else if (child.name == "Text_dead")
                go_dead = child.GetComponent<Text>();
            else if (child.name == "Text_endscore")
                go_endscore = child.GetComponent<Text>();
            else if (child.name == "Button_menu")
                go_menu = child.GetComponent<Button>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            //Ray downray = new Ray(transform.position, )
            if (!Physics.Raycast(viking.transform.position, Vector3.down, 0.3f))
            {
                velocity_y -= var_gravity;
                jump = true;
            }
            else
            {
                velocity_y = 0f;
                jump = false;
            }

            if (Physics.Raycast(viking.transform.position, viking.transform.forward, 1.2f))
            {
                Debug.Log("True");
                ske_on = true;
                skeleton.SetActive(true);
                if (run)
                    slowly_go_back -= 2.5f * Time.deltaTime;
                if (slowly_go_back >= 3f)
                    slowly_go_back = 3f;
            }
            else
            {
                if (run)
                    slowly_go_back += 0.7f * Time.deltaTime;
            }
            if (ske_on)
            {
                if (Vector3.Distance(viking.transform.position, skeleton.transform.position) <= 1f)
                {
                    dead = true;
                    ani_vik.SetBool("st_dead", dead);
                    ani_ske.SetBool("st_dead", dead);
                }
            }
            if (viking.transform.position.y <= 0)
            {
                dead = true;
                ani_vik.SetBool("st_dead", dead);
            }


            if (Input.GetKeyDown(KeyCode.Return))
            {
                return_count++;

                switch (return_count)
                {
                    case 1:
                        go_enter.gameObject.SetActive(false);
                        go_move.gameObject.SetActive(true);
                        break;
                    case 2:
                        go_move.gameObject.SetActive(false);
                        go_turn.gameObject.SetActive(true);
                        break;
                    case 3:
                        go_turn.gameObject.SetActive(false);
                        go_jump.gameObject.SetActive(true);
                        break;
                    case 4:
                        go_jump.gameObject.SetActive(false);
                        run = true;
                        velocity_x = 10f;
                        Debug.Log("Enter pressed");
                        start_time = Time.time;
                        break;
                }
            }

            if (run)
            {
                cc_vik.Move(velocity_x * Time.deltaTime * viking.transform.forward);
                //cc_ske.Move(velocity_x * Time.deltaTime * skeleton.transform.forward);

                if (velocity_y <= 0 && velocity_y >= -1.0045 && jump == false)
                {
                    jump = false;
                }

                if (Input.GetKey(KeyCode.Space))
                {
                    Debug.Log("Enter jump mode");
                    if (!jump)
                    {
                        jump = true;
                        velocity_y += var_jump_force;
                    }
                }

                if (Input.GetKey(KeyCode.A))
                    velocity_z = -5f;
                else if (Input.GetKey(KeyCode.D))
                    velocity_z = 5f;
                else
                    velocity_z = 0f;

                if (Input.GetKeyDown(KeyCode.LeftArrow) && !left && !right)
                {
                    if (Mathf.Abs(angle) <= 0.01 || Mathf.Abs(angle - 360) <= 0.01)
                        target_angle = 270;
                    else if (Mathf.Abs(angle - 90) <= 0.01)
                        target_angle = 0;
                    else
                        target_angle = viking.transform.eulerAngles.y - 90;
                    left = true;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) && !left && !right)
                {
                    if (Mathf.Abs(angle) <= 0.01 || Mathf.Abs(angle - 360) <= 0.01)
                        target_angle = 90;
                    else
                        target_angle = viking.transform.eulerAngles.y + 90;
                    right = true;
                }

                ani_vik.SetBool("st_jump", jump);
            }

            if (velocity_y < -15f)
                velocity_y = -15f;

            if (left)
            {
                angle = Mathf.SmoothDampAngle(viking.transform.eulerAngles.y, target_angle, ref rot_vel, 0.1f);
                if (Mathf.Abs(angle - target_angle) <= 0.01)
                {
                    angle = target_angle;
                    left = false;
                }
            }
            else if (right)
            {
                angle = Mathf.SmoothDampAngle(viking.transform.eulerAngles.y, target_angle, ref rot_vel, 0.1f);
                if (Mathf.Abs(angle - target_angle) <= 0.01)
                {
                    angle = target_angle;
                    right = false;
                }
            }

            viking.transform.eulerAngles = new Vector3(0, angle, 0);
            Vector3 vec_right = Vector3.back;
            switch (target_angle)
            {
                case 0:
                case 360:
                    vec_right = Vector3.right;
                    if (run)
                        vec_enemy_at = new Vector3(0, 0, -4.6f - slowly_go_back);
                    else
                        vec_enemy_at = new Vector3(0, 0, -4.6f);
                    break;
                case 90:
                    vec_right = Vector3.back;
                    if (run)
                        vec_enemy_at = new Vector3(-4.6f - slowly_go_back, 0, 0);
                    else
                        vec_enemy_at = new Vector3(-4.6f, 0, 0);
                    break;
                case 180:
                    vec_right = Vector3.left;
                    if (run)
                        vec_enemy_at = new Vector3(0, 0, 4.6f + slowly_go_back);
                    else
                        vec_enemy_at = new Vector3(0, 0, 4.6f);
                    break;
                case 270:
                    vec_right = Vector3.forward;
                    if (run)
                        vec_enemy_at = new Vector3(4.6f + slowly_go_back, 0, 0);
                    else
                        vec_enemy_at = new Vector3(4.6f, 0, 0);
                    break;
            }

            cc_vik.Move(velocity_y * Time.deltaTime * Vector3.up);
            //cc_ske.Move(velocity_y * Time.deltaTime * Vector3.up);
            cc_vik.Move(velocity_z * Time.deltaTime * vec_right);
            //cc_ske.Move(velocity_z * Time.deltaTime * vec_right);

            if (ske_on)
            {
                skeleton.transform.position = viking.transform.position + vec_enemy_at;
                skeleton.transform.eulerAngles = viking.transform.eulerAngles;
            }
            if (Mathf.Abs(skeleton.transform.position.x - viking.transform.position.x) >= 7.6 ||
                Mathf.Abs(skeleton.transform.position.z - viking.transform.position.z) >= 7.6)
            {
                skeleton.SetActive(false);
                ske_on = false;
            }

            ani_vik.SetBool("st_run", run);
            ani_ske.SetBool("st_run", run);

            float curr_time = Time.time;
            time = (int)(curr_time - start_time);
            if (run)
                go_score.text = "Surviving Time: " + time;
        }
        else
        {
            Platform_Queue.Clear();
            go_dead.gameObject.SetActive(true);
            go_menu.gameObject.SetActive(true);
            go_endscore.gameObject.SetActive(true);
            go_endscore.text = "Final Score: " + (Money_Destroy.money * 3 + time * 5);
            Money_Destroy.money = 0;
        }
    }
}
